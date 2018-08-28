using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSplit : MonoBehaviour {

    private float _damage;

    public void Setup(float damage, float duration)
    {
        _damage = damage;
        Destroy(transform.gameObject, duration);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<IEnemy>();
            enemy.TakeDamage(_damage);
            enemy.IsStunned = true;
            enemy.StunDuration = Time.time + 1f;
        }
    }
}
