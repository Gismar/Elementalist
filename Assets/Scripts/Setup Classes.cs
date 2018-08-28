using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSetup
{
    public float Decay { get; }
    public Transform Player { get; }
    public GlobalDataHandler GlobalData { get; }
    public State OrbState { get; }
    public float[] MainAttackTimers { get; }
    public float[] SecondaryAttackTimers { get; }
    public OrbElement OrbType { get; }

    public OrbSetup(float decay, Transform player, GlobalDataHandler globalData, State orbState, float[] mainTimers, float[] secondaryTimers, OrbElement orbType)
    {
        Decay = decay;
        Player = player;
        GlobalData = globalData;
        OrbState = OrbState;
        MainAttackTimers = mainTimers;
        SecondaryAttackTimers = secondaryTimers;
        OrbType = orbType;
    }
}

public class EnemySetup
{

}