using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : MonoBehaviour {

    private float _strength;

    public void Setup(float strength, float duration)
    {
        _strength = strength;
        Destroy(transform.gameObject, duration);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<IEnemy>();
            enemy.IsSlowed = true;
            enemy.SlowStrength = _strength;
            enemy.SlowDuration = Time.time + 1f;
        }
    }
}
