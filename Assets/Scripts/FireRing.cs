using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRing : MonoBehaviour {

    private int _Damage;
    private float _Duration;

    public void Setup(int damage, float duration)
    {
        _Damage = damage;
        _Duration = duration + Time.time;
    }

    void Update () {
        transform.Rotate(new Vector3(0,0,15));
        transform.localScale += new Vector3(0.1f, 0.1f, 0);
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
