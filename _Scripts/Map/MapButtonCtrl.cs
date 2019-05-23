using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MapButtonCtrl : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
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
