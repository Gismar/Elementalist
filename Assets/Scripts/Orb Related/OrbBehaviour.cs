using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum OrbElement
{
    Water, Fire, Lightning, Earth, Air
}
public enum State
{
    Orbiting,
    Idling,
    Returning,
    Attacking,
    Aiming
}

public abstract class OrbBehaviour : MonoBehaviour {


    protected Transform _player;
    protected GlobalDataHandler _globalData;
    protected OrbElement _orbType;
    protected State _state;
    protected float[] _mainAttackTimers;
    protected float[] _secondaryAttackTimers;
    protected float _decay;
    protected abstract float _damage { get; set; }
    protected abstract float _mainAttackDelay { get; }
    protected abstract float _secondaryAttackDelay { get; }

    public Dictionary<KeyCode, int> SwapKeys;
    
    private bool _canUseMainAttack = false;
    private bool _canUseSecondary = false;
    private float _idleTimerLerp;
    private float _mainAttackTimer;
    private float _secondaryAttackTimer;

    protected void Startup()
    {
        _secondaryAttackTimer = _secondaryAttackTimers[(int)_orbType];
        _mainAttackTimer = _mainAttackTimers[(int)_orbType];
        SwapKeys = new Dictionary<KeyCode, int>
        {
            [_globalData.Keys[GlobalDataHandler.Key.Water]] = 0,
            [_globalData.Keys[GlobalDataHandler.Key.Fire]] = 1,
            [_globalData.Keys[GlobalDataHandler.Key.Lightning]] = 2,
            [_globalData.Keys[GlobalDataHandler.Key.Earth]] = 3,
            [_globalData.Keys[GlobalDataHandler.Key.Air]] = 4
        };
    }

    protected virtual void Update() {
        switch (_state)
        {
            case State.Orbiting:
                {
                    transform.position = new Vector2(_player.position.x + Mathf.Cos(Time.time) * transform.localScale.x,
                        _player.position.y + Mathf.Sin(Time.time) * transform.localScale.y);
                    break;
                }
            case State.Returning:
                {
                    transform.position = Vector3.Lerp(transform.position, _player.position, _idleTimerLerp);
                    _idleTimerLerp += Time.deltaTime / Vector3.Distance(transform.position, _player.position);

                    if (_idleTimerLerp >= 1)
                    {
                        transform.localScale = _globalData.OrbSize;
                        _decay = 0;
                        _state = State.Orbiting;
                    }
                    break;
                }
            case State.Idling:
                {
                    transform.localScale = Vector2.Lerp(_globalData.OrbSize, new Vector2(0.25f, 0.25f), _decay / 10f);
                    _decay += Time.deltaTime;

                    if (Input.GetKeyDown(_globalData.Keys[GlobalDataHandler.Key.Recall]))
                    {
                        _state = State.Returning;
                        _idleTimerLerp = 0;
                    }
                    break;
                }
        }

        if (!_canUseSecondary)
        {
            _canUseSecondary = _secondaryAttackTimer <= Time.time;
            transform.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(0.7f, 0.7f, 0.7f, 0.7f), (_secondaryAttackTimer - Time.time) / _secondaryAttackDelay);
        }

        if (!_canUseMainAttack)
        {
            _canUseMainAttack = _mainAttackTimer <= Time.time;
        }

        if (Input.GetMouseButtonDown(1) && _canUseSecondary)
        {
            SetUpSecondaryAttackTimer();
            SecondaryAttack();
        }


        var keyPressed = SwapKeys.FirstOrDefault(k => Input.GetKeyDown(k.Key));
        if (_state != State.Attacking)
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            scroll = scroll == 0f ? 0 : Mathf.Sign(scroll);
            Debug.Log(scroll);
            _orbType = (OrbElement)keyPressed.Value;
            Swap();
        }
        
        if (!_canUseMainAttack) return;

        if (Input.GetMouseButton(0))
        {
            UpdateAimLine();
            _state = State.Aiming;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _state = State.Attacking;
            MainAttack();
            SetUpMainAttackTimer();
        }
    }
    
    protected abstract void MainAttack();
    protected abstract void SecondaryAttack();
    protected abstract void UpdateAimLine();
    protected abstract void Setup(OrbSetup orbSetup);

    public void SetupPublic(OrbSetup orbSetup)
    {
        Setup(orbSetup);
    }

    private void SetUpMainAttackTimer()
    {
        _mainAttackTimer = Time.time + _mainAttackDelay;
        _mainAttackTimers[(int)_orbType] = _mainAttackTimer;
        _canUseMainAttack = false;
    }
    private void SetUpSecondaryAttackTimer()
    {
        _secondaryAttackTimer = Time.time + _secondaryAttackDelay;
        _secondaryAttackTimers[(int)_orbType] = _secondaryAttackTimer;
        _canUseSecondary = false;
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

    public void Swap()
    {
        GameObject temp = Instantiate(_player.GetComponent<PlayerMovement>().Orbs[(int)_orbType]);
        if (temp == null) return;
        temp.transform.position = transform.position;
        var orbSetup = new OrbSetup
        (
            _decay, 
            _player, 
            _globalData, 
            _state, 
            _mainAttackTimers,
            _secondaryAttackTimers, 
            _orbType
        );
        temp.GetComponent<OrbBehaviour>().Setup(orbSetup);
        Destroy(transform.gameObject);
    }
}
