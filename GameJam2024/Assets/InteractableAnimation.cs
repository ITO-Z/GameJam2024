using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableAnimation : MonoBehaviour
{
    [SerializeField] Sprite selectedSprite;
    [SerializeField] Sprite selectedSprite2;
    [SerializeField] Sprite sprite;
    [SerializeField] List<Sprite> animationSprites;
    [SerializeField] UITest uiTest;
    [SerializeField] float timeBtwMoves;
    [SerializeField] List<InteractableAnimation> animations = new List<InteractableAnimation>();
    [SerializeField] List<AnimatedObject> animatedObjects = new List<AnimatedObject>();
    bool active;
    bool animationActive;
    private void Start()
    {
        sprite = GetComponent<Image>().sprite;
        foreach (var o in animatedObjects)
        {
            o.initPos = o.rect.localPosition;
            o.initRot = o.rect.localEulerAngles;
            o.initCol = o.rect.GetComponent<Image>().color;
            o.curPos = o.initPos;
            o.curCol = o.initCol;
            o.curRot = o.initRot;
        }
    }
    private void Update()
    {
        if (uiTest.isOverAnimatedImage && uiTest.RaycastResult(uiTest.AnimatedImageLayer) == gameObject)
        {

            if (!active && !animationActive && selectedSprite != null)
                GetComponent<Image>().sprite = selectedSprite;
            else GetComponent<Image>().sprite = sprite;
            if (Input.GetMouseButtonDown(0) && !active)
            {
                for (int i = 0; i < animations.Count; i++)
                {
                    for (int j = 0; j < animations[i].animatedObjects.Count; j++)
                    {
                        animations[i].GoToInitState(animations[i].animatedObjects[j], .05f * j);
                    }
                }
                StartCoroutine(animate(animatedObjects[0]));
                for (int i = 1; i < animatedObjects.Count; i++)
                {
                    StartCoroutine(animate(animatedObjects[i], .05f * i));
                }
            }
        }
        else GetComponent<Image>().sprite = sprite;
    }
    IEnumerator animate(AnimatedObject ao, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        if (!ao.init)
            ActivateObject(ao);

        if (ao.rect.gameObject == gameObject && animationSprites.Count != 0)
            if (ao.fin)
            {
                animationActive = true;
                for (int i = animationSprites.Count - 1; i >= 0; i--)
                {
                    sprite = animationSprites[i];
                    yield return new WaitForSeconds(timeBtwMoves * 5);
                }
                var auxSpr = selectedSprite;
                selectedSprite = selectedSprite2;
                selectedSprite2 = auxSpr;
                animationActive = false;
            }

        active = true;
        if (!ao.firstTrigger)
        {
            var aux = ao.finalPos;
            ao.finalPos = ao.initPos;
            ao.initPos = aux;

            aux = ao.finalRot;
            ao.finalRot = ao.initRot;
            ao.initRot = aux;

            var auxCol = ao.finalCol;
            ao.finalCol = ao.initCol;
            ao.initCol = auxCol;
        }
        ao.firstTrigger = false;

        float t = 0;
        while (ao.rect.localPosition != ao.finalPos)
        {
            ao.curPos = Vector3.Lerp(ao.initPos, ao.finalPos, t);
            ao.curRot = Vector3.Lerp(ao.initRot, ao.finalRot, t);
            if (t >= .7f || t <= .3f)
                t += .04f;
            else if (t >= .85f || t <= .15f)
                t += .05f;
            else
                t += .03f;

            ao.rect.localPosition = ao.curPos;
            ao.rect.localEulerAngles = ao.curRot;

            yield return new WaitForSeconds(timeBtwMoves);
        }
        t = 0;

        while (ao.rect.GetComponent<Image>().color != ao.finalCol)
        {
            ao.curCol = Color32.Lerp(ao.initCol, ao.finalCol, t);
            if (t >= .7f || t <= .3f)
                t += .04f;
            else if (t >= .85f || t <= .15f)
                t += .05f;
            else
                t += .03f;
            Debug.Log(ao.curCol);
            ao.rect.GetComponent<Image>().color = ao.curCol;

            yield return new WaitForSeconds(timeBtwMoves);
        }
        if (ao.rect.gameObject == gameObject && animationSprites.Count != 0)
            if (ao.init)
            {
                animationActive = true;
                for (int i = 0; i < animationSprites.Count; i++)
                {
                    sprite = animationSprites[i];
                    yield return new WaitForSeconds(timeBtwMoves * 5);
                }
                var auxSpr = selectedSprite;
                selectedSprite = selectedSprite2;
                selectedSprite2 = auxSpr;
                animationActive = false;
            }
        if (ao.init)
            ActivateObject(ao);
        ao.fin = !ao.fin;
        ao.init = !ao.init;
        ao.timesFinished++;
        active = false;
    }
    void ActivateObject(AnimatedObject ao)
    {
        if (ao.activateObj != null)
            ao.activateObj.SetActive(!ao.fin);
    }
    public void GoToInitState(AnimatedObject ao, float delay = 0)
    {
        if (ao.timesFinished % 2 == 1)
            StartCoroutine(animate(ao, delay));
    }
    [System.Serializable]
    public class AnimatedObject
    {
        public bool firstTrigger = true;
        [HideInInspector] public bool fin = false;
        [HideInInspector] public bool init = true;
        [HideInInspector] public int timesFinished = 0;
        public RectTransform rect;
        public GameObject activateObj;
        public Vector3 finalPos;
        public Vector3 finalRot;
        public Color32 finalCol;
        [HideInInspector] public Vector3 initRot;
        [HideInInspector] public Vector3 initPos;
        [HideInInspector] public Color32 initCol;
        [HideInInspector] public Vector3 curRot;
        [HideInInspector] public Vector3 curPos;
        [HideInInspector] public Color32 curCol;
    }
}
