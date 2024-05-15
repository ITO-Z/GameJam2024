using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionBehaviour : MonoBehaviour
{
    [HideInInspector] public Region region;
    [SerializeField] LogMessages log;
    [HideInInspector] List<RegionAvailableMatForBuy> availableMatsForBuy = new List<RegionAvailableMatForBuy>();
    [SerializeField] PlayerStats stats;
    public List<GenerateMaterialsWithAmount> generatedMaterials = new List<GenerateMaterialsWithAmount>(4);
    [SerializeField] [Range(0, 12)] int neededConqueredRegions;
    public bool conquered;
    [SerializeField] RegionInfoBhvr infoBhvr;

    public int level = 1;
    public MaterialSO upgradeMaterial;
    public float upgradeCost = 0;
    float upCostConst = 0;
    [System.Serializable]
    public struct canBeConq
    {
        public bool neededMaterials, neededRegionsConq;
    };
    [SerializeField] canBeConq canBeConquered;
    public void Init()
    {
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

        //upgrade cost price
        upCostConst = 0;
        for (int i = 0; i < region.needs.Count; i++)
        {
            var need = region.needs[i];
            upCostConst += need.value / need.value.ToString().ToCharArray().Length;
        }
        upgradeCost = upCostConst * level / (2.5f + region.needs.Count);
        if (upgradeCost < 5)
            upgradeCost = 5;
    }
    private void FixedUpdate()
    {
        if (!conquered)
            CheckAvailability();
    }
    bool logged;
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
        {
            canBeConquered.neededRegionsConq = true;
            if (!logged)
            {
                logged = true;
                log.SendMessageInLog($"{region.regionName} is now conquerable.");
            }
        }
        else canBeConquered.neededRegionsConq = false;

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
            infoBhvr.UpdateListInfo(region, generatedMaterials, this);
            log.SendMessageInLog($"{region.regionName} accepted your offer!", LogMessages.typeOfLogMessage.normal);
        }
        else log.SendMessageInLog($"Not enough resources to buy {region.regionName}", LogMessages.typeOfLogMessage.warning);

    }

    public void UpgradeRegion()
    {
        PlayerStats.Resource resource = null;
        if (conquered && level < 10)
        {
            foreach (var r in stats.resources)
            {
                if (r.matSo == upgradeMaterial)
                {
                    resource = r;
                    break;
                }
            }
            if (resource != null)
            {
                if (resource.amount >= upgradeCost)
                {
                    level++;
                    resource.amount -= upgradeCost;
                    log.SendMessageInLog($"Upgraded {gameObject.name} to level {level}");
                }
                else log.SendMessageInLog($"Not enough {upgradeMaterial.materialName}(s) to upgrade {region.regionName}", LogMessages.typeOfLogMessage.warning);
            }
        }
        upgradeCost = upCostConst * level / (2.5f + region.needs.Count);
        if (upgradeCost < 5)
            upgradeCost = 5;
        infoBhvr.UpdateListInfo(region, generatedMaterials, this);
    }

    //could use some more polish
    //is invoked every week
    public void GenerateMaterials()
    {
        if (conquered)
        {
            foreach (var mat in generatedMaterials)
            {
                foreach (var res in stats.resources)
                {
                    //Debug.Log($"{mat.materialName}, {res.matSo.materialName}");
                    if (res.matSo == mat.matSo && CheckForReqLevel(mat))
                    {
                        res.amount += ModifyMaterialAmountByLevel(mat);
                        break;
                    }
                }
            }
        }
    }
    public float ModifyMaterialAmountByLevel(GenerateMaterialsWithAmount mat)
    {
        return mat.amount + (level - mat.levelNeededToGenerate) * (upgradeCost / 100f);
    }
    public bool CheckForReqLevel(GenerateMaterialsWithAmount mat)
    {
        //Debug.Log($"{mat.levelNeededToGenerate}, {mat.matSo.materialName}, {mat.levelNeededToGenerate <= level}");
        return (mat.levelNeededToGenerate <= level);
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
        public int levelNeededToGenerate = 1;
        public MaterialSO matSo;
        [Range(1, 999)] public float amount;
    }
}
