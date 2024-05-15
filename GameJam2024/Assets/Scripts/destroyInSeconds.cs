using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class destroyInSeconds : MonoBehaviour
{
    public IEnumerator destroyInSec(float seconds)
    {
        var img = GetComponent<Image>();
        Text text = transform.GetChild(0).GetComponent<Text>();
        if (img != null && text != null)
        {
            Color color1 = img.color;
            Color txtcolor1 = text.color;
            Color color2 = new Color(color1.r, color1.g, color1.b, 0f);
            Color txtcolor2 = new Color(txtcolor1.r, txtcolor1.g, txtcolor1.b, 0f);

            float fadeDuration = .2f; // Duration of the fade effect
            float timer = 0f;

            yield return new WaitForSeconds(seconds);

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / fadeDuration);
                Color color = Color.Lerp(color1, color2, t);
                Color txtcolor = Color.Lerp(txtcolor1, txtcolor2, t);
                img.color = color;
                text.color = txtcolor;
                yield return new WaitForSeconds(.005f);
            }
        }
        else if (img != null)
        {
            Color color1 = img.color;
            Color color2 = new Color(color1.r, color1.g, color1.b, 0f);

            float fadeDuration = .2f; // Duration of the fade effect
            float timer = 0f;

            yield return new WaitForSeconds(seconds);

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / fadeDuration);
                Color color = Color.Lerp(color1, color2, t);
                img.color = color;
                yield return new WaitForSeconds(.005f);
            }

        }
        else if (text != null)
        {
            Color color1 = text.color;
            Color color2 = new Color(color1.r, color1.g, color1.b, 0f);

            float fadeDuration = .2f; // Duration of the fade effect
            float timer = 0f;

            yield return new WaitForSeconds(seconds);

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / fadeDuration);
                Color color = Color.Lerp(color1, color2, t);
                text.color = color;
                yield return new WaitForSeconds(.005f);
            }
        }
        Destroy(gameObject);
    }

}
