using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : ButtonCtrl
{
    private const int buttonCount = 20;
    public Transform ButtonGroup;
    void Start()
    {
        for(int i=0; i< GameCtrl.instance.playerData().MySkills.Length; ++i)
        {
            ButtonGroup.GetChild(i).gameObject.SetActive(true);
            ButtonGroup.GetChild(i).GetComponent<Image>().sprite =
                Resources.Load<Sprite>("Skills/"+ GameCtrl.instance.playerData().MySkills[i]);
        }
        for(int i= GameCtrl.instance.playerData().MySkills.Length; i < buttonCount; ++i)
        {
            ButtonGroup.GetChild(i).gameObject.SetActive(false);
        }
    }

    void Update()
    {
        MouseCtrl.onBtn = OnBtn;
    }
}
