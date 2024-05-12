using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Region_", menuName = "Regions/Region", order = 1)]
public class Region : ScriptableObject
{
    //the region name can be the same with the ones in regions.txt if you want them to be auto-assigned.
    public string regionName;
    public List<Need> needs = new List<Need>();
}
[System.Serializable]
public class Need
{
    /*public enum Resources
    {
        Wood,
        WoodenPlanks,
    };
    public Resources resource;*/
    public MaterialSO material;
    public int value;
    public bool randomValue = true;
}
