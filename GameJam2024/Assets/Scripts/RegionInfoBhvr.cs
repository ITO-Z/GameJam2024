using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionInfoBhvr : MonoBehaviour
{
    [SerializeField] Color[] bgColors;
    [SerializeField] GameObject needPrefab;
    [SerializeField] Transform needsList;
    [SerializeField] GameObject needsListParent;
    [SerializeField] GameObject needsTitle;
    [SerializeField] Text regionTitle;
    [Space]
    [SerializeField] GameObject materialGeneratedPrefab;
    [SerializeField] Transform materialsGeneratedList;
    [Space]
    [SerializeField] Button buyButton;
    [SerializeField] GameObject buyButtonParent;
    [Space]
    [SerializeField] Button upgradeButton;
    [SerializeField] GameObject upgradeButtonParent;
    [Space]
    [SerializeField] Slider levelSlider;
    public void UpdateListInfo(Region region, List<RegionBehaviour.GenerateMaterialsWithAmount> genMats, RegionBehaviour regBhvr)
    {
        #region DestroyPastInfo
        List<GameObject> delete = new List<GameObject>();
        buyButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.RemoveAllListeners();

        for (int i = 0; i < needsList.childCount; i++)
        {
            delete.Add(needsList.GetChild(i).gameObject);
        }
        for (int i = 0; i < materialsGeneratedList.childCount; i++)
        {
            delete.Add(materialsGeneratedList.GetChild(i).gameObject);
        }
        foreach (var o in delete)
        {
            Destroy(o);
        }
        #endregion
        regionTitle.text = region.regionName;

        if (regBhvr.conquered)
        {
            levelSlider.transform.parent.gameObject.SetActive(true);
            levelSlider.value = regBhvr.level;
            GetComponent<Image>().color = bgColors[1];

            var resIcon = upgradeButtonParent.transform.Find("Resource").GetChild(0).GetComponent<Image>();
            var resAmount = upgradeButtonParent.transform.Find("Resource").GetChild(0).GetChild(0).GetComponent<Text>();

            resIcon.sprite = regBhvr.upgradeMaterial.icon;
            resAmount.text = $"x{regBhvr.upgradeCost:F1}";

            needsListParent.SetActive(false);
            needsTitle.SetActive(false);
            buyButtonParent.SetActive(false);
            upgradeButtonParent.SetActive(regBhvr.level >= 10 ? false : true);
        }
        else
        {
            levelSlider.transform.parent.gameObject.SetActive(false);
            GetComponent<Image>().color = bgColors[0];
            needsListParent.SetActive(true);
            needsTitle.SetActive(true);
            buyButtonParent.SetActive(true);
            upgradeButtonParent.SetActive(false);
        }
        if (regBhvr != null)
        {
            buyButton.onClick.AddListener(regBhvr.BuyRegion);
            upgradeButton.onClick.AddListener(regBhvr.UpgradeRegion);
        }

        for (int i = 0; i < region.needs.Count; i++)
        {
            var need = Instantiate(needPrefab, needsList);
            var needIcon = need.transform.GetChild(0).GetComponent<Image>();
            var needName = need.transform.GetChild(0).GetChild(0).GetComponent<Text>();
            var needValue = need.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();

            needIcon.sprite = region.needs[i].material.icon;
            needName.text = region.needs[i].material.materialName;
            needValue.text = region.needs[i].value.ToString();
        }
        for (int i = 0; i < genMats.Count; i++)
        {
            if (i >= genMats.Capacity) break;
            if (!regBhvr.CheckForReqLevel(genMats[i])) continue;
            var mat = Instantiate(materialGeneratedPrefab, materialsGeneratedList);
            mat.transform.GetChild(0).GetComponent<Image>().sprite = genMats[i].matSo.icon;
            mat.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = $"x{regBhvr.ModifyMaterialAmountByLevel(genMats[i]):F1}";
        }
    }
}
