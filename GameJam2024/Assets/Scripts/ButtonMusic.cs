using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMusic : MonoBehaviour
{
    private SoundManager soundManager;
    // Start is called before the first frame update

    void Start()
    {
        Button button = GetComponent<Button>();
        // adding a delegate with no parameters
        button.onClick.AddListener(ButtonClicked);
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();

    }
    void ButtonClicked(){
        soundManager.Play(1);
    }

}
