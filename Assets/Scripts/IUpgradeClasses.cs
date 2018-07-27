using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgradeClasses {
    bool CanBuy();
    bool IsMaxLevel();
    int Upgrade();
    int Cost { get; }
    string Value { get; }
}

public class OrbDamage : IUpgradeClasses
{
    private readonly int _MaxLevel = 10;
    private int _CurrentLevel = 0;

    public int Cost { get; private set; } = 200;
    public string Value { get { return WorldSettings.OrbDamage.ToString("0.0x"); } }

    public bool IsMaxLevel()
    {
        return (_CurrentLevel == _MaxLevel);
    }

    public bool CanBuy()
    {
        return (WorldSettings.Points >= Cost);
    }

    public int Upgrade()
    {
        var initalCost = Cost;
        WorldSettings.OrbDamage += 0.5f;
        Cost *= 2;
        _CurrentLevel++;
        return initalCost;
    }
}

public class OrbDelay : IUpgradeClasses
{
    private readonly int _MaxLevel = 4;
    private int _CurrentLevel = 0;

    public int Cost { get; private set; } = 400;
    public string Value { get { return WorldSettings.OrbDelay.ToString("0s"); } }

    public bool IsMaxLevel()
    {
        return (_CurrentLevel == _MaxLevel);
    }

    public bool CanBuy()
    {
        return (WorldSettings.Points >= Cost);
    }

    public int Upgrade()
    {
        var initalCost = Cost;
        WorldSettings.OrbDelay -= 5f;
        Cost *= 2;
        _CurrentLevel++;
        return initalCost;
    }
}

public class OrbSize : IUpgradeClasses
{
    private readonly int _MaxLevel = 4;
    private int _CurrentLevel = 0;

    public int Cost { get; private set; } = 350;
    public string Value { get { return WorldSettings.OrbSize.x.ToString("0.00x"); } }

    public bool IsMaxLevel()
    {
        return (_CurrentLevel == _MaxLevel);
    }

    public bool CanBuy()
    {
        return (WorldSettings.Points >= Cost);
    }

    public int Upgrade()
    {
        var initalCost = Cost;
        WorldSettings.OrbSize += new Vector2(0.25f, 0.25f);
        Cost *= 2;
        _CurrentLevel++;
        return initalCost;
    }
}

public class ChargeRate : IUpgradeClasses
{
    private readonly int _MaxLevel = 5;
    private int _CurrentLevel = 0;

    public int Cost { get; private set; } = 400;
    public string Value { get { return WorldSettings.ChargeRate.ToString("0.0x"); } }

    public bool IsMaxLevel()
    {
        return (_CurrentLevel == _MaxLevel);
    }

    public bool CanBuy()
    {
        return (WorldSettings.Points >= Cost);
    }

    public int Upgrade()
    {
        var initalCost = Cost;
        WorldSettings.ChargeRate += 0.5f;
        Cost *= 2;
        _CurrentLevel++;
        return initalCost;
    }
}

public class PlayerSpeed : IUpgradeClasses
{
    private readonly int _MaxLevel = 10;
    private int _CurrentLevel = 0;

    public int Cost { get; private set; } = 100;
    public string Value { get { return WorldSettings.PlayerSpeed.ToString("0m/s"); } }

    public bool IsMaxLevel()
    {
        return (_CurrentLevel == _MaxLevel);
    }

    public bool CanBuy()
    {
        return (WorldSettings.Points >= Cost);
    }

    public int Upgrade()
    {
        var initalCost = Cost;
        WorldSettings.PlayerSpeed += 1;
        Cost *= 2;
        _CurrentLevel++;
        return initalCost;
    }
}

public class PlayerMaxHealth : IUpgradeClasses
{
    private readonly int _MaxLevel = 10;
    private int _CurrentLevel = 0;

    public int Cost { get; private set; } = 150;
    public string Value { get { return WorldSettings.PlayerMaxHealth.ToString(); } }

    public bool IsMaxLevel()
    {
        return (_CurrentLevel == _MaxLevel);
    }

    public bool CanBuy()
    {
        return (WorldSettings.Points >= Cost);
    }

    public int Upgrade()
    {
        var initalCost = Cost;
        WorldSettings.PlayerMaxHealth += 2;
        Cost *= 2;
        _CurrentLevel++;
        return initalCost;
    }
}