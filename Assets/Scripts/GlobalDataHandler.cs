using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Orb;

public class GlobalDataHandler : MonoBehaviour {

    public float OrbDelay = 30f;
    public Vector2 OrbSize = new Vector2(1, 1);
    public float OrbDamage = 1f;
    public float OrbDistance = 1f;

    public int PlayerMaxHealth = 10;
    public float PlayerSpeed = 5f;

    public int Points = 1000000;
    public int PointsMultiplier = 1;
    public int HighestMinute;
    public int SurvivalTime;

    public enum Stat
    {
        OrbDistance,
        OrbDamage,
        OrbDelay,
        OrbSize,
        PlayerMaxHealth,
        PlayerSpeed
    }

    public enum Key
    {
        Up, Down, Left, Right,
        Recall,
        Water, Fire, Lightning, Earth, Air
    }

    public Dictionary<Key, KeyCode> Keys =
        new Dictionary<Key, KeyCode>
        {
            [Key.Up] = KeyCode.W,
            [Key.Down] = KeyCode.S,
            [Key.Left] = KeyCode.A,
            [Key.Right] = KeyCode.D,
            [Key.Recall] = KeyCode.Space,
            [Key.Water] = KeyCode.Q,
            [Key.Fire] = KeyCode.E,
            [Key.Lightning] = KeyCode.R,
            [Key.Earth] = KeyCode.F,
            [Key.Air] = KeyCode.V
        };

    public Dictionary<Stat, IUpgradeable> StatDictionary =
        new Dictionary<Stat, IUpgradeable>
        {
            {Stat.OrbDamage, new OrbDamage() },
            {Stat.OrbSize, new OrbSize() },
            {Stat.OrbDelay, new OrbDelay() },
            {Stat.OrbDistance, new OrbDistance() },
            {Stat.PlayerMaxHealth, new PlayerMaxHealth() },
            {Stat.PlayerSpeed, new PlayerSpeed() }
        };

    public void UsePoints(int amount)
    {
        Points -= amount;
    }

    public void AddPoints(int amount)
    {
        Points += amount * PointsMultiplier;
    }

    private void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        SceneManager.LoadScene("Main Menu");
    }

    public void CheckScore()
    {
        if (SurvivalTime > HighestMinute) HighestMinute = Mathf.FloorToInt(Time.timeSinceLevelLoad);
    }
}
