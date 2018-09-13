using System.Collections.Generic;
using UnityEngine;
using Elementalist.Enemy;

namespace Elementalist.Orb
{
    public class LightningOrb : OrbBehaviour
    {
        [SerializeField] private GameObject _mainAttackPrefab;
        [SerializeField] private GameObject _secondaryAttackPrefab;
        [SerializeField] private LayerMask[] _mask;
        private LineRenderer _aimLine;
        private Vector2 _originalSize;

        protected override float _damage { get; set; }
        protected override float _mainAttackDelay { get; } = 0.5f;
        protected override float _secondaryAttackDelay { get; } = 3f;

        private void Start()
        {
            _aimLine = GetComponent<LineRenderer>();
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

        protected override void UpdateAimLine()
        {
            _aimLine.enabled = true;
            transform.rotation = Quaternion.Euler(0, 0, MouseAngle());
            _aimLine.endWidth = transform.localScale.x * _aimLine.widthMultiplier;
        }

        protected override void MainAttack()
        {
            _aimLine.enabled = false;
            var mouseAngle = MouseAngle();
            var temp = Instantiate(_mainAttackPrefab);
            temp.GetComponent<LightningOrbBeam>().Setup(_globalData.OrbDistance * 6f * Mathf.Min(transform.localScale.x, 1f), 15f * _globalData.OrbDamage, transform);
            temp.transform.rotation = Quaternion.Euler(0, 0, mouseAngle);
            temp.transform.position = transform.position;
            _originalSize = transform.localScale;
            transform.localScale = Vector2.zero;
        }

        protected override void SecondaryAttack()
        {
            _damage = _globalData.OrbDamage * 20f;
            int count = Mathf.FloorToInt(Mathf.Pow(_globalData.OrbDistance * 2f, 2f));
            List<EnemyBehaviour> enemiesHit = new List<EnemyBehaviour>();
            var hit = Physics2D.OverlapCircle(transform.position, transform.localScale.x * _globalData.OrbDistance * 3f, _mask[0]);
            Vector3 prevPosition = transform.position;
            Debug.Log(hit);
            for (int i = 0; i < count; i++)
            {
                if (hit == null) return;
                var temp = Instantiate(_secondaryAttackPrefab);
                temp.transform.position = (hit.transform.position + prevPosition) / 2f;

                Vector2 direction = (hit.transform.position - prevPosition).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                float radius = Vector3.Distance(hit.transform.position, prevPosition);
                temp.transform.rotation = Quaternion.Euler(0, 0, angle);

                temp.GetComponent<SpriteRenderer>().size = new Vector2(radius, 0.5f);
                var shape = temp.GetComponent<ParticleSystem>().shape;
                shape.radius = radius / 2f;

                Destroy(temp, 1f);
                hit.GetComponent<EnemyBehaviour>().TakeDamage(_damage);
                prevPosition = hit.transform.position;
                hit = Physics2D.OverlapCircle(hit.transform.position, transform.localScale.x * _globalData.OrbDistance, _mask[0]);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                var orbHit = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x, _mask[0]);
                foreach (Collider2D hit in orbHit)
                {
                    hit.GetComponent<EnemyBehaviour>().TakeDamage(_damage / orbHit.Length);
                }
            }
        }

        public void KillOrbling(Vector3 pos)
        {
            transform.position = pos;
            transform.localScale = _originalSize;
            _state = State.Idling;
        }
    }
}
