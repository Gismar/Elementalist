using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldInformation", menuName = "WorldInformation")]
public class WorldInformation : ScriptableObject {
    public float OrbDelay = 30f;
    public Vector2 OrbSize = new Vector2(1,1);
    public float OrbDamage = 1f;
    public float OrbDistance = 1f;

    public int PlayerMaxHealth = 10;
    public float PlayerSpeed = 5f;
    
    public KeyCode Up = KeyCode.W;
    public KeyCode Down = KeyCode.S;
    public KeyCode Left = KeyCode.A;
    public KeyCode Right = KeyCode.D;
    public KeyCode Recall = KeyCode.Space;

    public int Points = 0;
    public int HighestTime;

    public enum Stats
    {
        OrbDistance,
        OrbDamage,
        OrbDelay,
        OrbSize,
        PlayerMaxHealth,
        PlayerSpeed
    }

    public Dictionary<Stats, IUpgradeable> StatDictionary =
        new Dictionary<Stats, IUpgradeable>
        {
            {Stats.OrbDamage, new OrbDamage() },
            {Stats.OrbSize, new OrbSize() },
            {Stats.OrbDelay, new OrbDelay() },
            {Stats.OrbDistance, new OrbDistance() },
            {Stats.PlayerMaxHealth, new PlayerMaxHealth() },
            {Stats.PlayerSpeed, new PlayerSpeed() }
        };

    public void UsePoints(int amount)
    {
        Points -= amount;
    }
}
