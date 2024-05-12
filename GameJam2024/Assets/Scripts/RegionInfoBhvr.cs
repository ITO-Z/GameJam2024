using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionInfoBhvr : MonoBehaviour
{
    [SerializeField] GameObject needPrefab;
    [SerializeField] Transform needsList;
    [SerializeField] Text regionTitle;

    [SerializeField] GameObject materialGeneratedPrefab;
    [SerializeField] Transform materialsGeneratedList;
    public void UpdateListInfo(Region region, List<MaterialSO> genMats)
    {
        #region DestroyPastInfo
        List<GameObject> delete = new List<GameObject>();
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
        for (int i = 0; i < region.needs.Count; i++)
        {
            var need = Instantiate(needPrefab, needsList);
            var needIcon = need.transform.GetChild(0).GetComponent<Image>();
            var needName = need.transform.GetChild(0).GetChild(0).GetComponent<Text>();
            var needValue = need.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();

            needIcon.sprite = region.needs[i].material.icon;
            needName.text = region.needs[i].material.materialName;
            needValue.text = ((int)Random.Range(region.needs[i].value, region.needs[i].value + 250)).ToString();
        }
        for (int i = 0; i < genMats.Count; i++)
        {
            if (i >= genMats.Capacity)
                break;
            var mat = Instantiate(materialGeneratedPrefab, materialsGeneratedList);
            mat.transform.GetChild(0).GetComponent<Image>().sprite = genMats[i].icon;
        }
    }
}
