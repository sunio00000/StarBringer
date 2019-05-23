using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{
    [SerializeField]
    private int INumber;
    public override void Awake()
    {
        base.Awake();
        item.typeName = ItemType.ETC;
        gameObject.layer = LayerMask.NameToLayer("CosumableItem");
    }
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        if (this.isSetUp && useToUpgrade) SetPosUpgradePanel();
    }

    protected override void SetPosUpgradePanel()
    {
        SaveInventoryPos();
        transform.position = UVector + AssignedModelPos - new Vector3(0, -4, 12);
        useToUpgrade = false;
    }

    public void UseItem(ref Item obj)
    {
        switch(Class){
            case "Star":
                ApplyPerfectStar(ref obj);
                break;
            case "SkillBook":
                ApplySkillToEquipments(ref obj);
                break;
            default:
                ItHasAnyFunction();
                UpgradeItem.CanDo = false;
                break;
        }
    }

    private void ApplyPerfectStar(ref Item obj)
    {

        obj.GetComponent<EquipmentItem>().AddSocketColor(Name);
    }

    private void ApplySkillToEquipments(ref Item obj)
    {
        obj.GetComponent<EquipmentItem>().SetItemBelongSkill(Name);
    }


    private void ItHasAnyFunction()
    {
        Debug.LogError("할 수 있는 일이 없습니다.");
    }
}
