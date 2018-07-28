using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    [SerializeField] private float _MaxHealth;
    [SerializeField] private float _CurrentHealth;
    [SerializeField] private Color _OriginalColor;
    [SerializeField] private Transform _Player;
    [SerializeField] private float _Speed;
    [SerializeField] private Rigidbody2D _RB;

    private void Start() => _RB = GetComponent<Rigidbody2D>();

    private void Update()
    {
        var direction = (_Player.position - transform.position).normalized;
        var rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        transform.position += transform.up * Time.deltaTime * _Speed;
        _RB.velocity = _RB.velocity.magnitude >= 0.1f ? _RB.velocity * 0.9f : Vector2.zero;
    }

    public void SetUp(float speed, Transform player, int maxHealth, Color color)
    {
        _Speed = speed;
        _Player = player;
        _MaxHealth = maxHealth;
        _CurrentHealth = _MaxHealth;
        _OriginalColor = color;
        GetComponent<SpriteRenderer>().color = color;
    }

    public void TakeDamage(int dmg)
    {
        _CurrentHealth -= dmg;
        GetComponent<SpriteRenderer>().color = Color.Lerp(Color.black, _OriginalColor, _CurrentHealth / _MaxHealth);
        if (_CurrentHealth <= 0)
        {
            WorldSettings.Points += 10;
            Destroy(transform.gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<PlayerMovement>().TakeDamage(1);
            KnockBack(20f, -transform.up);
        }
    }

    public void KnockBack(float strengh, Vector3 direction)
    {
        _RB.velocity = direction * strengh;
    }
}
