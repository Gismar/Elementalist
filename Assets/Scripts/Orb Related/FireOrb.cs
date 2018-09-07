using UnityEngine;

namespace Orb
{
    public class FireOrb : OrbBehaviour
    {
        [SerializeField] private GameObject _lineHolder;
        [SerializeField] private GameObject _fireball;
        [SerializeField] private GameObject _ringOfFire;

        protected override float _damage { get; set; }
        protected override float _mainAttackDelay { get; } = 1f;
        protected override float _secondaryAttackDelay { get; } = 5f;


        void Start()
        {
            _lineHolder = transform.GetChild(0).gameObject;
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
            _state = State.Idling;
            _lineHolder.SetActive(false);
            _damage = 20 * _globalData.OrbDamage;
            CreateFireball(MouseAngle());
            CreateFireball(MouseAngle() - 180f);
        }

        protected override void SecondaryAttack()
        {
            _damage = 25 * _globalData.OrbDamage;
            CreateRingOfFire();
        }

        protected override void UpdateAimLine()
        {
            _lineHolder.SetActive(true);
            _lineHolder.transform.rotation = Quaternion.Euler(0, 0, MouseAngle());
            var children = _lineHolder.GetComponentsInChildren<LineRenderer>();
            children[0].startWidth = transform.localScale.x;
            children[1].startWidth = transform.localScale.x;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<Enemy.EnemyBehaviour>().TakeDamage(5 * Time.deltaTime);
            }
        }

        private void CreateFireball(float rotation)
        {
            var temp = Instantiate(_fireball);
            temp.transform.position = transform.position;
            temp.transform.localScale = transform.localScale;
            temp.transform.rotation = Quaternion.Euler(0, 0, rotation);
            temp.GetComponent<Fireball>().Setup(_damage, _globalData.OrbDistance * Mathf.Min(transform.localScale.x, 1f));
        }

        private void CreateRingOfFire()
        {
            var temp = Instantiate(_ringOfFire);
            temp.transform.position = transform.position;
            temp.transform.localScale = transform.localScale;
            temp.GetComponent<FireRing>().Setup(_damage, _globalData.OrbDistance * Mathf.Min(transform.localScale.x, 1f));
        }
    }
}
