using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionIdentifier : MonoBehaviour
{
    [SerializeField] Region[] regions;
    [SerializeField] TextAsset regionsData;
    private void Start()
    {
        Identify();
    }
    [ContextMenu("Identify Regions")]
    private void Identify()
    {
        JsonRegions jsonRegions = JsonUtility.FromJson<JsonRegions>(regionsData.text);

        for (int j = 0; j < transform.childCount; j++)
        {
            bool colorFound = false;
            var child = transform.GetChild(j);
            var sprite = child.GetComponent<Image>().sprite;
            var rect = sprite.textureRect;
            for (int x = (int)rect.xMin; x < (int)rect.xMax; x++)
            {
                for (int y = (int)rect.yMin; y < (int)rect.yMax; y++)
                {
                    var pixelColor = sprite.texture.GetPixel(x, y);
                    if (pixelColor.a != 1) continue;
                    //Debug.Log($"#{ColorUtility.ToHtmlStringRGB(pixelColor)}");

                    foreach (var jsonReg in jsonRegions.regions)
                        if (ColorUtility.ToHtmlStringRGB(pixelColor).ToLower() == jsonReg.color.Replace("#", "").ToLower())
                        {
                            colorFound = true;
                            //Debug.Log($"--------------{jsonReg.name}, {jsonReg.color}, #{ColorUtility.ToHtmlStringRGB(pixelColor)}");
                            child.name = jsonReg.name;
                            foreach (var region in regions)
                            {
                                if (region.regionName == jsonReg.name)
                                {
                                    child.GetComponent<RegionBehaviour>().region = region;
                                    child.GetComponent<RegionBehaviour>().Init();
                                }
                            }
                            break;
                        }
                    if (colorFound) break;
                }
                if (colorFound) break;
            }

        }
        Camera.main.GetComponent<GameManager>().LoadGame();
    }
}
[System.Serializable]
public class JsonRegion
{
    public string color;
    public string name;
}
[System.Serializable]
public class JsonRegions
{
    public JsonRegion[] regions;
}