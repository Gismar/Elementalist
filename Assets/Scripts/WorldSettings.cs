using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldSettings {
    public static float OrbDelay = 30f;
    public static Vector2 OrbSize = new Vector2(1,1);
    public static float OrbDamage = 1f;
    public static float ChargeRate = 1f;
    public static int PlayerMaxHealth = 10;
    public static float PlayerSpeed = 5f;

    public static KeyCode Up = KeyCode.W;
    public static KeyCode Down = KeyCode.S;
    public static KeyCode Left = KeyCode.A;
    public static KeyCode Right = KeyCode.D;

    public static int Points = 0;

    public enum Stats
    {
        ChargeRate,
        OrbDamage,
        OrbDelay,
        OrbSize,
        PlayerMaxHealth,
        PlayerSpeed
    }

    public static Dictionary<Stats, IUpgradeClasses> StatDictionary =
        new Dictionary<Stats, IUpgradeClasses>
        {
            {Stats.OrbDamage, new OrbDamage() },
            {Stats.OrbSize, new OrbSize() },
            {Stats.OrbDelay, new OrbDelay() },
            {Stats.ChargeRate, new ChargeRate() },
            {Stats.PlayerMaxHealth, new PlayerMaxHealth() },
            {Stats.PlayerSpeed, new PlayerSpeed() }
        };

    public static void UsePoints(int amount)
    {
        Points -= amount;
    }
}
