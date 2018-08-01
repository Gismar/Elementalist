using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbBehaviour : MonoBehaviour {
    protected IOrb _Orb;
    protected Vector2 _Offset;
    protected Transform _Player;
    protected GlobalDataHandler _GlobalData;
    protected bool _IsAttacking = false;
    protected bool _BeganAim = false;
    protected bool _IsIdle = true;
    protected float[] _MainAttackTimers;
    protected float[] _SecondaryAttackTimers;
    protected int _OrbType;

    private bool _Return;
    private bool _CanUseMainAttack = false;
    private bool _CanUseSecondary = false;
    private float _IdleTimerLerp;
    private float _MainAttackTimer;
    private float _SecondaryAttackTimer;

    protected void Startup()
    {
        _SecondaryAttackTimer = _SecondaryAttackTimers[_OrbType];
        _MainAttackTimer = _MainAttackTimers[_OrbType];
    }

    void Update () {
        if (_IsIdle)
        {
            transform.position = new Vector2(_Player.position.x + Mathf.Cos(Time.time + _Offset.x) * transform.localScale.x,
                                            _Player.position.y + Mathf.Sin(Time.time + _Offset.y) * transform.localScale.y);
        }
        if (_Return)
        {
            transform.position = Vector3.Lerp(transform.position, _Player.position, _IdleTimerLerp);
            _IdleTimerLerp += Time.deltaTime / Vector3.Distance(transform.position, _Player.position);
            if (_IdleTimerLerp >= 1)
            {
                _IsIdle = true;
                _Return = false;
            }
        }

        if (!_CanUseSecondary)
        {
            _CanUseSecondary = _SecondaryAttackTimer <= Time.time;
            transform.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(0.7f, 0.7f, 0.7f, 0.7f), (_SecondaryAttackTimer - Time.time) / _Orb.SecondaryAttackDelay);
        }

        if (!_CanUseMainAttack)
        {
            _CanUseMainAttack = _MainAttackTimer <= Time.time;
        }

        if (_IsAttacking) return;

        if (Input.GetKeyDown(_GlobalData.Swap))
        {
            _OrbType = _OrbType + 1 == 2 ? 0 : _OrbType + 1;
            _Orb.Swap(_OrbType);
        }

        if (Input.GetKeyDown(_GlobalData.Recall) && !_IsIdle)
        {
            _Return = true;
            _Orb.SetIdle();
            _IdleTimerLerp = 0;
        }

        if (Input.GetMouseButtonDown(1) && _CanUseSecondary)
        {
            SetUpSecondaryAttackTimer();
            _IsAttacking = true;
            _Orb.SecondaryAttack();
        }

        if (!_CanUseMainAttack) return;

        if (Input.GetMouseButtonDown(0))
        {
            _Orb.ActivateAimLine();
        }

        if (Input.GetMouseButton(0))
        {
            _Orb.UpdateAimLine();
            _IsIdle = false;
            _Return = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _IsAttacking = true;
            _BeganAim = false;
            _Orb.MainAttack();
            SetUpMainAttackTimer();
        }

    }

    private void SetUpMainAttackTimer()
    {
        _MainAttackTimer = Time.time + _Orb.MainAttackDelay;
        _MainAttackTimers[_OrbType] = _MainAttackTimer;
        _CanUseMainAttack = false;
    }

    private void SetUpSecondaryAttackTimer()
    {
        _SecondaryAttackTimer = Time.time + _Orb.SecondaryAttackDelay;
        _SecondaryAttackTimers[_OrbType] = _SecondaryAttackTimer;
        _CanUseSecondary = false;
    }

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
}
