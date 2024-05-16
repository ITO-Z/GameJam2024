using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    bool stop;
    private void Start()
    {
        fadeImage.raycastTarget = false;
        StartCoroutine(FadeIn());
    }

    public void FadeOutScene(string sceneName)
    {
        stop = true;
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = Color.black;

        float t = 1f;
        while (t > 0f)
        {
            if (stop)
                break;
            t -= Time.deltaTime / fadeDuration;
            fadeImage.color = new Color(0f, 0f, 0f, t);
            yield return null;
        }

        fadeImage.gameObject.SetActive(false);
    }

    private IEnumerator FadeOut(string sceneName)
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0f, 0f, 0f, 0f);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            fadeImage.color = new Color(0f, 0f, 0f, t);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}
