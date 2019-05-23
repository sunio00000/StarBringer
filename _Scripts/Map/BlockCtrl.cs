using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockCtrl : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[2];
    private bool isClear = false;

    void Update()
    {
        if (isClear)
        {
            transform.GetComponent<Image>().sprite = sprites[1];
        }
        //else transform.GetComponent<Image>().sprite = sprites[0];
    }

    public void ClearThisMap()
    {
        isClear = true;
    }

    public bool GetIsClear()
    {
        return isClear;
    }
}
