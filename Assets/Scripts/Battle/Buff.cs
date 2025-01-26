using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    HealthRegen,
    MovingSpeed,
    ChargingSpeed,
    NewBubble,
    ChargingBar,
    SlowEnemy,
    EnemySize,
    ChargingRegen
}

[CreateAssetMenu(fileName = "Buff", menuName = "ScriptableObject/Buff")]
public class Buff : ScriptableObject
{
    public string descrition;
    public BuffType type;
    public float value;
    public float weight;
}
