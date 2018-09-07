using UnityEngine;

namespace Orb
{
    public class EarthSplit : MonoBehaviour
    {

        private float _damage;

        public void Setup(float damage, float duration)
        {
            _damage = damage;
            Destroy(transform.gameObject, duration);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                var enemy = collision.GetComponent<Enemy.EnemyBehaviour>();
                enemy.TakeDamage(_damage);
                enemy.Debuffs[DebuffType.Stun].IsAffected = true;
                enemy.Debuffs[DebuffType.Slow].Duration = Time.time + 1f;
            }
        }
    }
}