using UnityEngine;
using UnityEngine.UI;

public class DetailItemDisplay : MonoBehaviour
{
    public Text subTitle;
    public Text skillName;
    public Image skillImg;
    public Text PutOnBtn;
    private GameObject SelectedItem;

    void OnEnable()
    {
        SelectedItem = MouseCtrl.instance.selectedItem;
        if (SelectedItem != null)
        {
            subTitle.text = SelectedItem.GetComponent<Item>().GetItemData();
            skillName.text = SelectedItem.GetComponent<EquipmentItem>().GetSkillName();
            if (skillName.text == "") skillImg.enabled = false;
            else
            {
                skillImg.enabled = true;
                skillImg.sprite = Resources.Load<Sprite>("Skills/" + skillName.text);
            }
            if (SelectedItem.GetComponent<Item>().GetEquipped())
                PutOnBtn.text = "Take OFF";
            else PutOnBtn.text = "Put ON";
        }
    }
    void Start()
    {
    }

    void Update()
    {
        //SelectedItem = MouseCtrl.instance.selectedItem;
        //if (SelectedItem != null)
        //{
        //    subTitle.text = SelectedItem.GetComponent<Item>().GetItemData();
        //    if (SelectedItem.GetComponent<Item>().GetEquipped())
        //        PutOnBtn.text = "Take OFF";
        //    else PutOnBtn.text = "Put ON";
        //}
        //else subTitle.text = "( NO SELECTED. )";
    }


    public void EquipButton()
    {
        if(GameCtrl.instance.
            data[(int)Whose.Player].
            IsEquipped[SelectedItem.GetComponent<Item>().GetItemType()])
        {
            if (!SelectedItem.GetComponent<Item>().GetEquipped())
            {
                Debug.Log("중복 착용이 불가능합니다.");
            }
            else
            {
                SelectedItem.GetComponent<Item>().SetUnEquipItem();
                SelectedItem.GetComponent<Item>().SetEquip(false);
                GameCtrl.instance.data[(int)Whose.Player]
                    .IsEquipped[SelectedItem.GetComponent<Item>().GetItemType()] = false;
                GameCtrl.instance.data[(int)Whose.Player].Equipments[(int)MouseCtrl.instance.selectedItem.GetComponent<Item>().GetItemType()] = null;
                SelectedItem.GetComponent<Item>().ApplyItemAbility(false);
            }
            //
            MouseCtrl.instance.selectedItem.GetComponent<CustomOutline>().enabled = false;
            MouseCtrl.instance.selectedItem = null;
            MouseCtrl.instance.ItemClick = false;
            MouseCtrl.onBtn = false;
        }
        else
        {
            SelectedItem.GetComponent<Item>().SetEquippedPos(SelectedItem.GetComponent<Item>().GetItemType());
            SelectedItem.GetComponent<Item>().SetEquip(true);
            GameCtrl.instance.data[(int)Whose.Player]
                .IsEquipped[SelectedItem.GetComponent<Item>().GetItemType()] = true;
            GameCtrl.instance.data[(int)Whose.Player].Equipments[(int)SelectedItem.GetComponent<Item>().GetItemType()] = MouseCtrl.instance.selectedItem;
            SelectedItem.GetComponent<Item>().ApplyItemAbility(true);
            //
            MouseCtrl.instance.selectedItem.GetComponent<CustomOutline>().enabled = false;
            MouseCtrl.instance.selectedItem = null;
            MouseCtrl.instance.ItemClick = false;
            MouseCtrl.onBtn = false;
        }

        // RENEW
    }

    public void UpgradeButton()
    {

    }
}
