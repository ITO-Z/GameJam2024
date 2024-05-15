using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PlayerStats ps;
    Fader fader;
    private void Start()
    {
        ps = GetComponent<PlayerStats>();
        fader = GetComponent<Fader>();
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
        if (win)
        {
            fader.FadeOutScene("Win");
        }
        else
        {
            fader.FadeOutScene("Lose");
        }
    }
}
