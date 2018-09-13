using Elementalist.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elementalist.Orb
{
    public class AirOrb : OrbBehaviour
    {
        private AnimationCurve _bellCurve;
        private LineRenderer _aimLine;
        private Vector2 _targetPosition;
        private Vector2 _startingPosition;
        private Vector2 _targetSize;
        private Vector2 _startingSize;
        private float _mainAttackLerptimer;
        private float _secondaryAttackLerpTimer;
        private float _distance;
        private bool _isMainAttacking;
        private bool _isSecondaryAttacking;

        protected override float _damage { get; set; }
        protected override float _mainAttackDelay { get; } = 1f;
        protected override float _secondaryAttackDelay { get; } = 5f;

        private void Start()
        {
            _aimLine = GetComponent<LineRenderer>();
            _bellCurve = new AnimationCurve(
                new Keyframe(0, 0),
                new Keyframe(0.5f, 1),
                new Keyframe(1, 0));
        }

        protected override void Setup(OrbSetup orbSetup)
        {
            _swapTimer = orbSetup.SwapTimer;
            _decay = orbSetup.Decay;
            _player = orbSetup.Player;
            _globalData = orbSetup.GlobalData;
            _state = orbSetup.OrbState;
            _mainAttackTimers = orbSetup.MainAttackTimers;
            _secondaryAttackTimers = orbSetup.SecondaryAttackTimers;
            _orbType = orbSetup.OrbType;
            Startup();
        }

        protected override void MainAttack()
        {
            UpdateAimLine();
            _mainAttackLerptimer = 0;
            _isMainAttacking = true;
            _aimLine.enabled = false;
            _damage = 5f * _globalData.OrbDamage;
            _startingPosition = transform.position;
        }

        protected override void SecondaryAttack()
        {
            _secondaryAttackLerpTimer = 0;
            _isSecondaryAttacking = true;
            _damage = 20f * _globalData.OrbDamage;
            _startingSize = transform.localScale;
            _targetSize = _globalData.OrbSize * 5f;
        }

        protected override void UpdateAimLine()
        {
            _distance = Mathf.Clamp(MouseDistance(), 0f, _globalData.OrbDistance * 10f) / _globalData.OrbSize.x;
            var angle = (MouseAngle() + 90) * Mathf.Deg2Rad;

            _aimLine.enabled = true;

            var target = new Vector3(_distance * Mathf.Cos(angle) + transform.position.x, _distance * Mathf.Sin(angle) + transform.position.y, 0);

            _targetPosition = target;
            _aimLine.SetPosition(1, (target - transform.position) / _globalData.OrbSize.x);
            _aimLine.SetPosition(2, (_player.position - transform.position) / _globalData.OrbSize.x);
        }

        protected override void Update()
        {
            base.Update();

            if (_isMainAttacking)
            {
                MainAttackFunctionality();
            }

            if (_isSecondaryAttacking)
            {
                SecondaryAttackFunctionality();
            }
        }

        private void SecondaryAttackFunctionality()
        {
            _secondaryAttackLerpTimer += Time.deltaTime;
            transform.localScale = Vector2.Lerp(_startingSize, _targetSize, _bellCurve.Evaluate(_secondaryAttackLerpTimer));
            if (_secondaryAttackLerpTimer >= 1)
            {
                _isSecondaryAttacking = false;
            }
        }

        private void MainAttackFunctionality()
        {
            _mainAttackLerptimer += Time.deltaTime;

            var position = _mainAttackLerptimer <= 0.5f ? _startingPosition : (Vector2)_player.position;
            transform.position = Vector2.Lerp(position, _targetPosition, _bellCurve.Evaluate(_mainAttackLerptimer));

            if (_mainAttackLerptimer >= 1)
            {
                _state = State.Orbiting;
                _isMainAttacking = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                var enemy = collision.GetComponent<EnemyBehaviour>();
                enemy.TakeDamage(_damage);
                var dir = ((_player.transform.position + transform.position) / -2f).normalized;
                enemy.KnockBack(_damage, dir);
            }
        }
    }
}