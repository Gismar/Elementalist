using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOrb : OrbBehaviour, IOrb {
    [SerializeField] private GameObject _lineHolder;
    [SerializeField] private GameObject _fireball;
    [SerializeField] private GameObject _ringOfFire;

    public float Damage { get; private set; }
    public float MainAttackDelay { get; private set; } = 1f;
    public float SecondaryAttackDelay { get; private set; } = 5f;


    void Start()
    {
        transform.localScale = _globalData.OrbSize;
        _orb = this;
        _lineHolder = transform.GetChild(0).gameObject;
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
        _lineHolder.SetActive(false);
    }

    public void MainAttack()
    {
        _isAttacking = false;
        _lineHolder.SetActive(false);
        Damage = 20 * _globalData.OrbDamage;
        CreateFireball(MouseAngle());
        CreateFireball(MouseAngle() - 180f);
    }

    public void SecondaryAttack()
    {
        _isAttacking = false;
        Damage = 25 * _globalData.OrbDamage;
        CreateRingOfFire();
    }

    public void ActivateAimLine()
    {
        _lineHolder.SetActive(true);
        _beganAim = true;
    }

    public void UpdateAimLine()
    {
        if (!_beganAim) ActivateAimLine();
        _lineHolder.transform.rotation = Quaternion.Euler(0, 0, MouseAngle());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<IEnemy>().TakeDamage(5 * Time.deltaTime);
        }
    }
    #endregion

    #region Additional Features
    private void CreateFireball(float rotation)
    {
        var temp = Instantiate(_fireball);
        temp.transform.position = transform.position;
        temp.transform.localScale = _globalData.OrbSize;
        temp.transform.rotation = Quaternion.Euler(0, 0, rotation);
        temp.GetComponent<Fireball>().Setup(Damage, _globalData.OrbDistance);
    }

    private void CreateRingOfFire()
    {
        var temp = Instantiate(_ringOfFire);
        temp.transform.position = transform.position;
        temp.transform.localScale = _globalData.OrbSize;
        temp.GetComponent<FireRing>().Setup(Damage, _globalData.OrbDistance);
    }
    #endregion
}
