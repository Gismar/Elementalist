using UnityEngine;
using UnityEngine.Tilemaps;
using Orb;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;

namespace Player
{
    public class PlayerMovement : PlayerNetworkingBehavior
    {
        private Tilemap _map;
        private float _speed;
        private int _maxHealth;
        private GlobalDataHandler _globalData;
        public int _currentHealth;
        private float _iframeTimer;

        void Start()
        {
            _globalData = GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalDataHandler>();
            _map = GameObject.FindGameObjectWithTag("Map").GetComponent<Tilemap>();
            GameObject.FindGameObjectWithTag("Level UI").GetComponent<UI.LevelUI>().Player = this;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBehaviour>().Player = transform;
            _maxHealth = _globalData.PlayerMaxHealth;
            _currentHealth = _maxHealth;
            CreateNewOrb();
            _speed = _globalData.PlayerSpeed;
        }

        void Update()
        {
            //if (!networkObject.IsServer)
            //{
            //    transform.position = networkObject.Position;
            //    transform.rotation = networkObject.Rotation;
            //    GetComponent<SpriteRenderer>().color = networkObject.Color;
            //    return;
            //}

            var direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            var rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotation);

            float moveX = 0;
            float moveY = 0;
            
            if (Input.GetKey(_globalData.Keys[GlobalDataHandler.Key.Right]))
                moveX = GetDeltaX(1);
            if (Input.GetKey(_globalData.Keys[GlobalDataHandler.Key.Left]))
                moveX = GetDeltaX(-1);

            if (Input.GetKey(_globalData.Keys[GlobalDataHandler.Key.Up]))
                moveY = GetDeltaY(1);
            if (Input.GetKey(_globalData.Keys[GlobalDataHandler.Key.Down]))
                moveY = GetDeltaY(-1);

            transform.Translate(new Vector3(moveX, moveY), Space.World);

            networkObject.Position = transform.position;
            networkObject.Rotation = transform.rotation;
            networkObject.Color = GetComponent<SpriteRenderer>().color;

            if (_iframeTimer < Time.time) GetComponent<SpriteRenderer>().color = Color.black;
        }

        //Movement Calculations
        private float GetDeltaX(int direction)
        {
            return IsXInMap(direction * Time.deltaTime * _speed);
        }
        private float GetDeltaY(int direction)
        {
            return IsYInMap(direction * Time.deltaTime * _speed);
        }

        //Checks if it is in the map
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
            var orb = NetworkManager.Instance.InstantiateOrbNetworking(0, transform.position, Quaternion.Euler(Vector2.up));

            var orbSetup = new OrbSetup
            (
                Time.time + 0.5f,
                0,
                transform,
                _globalData,
                State.Orbiting,
                new float[5],
                new float[5],
                OrbElement.Water
            );

            orb.GetComponent<OrbBehaviour>().SetupPublic(orbSetup);
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
}