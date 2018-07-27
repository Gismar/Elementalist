using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour {

    public int _CurrentHealth;
    [SerializeField] private float _Speed;
    [SerializeField] private GameObject[] _Orbs;
    [SerializeField] private float _Timer;
    [SerializeField] private int _MaxHealth;
    [SerializeField] private Tilemap _Map;
	// Use this for initialization
	void Start () {
        _MaxHealth = WorldSettings.PlayerMaxHealth;
        _CurrentHealth = _MaxHealth;
        CreateNewOrb();
        _Speed = WorldSettings.PlayerSpeed;
        _Timer = Time.timeSinceLevelLoad + WorldSettings.OrbDelay;
	}
	
	void Update () {
        var direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        var rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation);

        var translation = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime * _Speed;
        if(IsInMap(translation)) transform.Translate(translation, Space.World);

        if(Time.timeSinceLevelLoad > _Timer)
        {
            CreateNewOrb();
            _Timer = Time.timeSinceLevelLoad + WorldSettings.OrbDelay;
            _CurrentHealth++;
        }
    }

    private bool IsInMap(Vector3 position)
    {
        return _Map.localBounds.Contains(position + transform.position);
    }

    void CreateNewOrb()
    {
        var temp = Instantiate(_Orbs[Random.Range(0, _Orbs.Length)]);
        var randomOffset = Random.Range(-Mathf.PI, Mathf.PI);
        temp.GetComponent<WaterOrb>().SetUp(new Vector2(randomOffset, randomOffset), transform);
    }

    public void TakeDamage(int dmg)
    {
        _CurrentHealth -= dmg;
        _CurrentHealth = _CurrentHealth <= 0 ? 0 : _CurrentHealth;
    }
}
