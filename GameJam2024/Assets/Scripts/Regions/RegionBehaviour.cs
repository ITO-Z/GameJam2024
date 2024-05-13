using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionBehaviour : MonoBehaviour
{
    [HideInInspector] public Region region;
    [SerializeField] bool startingRegion;
    [HideInInspector] List<RegionAvailableMatForBuy> availableMatsForBuy = new List<RegionAvailableMatForBuy>();
    [SerializeField] PlayerStats stats;
    public List<GenerateMaterialsWithAmount> generatedMaterials = new List<GenerateMaterialsWithAmount>(4);
    [SerializeField] [Range(1, 12)] int neededConqueredRegions;
    [SerializeField] public bool conquered;
    [System.Serializable]
    public struct canBeConq
    {
        public bool neededMaterials, neededRegionsConq;
    };
    [SerializeField] canBeConq canBeConquered;
    public void Init()
    {
        conquered = startingRegion;
        transform.GetChild(0).gameObject.SetActive(conquered);
        if (!conquered)
        {
            foreach (var o in region.needs)
            {
                RegionAvailableMatForBuy q = new RegionAvailableMatForBuy();
                q.enough = false;
                q.matSo = o.material;
                availableMatsForBuy.Add(q);
            }
        }
    }
    private void FixedUpdate()
    {
        if (!conquered)
            CheckAvailability();
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

        if (!startingRegion)
            GetComponent<InteractableAnimation>().inactive = !canBeConquered.neededRegionsConq;
    }
    [ContextMenu("Buy Region")]
    public void BuyRegion()
    {
        if (conquered)
            return;
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
            stats.conqueredRegions++;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    //could use some more polish
    //is invoked every day
    public void GenerateMaterials()
    {
        if (conquered || startingRegion)
        {
            foreach (var mat in generatedMaterials)
            {
                foreach (var res in stats.resources)
                {
                    //Debug.Log($"{mat.materialName}, {res.matSo.materialName}");
                    if (res.matSo == mat.matSo)
                    {
                        res.amount += mat.amount;
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
    [System.Serializable]
    public class GenerateMaterialsWithAmount
    {
        public MaterialSO matSo;
        [Range(1, 999)] public int amount;
    }
}
