﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningOrbBeam : MonoBehaviour {

    private float _y;
    private float _speed = 10f;
    [SerializeField] private Transform _leftOrb;
    [SerializeField] private Transform _rightOrb;
    [SerializeField] private Transform _orbPos;
    private ParticleSystem.ShapeModule _particleShapeModule;
    private SpriteRenderer _spriteRenderer;
    private float _distance;
    private float _damage;

    public void Setup(float distance, float damage, Transform orbPos)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _particleShapeModule = GetComponent<ParticleSystem>().shape;
        _distance = distance;
        _damage = damage;
        _orbPos = orbPos;
    }

	void Update () {
        _y += Time.deltaTime * _speed;
        transform.position = transform.up * _y + _orbPos.position;
        _rightOrb.position = (transform.right * _y /2f) + transform.up * _y + _orbPos.position;
        _leftOrb.position = (-transform.right * _y /2f) + transform.up * _y + _orbPos.position;
        _spriteRenderer.size = new Vector2(_y, 0.5f);
        _particleShapeModule.radius = _y / 2f;

        if (_y >= _distance)
        {
            _orbPos.transform.GetComponent<LightningOrb>().KillOrbling(transform.position);
            Destroy(transform.gameObject);
        }
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<IEnemy>().TakeDamage(_damage);
        }
    }
}