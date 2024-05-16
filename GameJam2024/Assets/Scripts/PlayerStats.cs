using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Resources resources;
    public int conqueredRegions = 0;

    [System.Serializable]
    public class Resource
    {
        public MaterialSO matSo;
        public float amount;
    }
    [System.Serializable]
    public class Resources
    {
        public Resource[] resources;
    }
}
