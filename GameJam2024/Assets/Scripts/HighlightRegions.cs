using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using System.IO;
using JetBrains.Annotations;
using System.Linq;
using System;

public class HighlightRegions : MonoBehaviour
{
    [SerializeField] private Texture2D mapImage;
    [SerializeField] private string filepath;

    void Start()
    {
        LoadRegions();
    }

    void LoadRegions()
{
    // Load pixel color dictionary from the texture
    Dictionary<string, List<Vector2>> pixelColorDict = GetPixelColorDictionary(mapImage);

    // Load key-value pairs array from the JSON file
    KeyValuePair[] keyValuePairs = ReadFromFile(filepath);

    if (keyValuePairs != null)
    {
        Debug.Log("Loaded " + keyValuePairs.Length + " key-value pairs from JSON file.");

        // Iterate through the keyValuePairs array
        foreach (KeyValuePair pair in keyValuePairs)
        {
            Debug.Log("Key: " + pair.key + ", Value: " + pair.value);

            // Perform additional processing as needed with each key-value pair
        }
    }
}


    public Dictionary<string, List<Vector2>> GetPixelColorDictionary(Texture2D texture)
    {
        Dictionary<string, List<Vector2>> pixelColorDict = new Dictionary<string, List<Vector2>>();

        if (texture == null)
        {
            Debug.LogError("Texture is null. Cannot create pixel color dictionary.");
            return pixelColorDict;
        }

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                Color pixelColor = texture.GetPixel(x, y);
                string pixelColorStr = ColorUtility.ToHtmlStringRGB(pixelColor);

                if (!pixelColorDict.ContainsKey(pixelColorStr))
                {
                    pixelColorDict[pixelColorStr] = new List<Vector2>();
                }

                pixelColorDict[pixelColorStr].Add(new Vector2(x, y));
            }
        }

        return pixelColorDict;
    }

    public KeyValuePair[] ReadFromFile(string filepath)
{
    try
    {
        if (string.IsNullOrEmpty(filepath) || !File.Exists(filepath))
        {
            Debug.LogError("Invalid file path or file does not exist: " + filepath);
            return null;
        }

        // Read the file contents as text
        string fileContents = File.ReadAllText(filepath);

        // Deserialize the JSON string into an array of KeyValuePairs
        KeyValuePair[] keyValuePairs = JsonUtility.FromJson<KeyValuePair[]>(fileContents);

        if (keyValuePairs == null)
        {
            Debug.LogWarning("Failed to deserialize JSON file content into KeyValuePairs array.");
            return null;
        }

        return keyValuePairs;
    }
    catch (Exception e)
    {
        Debug.LogError("Error reading JSON file: " + filepath + "\n" + e.Message);
        return null;
    }
}

}

[System.Serializable]
public class KeyValuePair
{
    public string key;
    public string value;
}