using System;
using UnityEngine;

[Serializable]
public class ProgressData
{
    [HideInInspector] public bool[] levelProgress;
    [HideInInspector] public string[] elementName;
}
