using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "new Enemy", menuName = "Enemy Scriptable")]
    public class EnemyScriptable : ScriptableObject
    {
        public Color BaseColor;
        public Gradient TierColors;
        public Sprite[] Tiers;
        public float SpeedMultiplier;
        public float HealthMultiplier;
        public float StartingSpawnTime;
        public int SpawnAmount;
        public int PointValue;
    }
}