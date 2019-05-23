using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillImage : MonoBehaviour
{
    public Sprite[] sprites;
    void Update()
    {
        transform.GetComponent<Image>().sprite = sprites[0];
    }
}
