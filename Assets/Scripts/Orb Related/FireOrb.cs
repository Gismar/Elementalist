using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOrb : OrbBehaviour, IOrb {
    [SerializeField] private GameObject _LineHolder;
    [SerializeField] private GameObject _Fireball;
    [SerializeField] private GameObject _RingOfFire;

    public float Damage { get; private set; }
    public float IdleDelay { get; private set; } = 3f;
    public float MainAttackDelay { get; private set; } = 1f;
    public float SecondaryAttackDelay { get; private set; } = 5f;


    void Start()
    {
        transform.localScale = _GlobalData.OrbSize;
        _Orb = this;
        _LineHolder = transform.GetChild(0).gameObject;
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
        _LineHolder.SetActive(false);
    }

    public void MainAttack()
    {
        _IsAttacking = false;
        _LineHolder.SetActive(false);
        Damage = 20 * _GlobalData.OrbDamage;
        CreateFireball(MouseAngle());
        CreateFireball(MouseAngle() - 180f);
    }

    public void SecondaryAttack()
    {
        _IsAttacking = false;
        Damage = 25 * _GlobalData.OrbDamage;
        CreateRingOfFire();
    }

    public void ActivateAimLine()
    {
        _LineHolder.SetActive(true);
        _BeganAim = true;
    }

    public void UpdateAimLine()
    {
        if (!_BeganAim) ActivateAimLine();
        _LineHolder.transform.rotation = Quaternion.Euler(0, 0, MouseAngle());
    }

    public void Swap(int orbType)
    {
        switch (orbType)
        {
            case 0:
                var temp = Instantiate(_Player.GetComponent<PlayerMovement>().Orbs[orbType]);
                temp.transform.position = transform.position;
                temp.GetComponent<IOrb>().Setup(_Offset, _Player, _GlobalData, _IsIdle, _MainAttackTimers, _SecondaryAttackTimers, orbType);
                Destroy(transform.gameObject);
                break;
        }
    }

    private void CreateFireball(float rotation)
    {
        var temp = Instantiate(_Fireball);
        temp.transform.position = transform.position;
        temp.transform.localScale = _GlobalData.OrbSize;
        temp.transform.rotation = Quaternion.Euler(0, 0, rotation);
        temp.GetComponent<Fireball>().Setup(Damage, _GlobalData.OrbDistance);
    }

    private void CreateRingOfFire()
    {
        var temp = Instantiate(_RingOfFire);
        temp.transform.position = transform.position;
        temp.transform.localScale = _GlobalData.OrbSize;
        temp.GetComponent<FireRing>().Setup(Damage, _GlobalData.OrbDistance);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<IEnemy>().TakeDamage(5 * Time.deltaTime);
        }
    }
}
