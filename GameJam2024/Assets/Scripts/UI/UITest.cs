using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITest : MonoBehaviour
{
    [HideInInspector] public int AnimatedImageLayer;
    public bool isOverAnimatedImage;

    private void Start()
    {
        AnimatedImageLayer = LayerMask.NameToLayer("UI/AnimatedImg");
    }

    private void FixedUpdate()
    {
        isOverAnimatedImage = IsPointerOverUILayer(AnimatedImageLayer);
    }
    private bool IsPointerOverUILayer(int layer)
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults(), layer);
    }
    public GameObject RaycastResult(int layer)
    {
        return Raycast(GetEventSystemRaycastResults(), layer);
    }
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaycastResults, int layer)
    {
        for (int index = 0; index < eventSystemRaycastResults.Count; index++)
        {
            RaycastResult curRaycastResult = eventSystemRaycastResults[index];
            if (curRaycastResult.gameObject.layer == layer)
                return true;
        }
        return false;
    }
    private GameObject Raycast(List<RaycastResult> eventSystemRaycastResults, int layer)
    {
        for (int index = 0; index < eventSystemRaycastResults.Count; index++)
        {
            RaycastResult curRaycastResult = eventSystemRaycastResults[index];
            if (curRaycastResult.gameObject.layer == layer)
                return curRaycastResult.gameObject;
        }
        return null;
    }
    // Gets all event system raycast results of the current mouse or touch position.
    private static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults;
    }
}
