using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class StatusButtonCtrl : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        MouseCtrl.onBtn = true;
    }


    public void OnPointerExit(PointerEventData pointerEventData)
    {
        MouseCtrl.onBtn = false;
    }
}
