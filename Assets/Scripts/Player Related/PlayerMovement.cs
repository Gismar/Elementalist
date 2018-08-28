using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour {

    public GameObject[] Orbs;

    [SerializeField] private Tilemap _map;
    private float _speed;
    private int _maxHealth;
    private GlobalDataHandler _globalData;
    public int _currentHealth;
    private float _iframeTimer;
    
	void Start () {
        _globalData = GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalDataHandler>();
        _maxHealth = _globalData.PlayerMaxHealth;
        _currentHealth = _maxHealth;
        CreateNewOrb();
        _speed = _globalData.PlayerSpeed;
	}
	
	void Update () {
        var direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        var rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation);

        float moveX = 0;
        float moveY = 0;

        if (Input.GetKey(_globalData.Keys[GlobalDataHandler.Key.Right])) moveX = IsXInMap(1f * Time.deltaTime * _speed);
        if (Input.GetKey(_globalData.Keys[GlobalDataHandler.Key.Left])) moveX = IsXInMap(-1f * Time.deltaTime * _speed);
        if (Input.GetKey(_globalData.Keys[GlobalDataHandler.Key.Up])) moveY = IsYInMap(1f * Time.deltaTime * _speed);
        if (Input.GetKey(_globalData.Keys[GlobalDataHandler.Key.Down])) moveY = IsYInMap(-1f * Time.deltaTime * _speed);

        transform.Translate(new Vector3(moveX,moveY), Space.World);

        if (_iframeTimer < Time.time) GetComponent<SpriteRenderer>().color = Color.black;
    }

    private float IsXInMap(float x)
    {
        return _map.localBounds.Contains(new Vector3(transform.position.x + x, 0)) ? x : 0;
    }

    private float IsYInMap(float y)
    {
        return _map.localBounds.Contains(new Vector3(0, transform.position.y + y)) ? y : 0;
    }

    void CreateNewOrb()
    {
        var temp = Instantiate(Orbs[0]);
        temp.transform.position = transform.position;

        var orbSetup = new OrbSetup
        (
            0,
            transform,
            _globalData,
            State.Orbiting,
            new float[5],
            new float[5],
            OrbElement.Water
        );

        temp.GetComponent<OrbBehaviour>().SetupPublic(orbSetup);
    }

    public void TakeDamage(int dmg)
    {
        if (_iframeTimer > Time.time) return;
        _currentHealth -= dmg;
        _currentHealth = _currentHealth <= 0 ? 0 : _currentHealth;
        var color = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0.5f);
        _iframeTimer = Time.time + 1f;
    }
}
