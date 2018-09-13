using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum OrbElement
{
    Water,
    Fire,
    Lightning,
    Earth,
    Air
}

public enum State
{
    Orbiting,
    Idling,
    Returning,
    Attacking,
    Aiming
}

namespace Elementalist.Orb
{
    public abstract class OrbBehaviour : MonoBehaviour
    {
        //For inherited orbs
        protected abstract void MainAttack();
        protected abstract void UpdateAimLine();
        protected abstract void SecondaryAttack();
        protected abstract void Setup(OrbSetup orbSetup);
        protected abstract float _secondaryAttackDelay { get; }
        protected abstract float _mainAttackDelay { get; }
        protected abstract float _damage { get; set; }

        protected SpriteRenderer _spriteRenderer;
        protected GlobalDataHandler _globalData;
        protected Transform _player;
        protected OrbElement _orbType;
        protected State _state;
        protected float _decay;
        protected float _swapTimer;
        protected float[] _mainAttackTimers;
        protected float[] _secondaryAttackTimers;

        private Dictionary<KeyCode, int> SwapKeys;
        private bool _canUseMainAttack = false;
        private bool _canUseSecondary = false;
        private float _secondaryAttackTimer;
        private float _mainAttackTimer;
        private float _idleTimerLerp;

        //Custom constructor
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


        protected virtual void Update()
        {
            //Checks State
            switch (_state)
            {
                case State.Orbiting:
                    transform.position = new Vector2(_player.position.x + Mathf.Cos(Time.time) * transform.localScale.x,
                        _player.position.y + Mathf.Sin(Time.time) * transform.localScale.y);
                    break;

                case State.Returning:
                    transform.position = Vector3.Lerp(transform.position, _player.position, _idleTimerLerp);
                    _idleTimerLerp += Time.deltaTime / Vector3.Distance(transform.position, _player.position);
                    if (_idleTimerLerp >= 1)
                    {
                        transform.localScale = _globalData.OrbSize;
                        _decay = 0;
                        _state = State.Orbiting;
                    }
                    break;

                case State.Idling:
                    transform.localScale = Vector2.Lerp(_globalData.OrbSize, new Vector2(0.25f, 0.25f), _decay / 10f);
                    _decay += Time.deltaTime;

                    if (Input.GetKeyDown(_globalData.Keys[GlobalDataHandler.Key.Recall]))
                    {
                        _state = State.Returning;
                        _idleTimerLerp = 0;
                    }
                    break;
            }

            // Cooldowns
            if (!_canUseSecondary)
            {
                _canUseSecondary = _secondaryAttackTimer <= Time.time;
                transform.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(0.7f, 0.7f, 0.7f, 0.7f), (_secondaryAttackTimer - Time.time) / _secondaryAttackDelay);
            }
            if (!_canUseMainAttack)
            {
                _canUseMainAttack = _mainAttackTimer <= Time.time;
            }

            //Secondary Attack
            if (Input.GetMouseButtonDown(1) && _canUseSecondary)
            {
                SetUpSecondaryAttackTimer();
                SecondaryAttack();
            }

            //Swapping
            if (_state != State.Attacking && _swapTimer < Time.time)
            {
                var keyPressed = SwapKeys.FirstOrDefault(k => Input.GetKeyDown(k.Key));
                if (!keyPressed.Equals(default(KeyValuePair<KeyCode, int>)))
                {
                    _orbType = (OrbElement)keyPressed.Value;
                    Swap();
                }

                var scroll = Input.GetAxis("Mouse ScrollWheel");
                if(scroll != 0) ScrollSwap(Mathf.Sign(scroll));
            }

            //Mid Attack check to prevent 2 attacks at once
            if (!_canUseMainAttack) return;

            //Main Attack
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

        //So that other objects can just call it
        public void SetupPublic(OrbSetup orbSetup)
        {
            Setup(orbSetup);
        }

        //Cooldown setups
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

        //Calculations for Mouse relative to game window
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

        //Swap Methods
        private void ScrollSwap(float scroll)
        {
            if (scroll == 1)
                _orbType = (int)_orbType + 1 == _globalData.OrbPrefabs.Length ? 0 : _orbType + 1;
            else if (scroll == -1)
                _orbType = (int)_orbType - 1 == -1 ? (OrbElement)_globalData.OrbPrefabs.Length - 1 : _orbType - 1;
            Swap();
        }
        public void Swap()
        {
            var orb = Instantiate(_globalData.OrbPrefabs[(int)_orbType]);
            orb.transform.position = transform.position;
            var orbSetup = new OrbSetup
            (
                Time.time + 0.1f,
                _decay,
                _player,
                _globalData,
                _state,
                _mainAttackTimers,
                _secondaryAttackTimers,
                _orbType
            );

            orb.GetComponent<OrbBehaviour>().Setup(orbSetup);
            Destroy(transform.gameObject);
        }
    }
}