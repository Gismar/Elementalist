﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private float _Speed;
    [SerializeField] public GameObject[] Orbs;
    [SerializeField] private float _Timer;
    [SerializeField] private int _MaxHealth;
    [SerializeField] private Tilemap _Map;
    private GlobalDataHandler _GlobalData;
    public int _CurrentHealth;
    private float _OrbCount;
    private float _IframeTimer;
	// Use this for initialization
	void Start () {
        _GlobalData = GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalDataHandler>();
        _MaxHealth = _GlobalData.PlayerMaxHealth;
        _CurrentHealth = _MaxHealth;
        CreateNewOrb();
        _Speed = _GlobalData.PlayerSpeed;
        _Timer = Time.time + _GlobalData.OrbDelay;
	}
	
	void Update () {
        var direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        var rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        float moveX = 0;
        float moveY = 0;

        if (Input.GetKey(_GlobalData.Right)) moveX = IsXInMap(1f * Time.deltaTime * _Speed);
        if (Input.GetKey(_GlobalData.Left)) moveX = IsXInMap(-1f * Time.deltaTime * _Speed);
        if (Input.GetKey(_GlobalData.Up)) moveY = IsYInMap(1f * Time.deltaTime * _Speed);
        if (Input.GetKey(_GlobalData.Down)) moveY = IsYInMap(-1f * Time.deltaTime * _Speed);

        transform.Translate(new Vector3(moveX,moveY), Space.World);
        if (_IframeTimer < Time.time) GetComponent<SpriteRenderer>().color = Color.black;
        if (_OrbCount > 4) return;
        if(Time.time > _Timer)
        {
            CreateNewOrb();
            _Timer = Time.time + _GlobalData.OrbDelay;
            _CurrentHealth++;
        }
    }

    private float IsXInMap(float x)
    {
        return _Map.localBounds.Contains(new Vector3(transform.position.x + x, 0)) ? x : 0;
    }

    private float IsYInMap(float y)
    {
        return _Map.localBounds.Contains(new Vector3(0, transform.position.y + y)) ? y : 0;
    }

    void CreateNewOrb()
    {
        _OrbCount++;
        var temp = Instantiate(Orbs[0]);
        var randomOffset = _OrbCount/2.5f * Mathf.PI;
        temp.transform.position = transform.position;
        temp.GetComponent<IOrb>().Setup(new Vector2(randomOffset, randomOffset), transform, _GlobalData, true, new float[4], new float[4], 0);
    }

    public void TakeDamage(int dmg)
    {
        if (_IframeTimer > Time.time) return;
        _CurrentHealth -= dmg;
        _CurrentHealth = _CurrentHealth <= 0 ? 0 : _CurrentHealth;
        var color = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0.5f);
        _IframeTimer = Time.time + 1f;
    }
}
