using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    [SerializeField] private float _MaxHealth;
    [SerializeField] private float _CurrentHealth;
    [SerializeField] private Color _Healthy;
    [SerializeField] private Color _Dead;
    [SerializeField] private Transform _Player;
    [SerializeField] private float _Speed;

    private void Update()
    {
        var direction = (_Player.position - transform.position).normalized;
        var rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        transform.position += transform.up * Time.deltaTime * _Speed;
    }

    public void SetUp(float speed, Transform player, int maxHealth)
    {
        _Speed = speed;
        _Player = player;
        _MaxHealth = maxHealth;
        _CurrentHealth = _MaxHealth;
    }

    public void TakeDamage(int dmg)
    {
        _CurrentHealth -= dmg;
        GetComponent<SpriteRenderer>().color = Color.Lerp(_Dead, _Healthy, _CurrentHealth / _MaxHealth);
        if (_CurrentHealth <= 0)
        {
            WorldSettings.Points += 10;
            Destroy(transform.gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.transform.name);
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<PlayerMovement>().TakeDamage(1);
            transform.position -= transform.up * _Speed;
        }
    }
}
