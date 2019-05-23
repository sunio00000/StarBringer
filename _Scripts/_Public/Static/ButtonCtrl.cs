using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCtrl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public static bool OnBtn = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnBtn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnBtn = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnBtn = true;
    }
}
