using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpecialAbility
{
    public Type type;
    public enum Type
    {
        none, Dash, Bounce
    }
}
