using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new resource profile", menuName = "Resource/Resource Stats")]
public class ResourceStats : ScriptableObject
{
    public float startValue;
    public float max;
    public float min;
}
