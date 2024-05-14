using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public List<Resource> resources;
    public int conqueredRegions = 0;

    [System.Serializable]
    public class Resource
    {
        public MaterialSO matSo;
        public float amount;
    }
}
