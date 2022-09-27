using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpecialAbility
{
    public SpecialAbilityType type;
    [FloatDurationProperty(FloatDurationUnitsMode.Flexible)]
    public float duration;
    public float cooldown;
    public float power;
    public enum SpecialAbilityType
    {
        None, Dash, Bounce, Invisibility
    }
}
