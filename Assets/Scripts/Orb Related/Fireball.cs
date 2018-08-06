using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    private float _damage;
    private float _duration;

    public void Setup(float damage, float duration)
    {
        _damage = damage;
        _duration = duration + Time.time;
    }

	void Update () {
        transform.position += transform.up * Time.deltaTime * 10f;
        if (_duration <= Time.time) Destroy(transform.gameObject);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<IEnemy>().TakeDamage(_damage);
        }
    }
}
