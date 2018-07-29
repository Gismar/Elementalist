﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgradeable {
    bool CanBuy();
    bool IsMaxLevel();
    void SetWorldInfo(WorldInformation worldInfo);
    int Upgrade();

    int Cost { get; }
    string Format { get; }
    int CurrentLevel { get; }
    int MaxLevel { get; }
}

public class OrbDamage : IUpgradeable
{
    public int Cost { get; private set; } = 200;
    public string Format { get { return _WorldInfo.OrbDamage.ToString("0.0x"); } }
    public int CurrentLevel { get; private set; } = 0;
    public int MaxLevel { get; private set; } = 10;

    public WorldInformation _WorldInfo;
    public void SetWorldInfo(WorldInformation worldInfo)
    {
        _WorldInfo = worldInfo;
    }
    public bool CanBuy()
    {
        return (_WorldInfo.Points >= Cost);
    }
    public bool IsMaxLevel()
    {
        return (CurrentLevel == MaxLevel);
    }
    public int Upgrade()
    {
        var initalCost = Cost;
        _WorldInfo.OrbDamage += 0.5f;
        Cost *= 2;
        CurrentLevel++;
        return initalCost;
    }
}

public class OrbDelay : IUpgradeable
{
    public int MaxLevel { get; private set; } = 4;
    public int CurrentLevel { get; private set; } = 0;
    public int Cost { get; private set; } = 400;
    public string Format { get { return _WorldInfo.OrbDelay.ToString("0s"); } }
    public WorldInformation _WorldInfo;

    public void SetWorldInfo(WorldInformation worldInfo)
    {
        _WorldInfo = worldInfo;
    }
    public bool CanBuy()
    {
        return (_WorldInfo.Points >= Cost);
    }
    public bool IsMaxLevel()
    {
        return (CurrentLevel == MaxLevel);
    }
    public int Upgrade()
    {
        var initalCost = Cost;
        _WorldInfo.OrbDelay -= 5f;
        Cost *= 2;
        CurrentLevel++;
        return initalCost;
    }
}

public class OrbSize : IUpgradeable
{
    public int MaxLevel { get; private set; } = 4;
    public int CurrentLevel { get; private set; } = 0;
    public int Cost { get; private set; } = 350;
    public string Format { get { return _WorldInfo.OrbSize.ToString("0.00x"); } }
    public WorldInformation _WorldInfo;

    public void SetWorldInfo(WorldInformation worldInfo)
    {
        _WorldInfo = worldInfo;
    }
    public bool CanBuy()
    {
        return (_WorldInfo.Points >= Cost);
    }
    public bool IsMaxLevel()
    {
        return (CurrentLevel == MaxLevel);
    }
    public int Upgrade()
    {
        var initalCost = Cost;
        _WorldInfo.OrbSize += new Vector2(0.25f, 0.25f);
        Cost *= 2;
        CurrentLevel++;
        return initalCost;
    }
}

public class OrbDistance : IUpgradeable
{
    public int MaxLevel { get; private set; } = 4;
    public int CurrentLevel { get; private set; } = 0;
    public int Cost { get; private set; } = 400;
    public string Format { get { return _WorldInfo.OrbDistance.ToString("0.0"); } }
    public WorldInformation _WorldInfo;

    public void SetWorldInfo(WorldInformation worldInfo)
    {
        _WorldInfo = worldInfo;
    }
    public bool CanBuy()
    {
        return (_WorldInfo.Points >= Cost);
    }
    public bool IsMaxLevel()
    {
        return (CurrentLevel == MaxLevel);
    }
    public int Upgrade()
    {
        var initalCost = Cost;
        _WorldInfo.OrbDistance += 0.25f;
        Cost *= 2;
        CurrentLevel++;
        return initalCost;
    }
}

public class PlayerSpeed : IUpgradeable
{
    public int MaxLevel { get; private set; } = 10;
    public int CurrentLevel { get; private set; } = 0;
    public int Cost { get; private set; } = 100;
    public string Format { get { return _WorldInfo.PlayerSpeed.ToString("0m/s"); } }
    public WorldInformation _WorldInfo;

    public void SetWorldInfo(WorldInformation worldInfo)
    {
        _WorldInfo = worldInfo;
    }
    public bool CanBuy()
    {
        return (_WorldInfo.Points >= Cost);
    }
    public bool IsMaxLevel()
    {
        return (CurrentLevel == MaxLevel);
    }
    public int Upgrade()
    {
        var initalCost = Cost;
        _WorldInfo.PlayerSpeed += 1;
        Cost *= 2;
        CurrentLevel++;
        return initalCost;
    }
}

public class PlayerMaxHealth : IUpgradeable
{
    public int MaxLevel { get; private set; } = 10;
    public int CurrentLevel { get; private set; } = 0;
    public int Cost { get; private set; } = 150;
    public string Format { get { return _WorldInfo.PlayerMaxHealth.ToString(""); } }
    public WorldInformation _WorldInfo;

    public void SetWorldInfo(WorldInformation worldInfo)
    {
        _WorldInfo = worldInfo;
    }
    public bool CanBuy()
    {
        return (_WorldInfo.Points >= Cost);
    }
    public bool IsMaxLevel()
    {
        return (CurrentLevel == MaxLevel);
    }
    public int Upgrade()
    {
        var initalCost = Cost;
        _WorldInfo.PlayerMaxHealth += 2;
        Cost *= 2;
        CurrentLevel++;
        return initalCost;
    }
}