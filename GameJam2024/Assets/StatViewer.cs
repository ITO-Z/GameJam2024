using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatViewer : MonoBehaviour
{
    [SerializeField] PlayerStats stats;
    [SerializeField] GameObject resourceViewPrefab;
    struct resViewValue
    {
        public MaterialSO matSo;
        public Text text;
    }
    List<resViewValue> resViewValues = new List<resViewValue>();
    private void Start()
    {
        foreach (var res in stats.resources)
        {
            var o = Instantiate(resourceViewPrefab, transform);
            o.transform.GetChild(0).GetComponent<Image>().sprite = res.matSo.icon;
            o.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = res.matSo.materialName;
            o.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = ValueToStringCompact(res.amount);
            resViewValue temp = new resViewValue();
            temp.matSo = res.matSo;
            temp.text = o.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
            resViewValues.Add(temp);
        }
    }
    static public string ValueToStringCompact(float amount)
    {
        if (amount >= 1000)
        {
            return (amount / 1000).ToString("F2") + "k";
        }
        else if (amount >= 1000000)
        {
            return (amount / 1000000).ToString("F2") + "M";
        }
        else if (amount >= 1000000000)
        {
            return (amount / 1000000000).ToString("F2") + "B";
        }
        else if (amount >= 1000000000000)
        {
            return (amount / 1000000000000).ToString("F2") + "B";
        }
        else
            return amount.ToString("F1");
    }
    private void Update()
    {
        foreach (var res in stats.resources)
        {
            foreach (var o in resViewValues)
            {
                if (o.matSo == res.matSo)
                {
                    o.text.text = ValueToStringCompact(res.amount);
                }
            }
        }
    }
}
