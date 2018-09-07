using UnityEngine;

namespace Orb
{
    public class Earthquake : MonoBehaviour
    {
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
                var enemy = collision.GetComponent<Enemy.EnemyBehaviour>();
                enemy.Debuffs[DebuffType.Slow].IsAffected = true;
                enemy.Debuffs[DebuffType.Slow].Strength = _strength;
                enemy.Debuffs[DebuffType.Slow].Duration = Time.time + 1f;
            }
        }
    }
}