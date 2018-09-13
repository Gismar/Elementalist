using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;

namespace Elementalist.Player
{
    public class PlayerBehaviour : MonoBehaviourPunCallbacks
    {
        public float CurrentHealth { get; set; }

        private Tilemap _map;
        private float _speed;
        private float _maxHealth;
        private GlobalDataHandler _globalData;
        private float _iframeTimer;

        void Start()
        {
            if (!photonView.IsMine) return;

            _globalData = GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalDataHandler>();
            _map = GameObject.FindGameObjectWithTag("Map").GetComponent<Tilemap>();
            GameObject.FindGameObjectWithTag("Level UI").GetComponent<UI.LevelUI>().Player = this;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBehaviour>().Player = transform;
            _maxHealth = _globalData.PlayerMaxHealth;
            CurrentHealth = _maxHealth;
            _speed = _globalData.PlayerSpeed;
            CreateNewOrb();
        }

        void Update()
        {
            if (!photonView.IsMine) return;

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

            if (_iframeTimer < Time.time)
                GetComponent<SpriteRenderer>().color = Color.black;
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

        public void TakeDamage(int dmg)
        {
            if (!photonView.IsMine) return;
            if (_iframeTimer > Time.time) return;

            CurrentHealth -= dmg;
            CurrentHealth = CurrentHealth <= 0 ? 0 : CurrentHealth;

            var color = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0.5f);

            _iframeTimer = Time.time + 1f;
        }

        void CreateNewOrb()
        {
            var globalData = GameObject.FindGameObjectWithTag("Global").GetComponent<GlobalDataHandler>();
            var orb = Instantiate(globalData.OrbPrefabs[0]);

            var orbSetup = new Orb.OrbSetup
            (
                Time.time + 0.5f,
                0,
                transform,
                globalData,
                State.Orbiting,
                new float[5],
                new float[5],
                OrbElement.Water
            );

            orb.GetComponent<Orb.OrbBehaviour>().SetupPublic(orbSetup);
        }
    }
}