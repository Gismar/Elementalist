using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour {

    public GameObject[] Orbs;

    [SerializeField] private Tilemap _map;
    private float _speed;
    private float _timer;
    private int _maxHealth;
    private GlobalDataHandler _globalData;
    public int _currentHealth;
    private float _orbCount;
    private float _iframeTimer;
    
	void Start () {
        _globalData = GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalDataHandler>();
        _maxHealth = _globalData.PlayerMaxHealth;
        _currentHealth = _maxHealth;
        CreateNewOrb();
        _speed = _globalData.PlayerSpeed;
        _timer = Time.time + _globalData.OrbDelay;
	}
	
	void Update () {
        var direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        var rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        float moveX = 0;
        float moveY = 0;

        if (Input.GetKey(_globalData.Right)) moveX = IsXInMap(1f * Time.deltaTime * _speed);
        if (Input.GetKey(_globalData.Left)) moveX = IsXInMap(-1f * Time.deltaTime * _speed);
        if (Input.GetKey(_globalData.Up)) moveY = IsYInMap(1f * Time.deltaTime * _speed);
        if (Input.GetKey(_globalData.Down)) moveY = IsYInMap(-1f * Time.deltaTime * _speed);

        transform.Translate(new Vector3(moveX,moveY), Space.World);
        if (_iframeTimer < Time.time) GetComponent<SpriteRenderer>().color = Color.black;
        if (_orbCount > 4) return;
        if(Time.time > _timer)
        {
            CreateNewOrb();
            _timer = Time.time + (_globalData.OrbDelay * _orbCount);
            _currentHealth++;
        }
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
        _orbCount++;
        var temp = Instantiate(Orbs[0]);
        var randomOffset = _orbCount/2.5f * Mathf.PI;
        temp.transform.position = transform.position;
        temp.GetComponent<IOrb>().Setup(new Vector2(randomOffset, randomOffset), transform, _globalData, true, new float[4], new float[4], 0);
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
