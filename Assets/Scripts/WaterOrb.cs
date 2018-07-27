using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterOrb : MonoBehaviour {

    [SerializeField] private BoxCollider2D _Collider;
    [SerializeField] private Transform _Player;
    [SerializeField] private Animator _Anim;
    [SerializeField] private bool _Moved;
    [SerializeField] private float _Speed;
    [SerializeField] private Vector2 _Offset;
    [SerializeField] private float _IdleTimer;
    [SerializeField] private float _IdleTimerLerp;
    [SerializeField] private int _Damage;
    [SerializeField] private float _Distance;

	void Start () {
        _Collider = GetComponent<BoxCollider2D>();
        _Anim = GetComponent<Animator>();
        transform.localScale = WorldSettings.OrbSize;
	}

    public void SetUp(Vector2 offset, Transform player)
    {
        _Offset = offset;
        _Player = player;
    }

    private void Update()
    {
        if (!_Moved)
        {
            transform.position = new Vector2(_Player.position.x + Mathf.Cos(Time.timeSinceLevelLoad * _Speed + _Offset.x), 
                                            _Player.position.y + Mathf.Sin(Time.timeSinceLevelLoad * _Speed + _Offset.y));
        }
        else
        {
            if (_IdleTimer < Time.timeSinceLevelLoad)
            {
                transform.position = Vector3.Lerp(transform.position, _Player.position, _IdleTimerLerp);
                _IdleTimerLerp += Time.deltaTime / Vector3.Distance(transform.position, _Player.position);
            }
            if (_IdleTimerLerp >= 1)
            {
                _Moved = false;
                _Damage = Mathf.RoundToInt(1 * WorldSettings.OrbDamage);
            }
        }

        if (_Anim.GetCurrentAnimatorStateInfo(0).IsTag("Moving")) return;
        if (Input.GetMouseButtonDown(0))
        {
            _Distance = 0.5f;
            GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, _Distance * 5 / WorldSettings.OrbSize.x, 0));
        }

        if (Input.GetMouseButton(0))
        {
            GetComponent<LineRenderer>().enabled = true;
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var direction = (mousePos - transform.position).normalized;
            var rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(0, 0, rotation);
            _Distance = Mathf.Clamp(_Distance + Time.deltaTime * WorldSettings.ChargeRate, 0, 3f);
            GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, _Distance * 5 / WorldSettings.OrbSize.x, 0));
        }

        if (Input.GetMouseButtonUp(0))
        {
            GetComponent<LineRenderer>().enabled = false;
            _Damage = 2 + Mathf.RoundToInt(_Distance * 5 * WorldSettings.OrbDamage);
            transform.position += transform.up * 1.5f;
            ResetCollider();
            _Anim.SetTrigger("Move");
            _Moved = true;
            _IdleTimer = Time.timeSinceLevelLoad + 3f;
            _IdleTimerLerp = 0;
        }
    }

    private void ResetCollider()
    {
        _Collider.size = new Vector2(0, 0);
        _Collider.offset = new Vector2(0, 0);
    }

    public void UpdateBoxCollider(string size)
    {
        var sizes = Array.ConvertAll(size.Split(','), float.Parse);
        _Collider.offset = new Vector2(0, sizes[0]);
        _Collider.size = new Vector2(sizes[1], sizes[2]);
    }

    public void Move(float gap)
    {
        transform.position += transform.up * gap * _Distance;
    }

    public void ResetDistance()
    {
        _Distance = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyBehaviour>().TakeDamage(_Damage);
        }
    }
}
