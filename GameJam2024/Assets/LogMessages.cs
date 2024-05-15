using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogMessages : MonoBehaviour
{
    [SerializeField] GameObject messagePrefab;
    float delay = 0;
    public enum typeOfLogMessage
    {
        error,
        warning,
        eveniment,
        normal
    }
    private void Update()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime / 1.2f;
        }
        if (delay < 0)
            delay = 0;
    }
    public void SendMessageInLog(string message, typeOfLogMessage type = typeOfLogMessage.normal)
    {
        StartCoroutine(sendMsgWithDelay(message, type, delay));
        delay += .2f;
    }
    IEnumerator sendMsgWithDelay(string message, typeOfLogMessage type, float delay)
    {
        yield return new WaitForSeconds(delay);
        var msg = Instantiate(messagePrefab, transform);
        msg.transform.GetChild(0).GetComponent<Text>().text = message;
        Color color = Color.white;
        bool bold = false;
        float seconds = 2f;
        switch (type)
        {
            case typeOfLogMessage.error:
                color = Color.red;
                break;
            case typeOfLogMessage.warning:
                color = Color.Lerp(Color.red, Color.yellow, .5f);
                break;
            case typeOfLogMessage.eveniment:
                color = Color.yellow;
                bold = true;
                seconds = 5f;
                break;
            case typeOfLogMessage.normal:
                color = Color.white;
                break;
        }
        msg.transform.GetChild(0).GetComponent<Text>().color = color;
        msg.transform.GetChild(0).GetComponent<Text>().fontStyle = bold ? FontStyle.Bold : FontStyle.Normal;
        StartCoroutine(msg.GetComponent<destroyInSeconds>().destroyInSec(seconds));
    }
}
