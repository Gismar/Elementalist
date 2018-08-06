using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningOrb : OrbBehaviour, IOrb {
    [SerializeField] private GameObject _mainAttackPrefab;
    [SerializeField] private GameObject _secondaryAttackPrefab;
    [SerializeField] private LayerMask[] _mask;
    private LineRenderer _aimLine;

    public float Damage { get; private set; }
    public float MainAttackDelay { get; private set; } = 0.5f;
    public float SecondaryAttackDelay { get; private set; } = 3f;

    private void Start()
    {
        _orb = this;
        transform.localScale = _globalData.OrbSize;
        _aimLine = GetComponent<LineRenderer>();
    }

    #region Main Functionality
    public void Setup(Vector2 offset, Transform player, GlobalDataHandler globalData, bool isIdle, float[] mainTimers, float[] secondTimers, int orbType)
    {
        _player = player;
        _offset = offset;
        _globalData = globalData;
        _isIdle = isIdle;
        _mainAttackTimers = mainTimers;
        _secondaryAttackTimers = secondTimers;
        _orbType = orbType;
        Startup();
    }

    public void SetIdle()
    {
        _beganAim = false;
        _isAttacking = false;
        _aimLine.enabled = false;
    }

    public void ActivateAimLine()
    {
        _aimLine.enabled = true;
        _beganAim = true;
    }

    public void UpdateAimLine()
    {
        if (!_beganAim) ActivateAimLine();
        transform.rotation = Quaternion.Euler(0, 0, MouseAngle());
    }

    public void MainAttack()
    {
        _aimLine.enabled = false;
        var mouseAngle = MouseAngle();
        var temp = Instantiate(_mainAttackPrefab);
        temp.GetComponent<LightningOrbBeam>().Setup(_globalData.OrbDistance * 6f, 15f * _globalData.OrbDamage, transform);
        temp.transform.rotation = Quaternion.Euler(0, 0, mouseAngle);
        temp.transform.position = transform.position;
        transform.localScale = Vector2.zero;
    }

    public void SecondaryAttack()
    {
        Damage = _globalData.OrbDamage * 20f;
        int count = Mathf.FloorToInt(Mathf.Pow(_globalData.OrbDistance * 2f, 2f));
        List<IEnemy> enemiesHit = new List<IEnemy>();
        var hit = Physics2D.OverlapCircle(transform.position, _globalData.OrbSize.x * _globalData.OrbDistance, _mask[0]);
        Vector3 prevPosition = transform.position;
        Debug.Log(hit);
        SetIdle();
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
            hit.GetComponent<IEnemy>().TakeDamage(Damage);
            prevPosition = hit.transform.position;
            hit = Physics2D.OverlapCircle(hit.transform.position, _globalData.OrbSize.x * _globalData.OrbDistance, _mask[0]);
            Debug.Log(hit);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var orbHit = Physics2D.OverlapCircleAll(transform.position, _globalData.OrbSize.x, _mask[0]);
            foreach (Collider2D hit in orbHit)
            {
                hit.GetComponent<IEnemy>().TakeDamage(Damage / orbHit.Length);
            }
        }
    }
    #endregion

    #region Additional Functions
    public void KillOrbling(Vector3 pos)
    {
        transform.position = pos;
        transform.localScale = _globalData.OrbSize;
        SetIdle();
    }
    #endregion
}
