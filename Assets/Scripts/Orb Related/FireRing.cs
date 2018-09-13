using UnityEngine;

namespace Elementalist.Orb
{
    public class FireRing : MonoBehaviour
    {
        private float _damage;
        private float _duration;

        public void Setup(float damage, float duration)
        {
            _damage = damage;
            _duration = duration + Time.time;
        }

        void Update()
        {
            transform.Rotate(new Vector3(0, 0, 15));
            transform.localScale += new Vector3(0.1f, 0.1f, 0);
            if (_duration <= Time.time) Destroy(transform.gameObject);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<Enemy.EnemyBehaviour>().TakeDamage(_damage * Time.deltaTime);
            }
        }
    }
}
