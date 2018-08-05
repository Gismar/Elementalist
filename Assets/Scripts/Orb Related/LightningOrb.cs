using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningOrb : OrbBehaviour, IOrb {
    [SerializeField] private GameObject _beamPrefab;
    [SerializeField] private LayerMask[] _Mask;
    private LineRenderer _aimLine;

    public float Damage { get; private set; }
    public float MainAttackDelay { get; private set; } = 0.5f;
    public float SecondaryAttackDelay { get; private set; } = 3f;

    private void Start()
    {
        _Orb = this;
        transform.localScale = _GlobalData.OrbSize;
        _aimLine = GetComponent<LineRenderer>();
    }

    public void Setup(Vector2 offset, Transform player, GlobalDataHandler globalData, bool isIdle, float[] mainTimers, float[] secondTimers, int orbType)
    {
        _Player = player;
        _Offset = offset;
        _GlobalData = globalData;
        _IsIdle = isIdle;
        _MainAttackTimers = mainTimers;
        _SecondaryAttackTimers = secondTimers;
        _OrbType = orbType;
        Startup();
    }

    public void SetIdle()
    {
        _BeganAim = false;
        _IsAttacking = false;
        _aimLine.enabled = false;
    }

    public void ActivateAimLine()
    {
        _aimLine.enabled = true;
        _BeganAim = true;
    }

    public void UpdateAimLine()
    {
        if (!_BeganAim) ActivateAimLine();
        transform.rotation = Quaternion.Euler(0, 0, MouseAngle());
    }

    public void MainAttack()
    {
        _aimLine.enabled = false;
        var mouseAngle = MouseAngle();
        var temp = Instantiate(_beamPrefab);
        temp.GetComponent<LightningOrbBeam>().Setup(_GlobalData.OrbDistance * 6f, 5f * _GlobalData.OrbDamage, transform);
        temp.transform.rotation = Quaternion.Euler(0, 0, mouseAngle);
        temp.transform.position = transform.position;
        transform.localScale = Vector2.zero;
    }

    public void SecondaryAttack()
    {
        Damage = _GlobalData.OrbDamage * 20f;
        int count = Mathf.FloorToInt(_GlobalData.OrbDistance * 2f);
        List<IEnemy> enemiesHit = new List<IEnemy>();
        var hit = Physics2D.OverlapCircle(transform.position, _GlobalData.OrbSize.x * 2f, _Mask[0]);
        Debug.Log(hit);
        SetIdle();
        for (int i = 0; i < count; i++)
        {
            if (hit == null) return;
            hit.GetComponent<IEnemy>().TakeDamage(Damage);
            hit = Physics2D.OverlapCircle(hit.transform.position, _GlobalData.OrbSize.x * 2f, _Mask[0]);
            Debug.Log(hit);
        }
    }

    public void KillOrbling(Vector3 pos)
    {
        transform.position = pos;
        transform.localScale = _GlobalData.OrbSize;
        SetIdle();

        var orbHit = Physics2D.OverlapCircleAll(transform.position, _GlobalData.OrbSize.x / 2f, _Mask[1]);
        Debug.Log(orbHit.Length);
        if (orbHit.Length < 2) return;
        Vector3 averagePos = Vector3.zero;
        foreach (Collider2D hit in orbHit)
        {
            averagePos += hit.transform.position;
        }
        averagePos = Vector3.Distance(averagePos, transform.position) < 0.5f ? new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)) : averagePos / orbHit.Length;
        transform.position += (transform.position - averagePos).normalized;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var orbHit = Physics2D.OverlapCircleAll(transform.position, _GlobalData.OrbSize.x, _Mask[0]);
            foreach (Collider2D hit in orbHit)
            {
                hit.GetComponent<IEnemy>().TakeDamage(Damage / orbHit.Length);
            }
        }
    }
}
