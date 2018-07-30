using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    private int _Damage;
    private float _Duration;

    public void Setup(int damage, float duration)
    {
        _Damage = damage;
        _Duration = duration + Time.time;
    }

	void Update () {
        transform.position += transform.up * Time.deltaTime * 10f;
        if (_Duration <= Time.time) Destroy(transform.gameObject);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyBehaviour>().TakeDamage(_Damage);
        }
    }
}
