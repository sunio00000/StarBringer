using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillName = System.String;
using SocketColor = System.String;

public class EquipmentItem : Item
{
    public GameObject Pivot;

    public class RGB
    {
        int R, G, B;
        public RGB() { }
        public RGB(int r, int g, int b) {
            R = r; G = g; B = b;
        }
    }

    [SerializeField]
    private SkillName PossessSkill;

    public override void Awake()
    {
        base.Awake();
        gameObject.layer = LayerMask.NameToLayer("Item");
    }

    public override void Start()
    {
        base.Start();
        SetPivot();

    }

    public override void Update()
    {
        base.Update();
        if (this.isSetUp && readyToUpgrade) SetPosUpgradePanel();
    }

    protected override void SetPosUpgradePanel()
    {
        SaveInventoryPos();
        if (IsSwordOrShield())
        {
            transform.parent.position = UVector + AssignedModelPos + new Vector3(0, -1, 12);
            if (tag == "Shield") transform.parent.position += Vector3.up * 2.0f;
        }
        else transform.position = UVector + AssignedModelPos + new Vector3(0, 2, 12);
        readyToUpgrade = false;
    }

    private void SetPivot()
    {
        if(IsSwordOrShield())
        {
            Pivot = Instantiate(Resources.Load<GameObject>("Items/Pivot"));
            Pivot.transform.SetParent(transform.parent);
            transform.SetParent(Pivot.transform,isTaken);
            if (tag == "Sword")
            {
                transform.position = SwordFromPivot - Vector3.up * transform.GetComponent<BoxCollider>().size.y / 4;
                Pivot.tag = "Sword";
            }
            else if (tag == "Shield")
            {
                transform.position = ShieldFromPivot - Vector3.up * transform.GetComponent<BoxCollider>().size.y / 4;
                Pivot.tag = "Shield";
            }
            transform.parent.name = transform.name + "_Pivot";
            transform.parent.localPosition = DropVector;
            //if(isTaken) Pivot.transform.position = new Vector3(-100, 0, 0);
        }
    }
    public void SetItemBelongSkill(SkillName skill)
    {
        PossessSkill = skill;
    }
    public SkillName GetSkill()
    {
        return PossessSkill;
    }

    public void AddSocketColor(SocketColor colorType)
    {
        if (colorType != "")
        {
            for (int i = 0; i < GetSocketCount(); ++i)
            {
                if (item.socketList[i] == "")
                {
                    item.socketList[i] = colorType;
                    return;
                }
            }
            Debug.LogError("Socket이 가득 찼습니다.");
        }

    }

    public void AddSocket()
    {
        Queue<SocketColor> container = new Queue<SocketColor>(item.socketList);
        container.Enqueue("");
        item.socketList = new List<SocketColor>(container);
        container.Clear();
    }

    public SkillName GetSkillName()
    {
        return PossessSkill;
    }

    public int GetRGB(string jewelName)
    {
        int count = 0;
        for(int i=0; i<item.socketList.Count; ++i)
            if (item.socketList[i] == jewelName)
                count++;
        return count;
    }
}
