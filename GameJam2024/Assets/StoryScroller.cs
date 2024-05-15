using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryScroller : MonoBehaviour
{
    Text text;
    string curText = "";
    List<string> story = new List<string>();
    [Tooltip("split into paragraphs with |||")] [SerializeField] string storyText;
    int curPar = 0;
    bool writing;
    bool stopAutoScroll;
    [SerializeField] GameObject disableOnEnd;
    [SerializeField] Fader fader;
    // Start is called before the first frame update
    void Start()
    {
        story = new List<string>();
        var stryLines = storyText.Split("|||");
        foreach (var o in stryLines)
        {
            story.Add(o);
        }
        text = GetComponent<Text>();
        StartCoroutine(startParagraph());
    }
    IEnumerator startParagraph()
    {
        yield return new WaitForSeconds(5f);
        if (!stopAutoScroll)
            ProgressToNextParagraph();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            stopAutoScroll = true;
            ProgressToNextParagraph();
        }
    }
    public void ProgressToNextParagraph()
    {
        if (writing)
        {
            writing = false;
            text.text = story[curPar - 1];
            return;
        }
        if (curPar < story.Count)
        {
            LoadParagraph(curPar);
            curPar++;
        }
        else
        {
            if (fader != null)
                fader.FadeOutScene("MainMenu");
            else if (disableOnEnd != null)
                disableOnEnd.SetActive(false);
        }
    }

    private void LoadParagraph(int par)
    {
        curText = "";
        text.text = curText;
        StartCoroutine(Write(story[par].ToCharArray()));
    }
    IEnumerator Write(char[] letters)
    {
        writing = true;
        for (int i = 0; i < letters.Length; i++)
        {
            if (!writing)
                break;
            text.text = curText;
            curText += letters[i];
            yield return new WaitForSeconds(.01f);
        }
        if (writing)
        {
            text.text = curText;
            writing = false;
        }
    }
}
