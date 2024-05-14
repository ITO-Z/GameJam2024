using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveForButton : MonoBehaviour
{
    public void SetActive()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}
