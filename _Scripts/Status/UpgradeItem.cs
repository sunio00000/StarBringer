using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItem : MonoBehaviour
{
    public GameObject Effect;
    public GameObject SkillPanel;
    public GameObject FullPanel;
    public Button UseIt;
    public Text Title;
    public Text Detail;
    public static bool CanDo = true,selectedSkill = false;
    private Item SelectedItem;
    private Item ObjectItem;

    void OnEnable()
    {
        UseIt.gameObject.SetActive(true);
        Detail.alignment = TextAnchor.MiddleLeft;
        Detail.color = Color.white;
        Title.text = "CRAFT the Item";
        if (MouseCtrl.instance.selectedItem != null)
        {
            SelectedItem = MouseCtrl.instance.selectedItem.GetComponent<Item>();
            Detail.text = SelectedItem.Detail;
        }
        if (SelectedItem.Class == "SkillBook" && !selectedSkill)
        {
            SkillPanel.SetActive(true);
        }
    }

    void Update()
    {
        if(SelectedItem.Class == "SkillBook" && selectedSkill)
        {
            SkillPanel.SetActive(false);
        }
        if (MouseCtrl.instance.upgradeItem != null) ObjectItem = MouseCtrl.instance.upgradeItem.GetComponent<Item>();
    }

    public void SelectSkill(int number)
    {
        SelectedItem.Name = GameCtrl.instance.playerData().MySkills[number];
        selectedSkill = true;   MouseCtrl.onBtn = false;
    }

    public void Upgrade()
    {
        if(ObjectItem == null)
        {
            Debug.LogError("대상이 없습니다.");
            return;
        }
        SelectedItem.GetComponent<ConsumableItem>().UseItem(ref ObjectItem);
        if (CanDo)
        {
            SelectedItem.UpgradeAnimaion();
            ObjectItem.UpgradeAnimaion();
            Effect.SetActive(true);
            FullPanel.SetActive(true);
            StartCoroutine(Delay());
            //ObjectItem.GetComponent<Item>().OutOfInventory = true;
        }
        CanDo = true;
    }

    IEnumerator Delay()
    {
        UseIt.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        Effect.SetActive(false);
        selectedSkill = false;
        SelectedItem.OutOfInventory = true;
        yield return new WaitForSeconds(0.5f);
        MouseCtrl.onBtn = false;
        FullPanel.SetActive(false);
        Detail.alignment = TextAnchor.MiddleCenter;
        Detail.color = Color.red;
        Detail.text = "Perfect CRAFT";
        // 텍스트 타이틀 바꾸면서, 디테일 설정해주고, 버튼을 끄고 창이 닫힌다.
    }
}
