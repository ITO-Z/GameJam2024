using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PlayerStats ps;
    Fader fader;
    string saveFilePath;
    public PlayerData playerData = new();
    [SerializeField] List<RegionBehaviour> regions = new();
    [SerializeField] TimeManager clock;
    [SerializeField] GameObject speech;
    private void Start()
    {
        ps = GetComponent<PlayerStats>();
        fader = GetComponent<Fader>();

        saveFilePath = Application.persistentDataPath + "/PlayerData.json";
    }

    private void Update()
    {
        if (ps.conqueredRegions == 13)
        {
            EndGame(true);
        }
    }
    public void EndGame(bool win)
    {
        DeleteSaveFile();
        if (win)
        {
            fader.FadeOutScene("Win");
        }
        else
        {
            fader.FadeOutScene("Lose");
        }
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    void SaveData()
    {
        List<RegionSaveInfo> tempList = new();
        foreach (var r in regions)
        {
            RegionSaveInfo info = new();
            info.region = r.region;
            info.level = r.level;
            info.conquered = r.conquered;
            tempList.Add(info);
            //Debug.Log($"Added {info.region}, {info.level}, {info.conquered}");
        }
        playerData.regions = tempList;
        playerData.conqueredRegions = ps.conqueredRegions;
        playerData.resources = ps.resources;
        playerData.clock = clock.clock;
    }
    void LoadData()
    {
        // Debug.Log("mata");

        foreach (var r in playerData.regions)
        {
            // Debug.Log("ok");
            foreach (var re in regions)
            {
                //Debug.Log("ok1");
                if (r.region == re.region)
                {
                    re.level = r.level;
                    re.conquered = r.conquered;
                    re.transform.GetChild(0).gameObject.SetActive(r.conquered);
                    //Debug.Log($"Added {r.region}, {r.level}, {r.conquered}");
                }
            }
        }
        clock.clock.bc = playerData.clock.bc;
        clock.clock.day = playerData.clock.day;
        clock.clock.month = playerData.clock.month;
        clock.clock.year = playerData.clock.year;
        ps.conqueredRegions = playerData.conqueredRegions;
        ps.resources = playerData.resources;
        speech.SetActive(playerData.speechSeen);
    }
    public void SaveGame()
    {
        SaveData();
        string savePlayerData = JsonUtility.ToJson(playerData);
        File.WriteAllText(saveFilePath, savePlayerData);
    }
    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string loadPlayerData = File.ReadAllText(saveFilePath);
            playerData = JsonUtility.FromJson<PlayerData>(loadPlayerData);
            LoadData();
        }
    }
    [ContextMenu("Delete Save")]
    public void DeleteSaveFile()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);

            Debug.Log("Save file deleted!");
        }
        else
            Debug.Log("There is nothing to delete!");
    }
    public bool CheckFilePath()
    {
        return File.Exists(saveFilePath);
    }
    [System.Serializable]
    public class PlayerData
    {
        public List<RegionSaveInfo> regions = new();
        public PlayerStats.Resources resources = new();
        public int conqueredRegions = 0;
        public Clock clock = new();
        public bool speechSeen = false;
    }
    [System.Serializable]
    public class RegionSaveInfo
    {
        public Region region;
        public int level;
        public bool conquered;
    }
}
