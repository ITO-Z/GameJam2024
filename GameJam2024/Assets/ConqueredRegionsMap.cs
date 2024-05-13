using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConqueredRegionsMap : MonoBehaviour
{
    [SerializeField] GameObject[] conqueredRegions;
    List<Image> conqRegionsImg = new List<Image>();
    public Color preferedColor;
    Color darkerCol;
    private void Start()
    {
        foreach (var o in conqueredRegions)
        {
            conqRegionsImg.Add(o.GetComponent<Image>());
        }
        float h, s, v;
        Color.RGBToHSV(preferedColor, out h, out s, out v);
        darkerCol = Color.HSVToRGB(h, s, v / 1.75f);
        DarkenRegions();
    }
    void DarkenRegions()
    {
        for (int i = 0; i < conqueredRegions.Length; i++)
        {
            float t = i / (conqueredRegions.Length - 1f);
            var img = conqRegionsImg[Random.Range(0, conqRegionsImg.Count)];
            conqRegionsImg.Remove(img);
            img.color = Color.Lerp(preferedColor, darkerCol, t);
        }
    }
}
