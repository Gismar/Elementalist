using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orb
{
    public class OrbSetup
    {
        public float SwapTimer { get; }
        public float Decay { get; }
        public Transform Player { get; }
        public GlobalDataHandler GlobalData { get; }
        public State OrbState { get; }
        public float[] MainAttackTimers { get; }
        public float[] SecondaryAttackTimers { get; }
        public OrbElement OrbType { get; }

        public OrbSetup(float swapTimer, float decay, Transform player, GlobalDataHandler globalData, State orbState, float[] mainTimers, float[] secondaryTimers, OrbElement orbType)
        {
            SwapTimer = swapTimer;
            Decay = decay;
            Player = player;
            GlobalData = globalData;
            OrbState = OrbState;
            MainAttackTimers = mainTimers;
            SecondaryAttackTimers = secondaryTimers;
            OrbType = orbType;
        }
    }
}

namespace Enemy
{
    public class EnemySetup
    {
        public float Speed { get; }
        public Transform Target { get; }
        public float Health { get; }
        public int Tier { get; }
        public GlobalDataHandler GlobalData { get; }

        public EnemySetup(float speed, Transform target, float health, int tier, GlobalDataHandler globalData)
        {
            Speed = speed;
            Target = target;
            Health = health;
            Tier = tier;
            GlobalData = globalData;
        }
    }
}