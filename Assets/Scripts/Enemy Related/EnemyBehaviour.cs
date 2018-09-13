using System.Collections.Generic;
using UnityEngine;
using Elementalist.StatusEffect;

namespace Elementalist.Enemy
{
    public abstract class EnemyBehaviour : MonoBehaviour
    {
        public abstract void Setup(EnemySetup enemySetup);
        protected abstract void Die();

        public Dictionary<BuffType, Buff> Buffs { get; set; }
        public Dictionary<DebuffType, Debuff> Debuffs { get; set; }

        public abstract EnemyScriptable EnemyInfo { get; }
        protected abstract SpriteRenderer TierRenderer { get; }
        protected abstract float MaxHealth { get; set; }
        protected abstract float CurrentHealth { get; set; }
        protected abstract int Tier { get; set; }
        
        protected Transform _player;
        protected float _speed;
        protected Rigidbody2D _rigidBody;
        protected GlobalDataHandler _globalData;

        private Color _invincibleColor;

        protected void Startup()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _invincibleColor = new Color(EnemyInfo.BaseColor.r, EnemyInfo.BaseColor.g, EnemyInfo.BaseColor.b, 0.25f);
            Buffs = new Dictionary<BuffType, Buff>();
            Debuffs = new Dictionary<DebuffType, Debuff>();
            CreateInvincibility();
            CreateKnockback();
            CreateSlow();
            CreateStun();
        }

        private void Update()
        {
            //Apply Buffs and Debuffs In Order
            Buffs[BuffType.Invinciblity].EffectMethod();
            if (Debuffs[DebuffType.Stun].EffectMethod() == 1f)
                return;
            Debuffs[DebuffType.Knockback].EffectMethod();

            MoveToPlayer();
        }

        public void KnockBack(float strength, Vector2 direction)
        {
            _rigidBody.velocity = direction * strength;
            Debuffs[DebuffType.Knockback].IsAffected = true;
        }

        private void MoveToPlayer()
        {
            var direction = (_player.position - transform.position).normalized;
            var rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, rotation);
            transform.position += transform.up * Time.deltaTime * _speed * Debuffs[DebuffType.Slow].EffectMethod();
        }

        public void TakeDamage(float dmg)
        {
            if (Buffs[BuffType.Invinciblity].IsAffected) return;
            CurrentHealth -= dmg;
            Die();
        }

        public void SetTierIcon()
        {
            TierRenderer.color = EnemyInfo.TierColors.Evaluate((Tier % 7) / 7f);
            TierRenderer.sprite = EnemyInfo.Tiers[Mathf.FloorToInt(Tier / 7f)];
        }
        
        //Buffs
        private void CreateInvincibility()
        {
            var buff = new Buff(true, Time.time + 0.5f);
            buff.EffectMethod = () =>
            {
                buff.IsAffected = buff.Duration >= Time.time;

                GetComponent<SpriteRenderer>().color = buff.IsAffected ? _invincibleColor : Color.Lerp(Color.black, EnemyInfo.BaseColor, CurrentHealth / MaxHealth);

                return 0f;
            };

            Buffs.Add(BuffType.Invinciblity, buff);
        }

        //Debuffs
        private void CreateKnockback()
        {
            var debuff = new Debuff();
            debuff.EffectMethod = () =>
            {
                _rigidBody.velocity = _rigidBody.velocity.magnitude >= 0.1f ? _rigidBody.velocity * 0.9f : Vector2.zero;
                debuff.IsAffected = _rigidBody.velocity.magnitude == 0;

                return 0f;
            };

            Debuffs.Add(DebuffType.Knockback, debuff);
        }
        private void CreateSlow()
        {
            var debuff = new Debuff();
            debuff.EffectMethod = () =>
            {
                debuff.IsAffected = debuff.Duration >= Time.time;

                if (debuff.IsAffected)
                    return debuff.Strength;
                else
                    return 1f;
            };

            Debuffs.Add(DebuffType.Slow, debuff);
        }
        private void CreateStun()
        {
            var debuff = new Debuff();
            debuff.EffectMethod = () =>
            {
                debuff.IsAffected = debuff.Duration >= Time.time;

                if (debuff.IsAffected)
                    return 1f;
                else
                    return 0f;
            };
            Debuffs.Add(DebuffType.Stun, debuff);
        }


        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag("Player")) 
            {
                KnockBack(20f, -transform.up);
                if (Buffs[BuffType.Invinciblity].IsAffected) return;
                collision.transform.GetComponent<Player.PlayerBehaviour>().TakeDamage(1);
            }
        }
    }
}