using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private string saveFilePath;
    [SerializeField] Button continueButton;

    private void Start()
    {
        saveFilePath = Application.persistentDataPath + "/PlayerData.json";
        continueButton.interactable = File.Exists(saveFilePath);
    }
    public void Play(bool newGame = false)
    {
        if (newGame)
            DeleteSaveFile();
        GetComponent<Fader>().FadeOutScene("Map");
    }

    public void Quit()
    {
        Application.Quit();
    }
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
}
