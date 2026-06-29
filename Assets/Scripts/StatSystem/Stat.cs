using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat 
{
    [SerializeField] private float baseValue;

    public float GetValue()
    {
        return baseValue;
    }
}
