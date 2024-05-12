using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionBehaviour : MonoBehaviour
{
    [HideInInspector] public Region region;
    [SerializeField] bool startingRegion;
    [HideInInspector] List<RegionAvailableMatForBuy> availableMatsForBuy = new List<RegionAvailableMatForBuy>();
    [SerializeField] PlayerStats stats;
    public List<MaterialSO> generatedMaterials = new List<MaterialSO>(4);
    [SerializeField] [Range(1, 12)] int neededConqueredRegions;
    bool conquered;
    struct canBeConq
    {
        public bool neededMaterials, neededRegionsConq;
    };
    canBeConq canBeConquered;
    public void Init()
    {
        if (!startingRegion)
        {
            foreach (var o in region.needs)
            {
                RegionAvailableMatForBuy q = new RegionAvailableMatForBuy();
                q.enough = false;
                q.matSo = o.material;
                availableMatsForBuy.Add(q);
            }
            InvokeRepeating("CheckAvailability", .1f, .1f);
        }
    }
    void CheckAvailability()
    {
        //check for materials
        for (int i = 0; i < region.needs.Count; i++)
        {
            var need = region.needs[i];
            foreach (var resource in stats.resources)
            {
                if (need.material == resource.matSo)
                {
                    if (resource.amount >= need.value)
                    {
                        availableMatsForBuy[i].enough = true;
                    }
                    else
                    {
                        availableMatsForBuy[i].enough = false;
                    }
                    break;
                }
            }
        }
        foreach (var o in availableMatsForBuy)
        {
            canBeConquered.neededMaterials = true;
            if (!o.enough)
            {
                canBeConquered.neededMaterials = false;
                break;
            }
        }
        //check for neededConqueredRegions
        if (stats.conqueredRegions >= neededConqueredRegions)
            canBeConquered.neededRegionsConq = true;
        else canBeConquered.neededRegionsConq = false;

        if (!canBeConquered.neededMaterials || !canBeConquered.neededRegionsConq)
            GetComponent<InteractableAnimation>().inactive = true;
        else if (canBeConquered.neededMaterials && canBeConquered.neededRegionsConq)
            GetComponent<InteractableAnimation>().inactive = false;
    }
    public void BuyRegion()
    {
        CheckAvailability();
        if (canBeConquered.neededMaterials && canBeConquered.neededRegionsConq)
        {
            for (int i = 0; i < region.needs.Count; i++)
            {
                var need = region.needs[i];
                foreach (var resource in stats.resources)
                {
                    if (need.material == resource.matSo)
                    {
                        resource.amount -= need.value;
                        break;
                    }
                }
            }
            conquered = true;
        }
    }
    //could use some more polish
    //is invoked every day
    public void GenerateMaterials(int amount)
    {
        if (conquered || startingRegion)
        {
            foreach (var mat in generatedMaterials)
            {
                foreach (var res in stats.resources)
                {
                    //Debug.Log($"{mat.materialName}, {res.matSo.materialName}");
                    if (res.matSo == mat)
                    {
                        Debug.Log("ok");
                        res.amount += amount;
                        break;
                    }
                }
            }
        }
    }
    [System.Serializable]
    public class RegionAvailableMatForBuy
    {
        public MaterialSO matSo;
        public bool enough = false;
    }
}
