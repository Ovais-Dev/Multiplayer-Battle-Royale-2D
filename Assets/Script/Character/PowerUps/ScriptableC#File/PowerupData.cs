using UnityEngine;
using System.Collections.Generic;

public abstract class PowerupData : ScriptableObject
{
    public int id;
    public float duration;
    public abstract IPowerup CreateInstance();
}


