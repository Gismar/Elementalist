using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbBehaviour : MonoBehaviour {
    protected IOrb _orb;
    protected Vector2 _offset;
    protected Transform _player;
    protected GlobalDataHandler _globalData;
    protected bool _isAttacking = false;
    protected bool _beganAim = false;
    protected bool _isIdle = true;
    protected float[] _mainAttackTimers;
    protected float[] _secondaryAttackTimers;
    protected int _orbType;

    private bool _return;
    private bool _canUseMainAttack = false;
    private bool _canUseSecondary = false;
    private float _idleTimerLerp;
    private float _mainAttackTimer;
    private float _secondaryAttackTimer;

    protected void Startup()
    {
        _secondaryAttackTimer = _secondaryAttackTimers[_orbType];
        _mainAttackTimer = _mainAttackTimers[_orbType];
    }

    void Update () {
        if (_isIdle)
        {
            transform.position = new Vector2(_player.position.x + Mathf.Cos(Time.time + _offset.x) * transform.localScale.x,
                                            _player.position.y + Mathf.Sin(Time.time + _offset.y) * transform.localScale.y);
        }
        if (_return)
        {
            transform.position = Vector3.Lerp(transform.position, _player.position, _idleTimerLerp);
            _idleTimerLerp += Time.deltaTime / Vector3.Distance(transform.position, _player.position);
            if (_idleTimerLerp >= 1)
            {
                _isIdle = true;
                _return = false;
            }
        }

        if (!_canUseSecondary)
        {
            _canUseSecondary = _secondaryAttackTimer <= Time.time;
            transform.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(0.7f, 0.7f, 0.7f, 0.7f), (_secondaryAttackTimer - Time.time) / _orb.SecondaryAttackDelay);
        }

        if (!_canUseMainAttack)
        {
            _canUseMainAttack = _mainAttackTimer <= Time.time;
        }

        if (_isAttacking) return;

        if (Input.GetKeyDown(_globalData.Swap))
        {
            _orbType = _orbType + 1 == _player.GetComponent<PlayerMovement>().Orbs.Length ? 0 : _orbType + 1;
            Debug.Log(_orbType);
            _orb.Swap();
        }

        if (Input.GetKeyDown(_globalData.Recall) && !_isIdle)
        {
            _return = true;
            _orb.SetIdle();
            _idleTimerLerp = 0;
        }

        if (Input.GetMouseButtonDown(1) && _canUseSecondary)
        {
            SetUpSecondaryAttackTimer();
            _isAttacking = true;
            _orb.SecondaryAttack();
        }
        
        if (!_canUseMainAttack) return;

        if (Input.GetMouseButtonDown(0))
        {
            _orb.ActivateAimLine();
        }

        if (Input.GetMouseButton(0))
        {
            _orb.UpdateAimLine();
            _isIdle = false;
            _return = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isAttacking = true;
            _beganAim = false;
            _orb.MainAttack();
            SetUpMainAttackTimer();
        }
    }

    #region //Setup Timers
    private void SetUpMainAttackTimer()
    {
        _mainAttackTimer = Time.time + _orb.MainAttackDelay;
        _mainAttackTimers[_orbType] = _mainAttackTimer;
        _canUseMainAttack = false;
    }

    private void SetUpSecondaryAttackTimer()
    {
        _secondaryAttackTimer = Time.time + _orb.SecondaryAttackDelay;
        _secondaryAttackTimers[_orbType] = _secondaryAttackTimer;
        _canUseSecondary = false;
    }
    #endregion

    #region //Mouse Calculations
    protected float MouseAngle()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = (mousePos - transform.position).normalized;
        var rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        return rotation;
    }

    protected float MouseDistance()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var distance = Vector3.Distance(mousePos, transform.position);
        return distance;
    }
    #endregion

    public void Swap()
    {
        GameObject temp = null;
        switch (_orbType)
        {
            case 0:
                temp = Instantiate(_player.GetComponent<PlayerMovement>().Orbs[_orbType]);
                break;
            case 1:
                temp = Instantiate(_player.GetComponent<PlayerMovement>().Orbs[_orbType]);
                break;
            case 2:
                temp = Instantiate(_player.GetComponent<PlayerMovement>().Orbs[_orbType]);
                break;
            default: break;

        }
        if (temp == null) return;
        temp.transform.position = transform.position;
        temp.GetComponent<IOrb>().Setup(_offset, _player, _globalData, _isIdle, _mainAttackTimers, _secondaryAttackTimers, _orbType);
        Destroy(transform.gameObject);
    }
}
