using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketColor = System.String;
using UnityEngine.SceneManagement;

abstract public class Item : ItemMgr
{
    public Transform Player;
    public Vector3 AssignedModelRot = Vector3.zero;
    public Vector3 AssignedModelPos = Vector3.zero;
    public Vector3 DropVector = Vector3.zero;
    //[HideInInspector]
    public bool isTaken = false, isSetUp = false, OutOfInventory = false, onFloor = true,
        sceneChanged = false, readyToUpgrade = false, useToUpgrade = false;

    private const float G = 9.8f;
    private const float Vy = 7.0f;
    private float dropTime;
    protected Vector3 UVector = new Vector3(-90, -1.5f, 4.5f);
    protected float UCenter = 4f;
    protected Vector3 EWeapon = new Vector3(-90, -5.7f, 50);
    protected Vector3 EArmor = new Vector3(-90, 18.5f, 50);
    protected Vector3 ESub = new Vector3(-90, -21, 50);

    protected Vector3 SwordFromPivot = new Vector3(-0.25f, 0f, 0.15f);
    protected Vector3 ShieldFromPivot = new Vector3(0.25f, 0f, 0.22f);
    protected Vector3 BSwordFromPivot = new Vector3(-0.75f, 0f, 0.15f);
    protected Vector3 BShieldFromPivot = new Vector3(0.75f, 0f, 0.22f);
    private float scaleFactor = 20f;

    public string ID
    {
        get { return item.id; }
    }
    public ItemType Type
    {
        get { return item.typeName; }
    }
    public string Class
    {
        get { return item.className; }
    }
    public string Name
    {
        get { return item.name; }
        set { item.name = value; }
    }
    public float Power
    {
        get { return item.power; }
        set { item.power = value; }
    }
    public float Speed
    {
        get { return item.speed; }
        set { item.speed = value; }
    }
    public int HP
    {
        get { return item.hp; }
        set { item.hp = value; }
    }
    public string Detail
    {
        get { return item.detail; }
    }
    public bool Equip
    {
        get { return item.equipped; }
        set { item.equipped = value; }
    }
        

    abstract protected void SetPosUpgradePanel();

    //void OnValidate()
    //{
    //    if (gameObject.GetComponent<CustomOutline>() == null)
    //    {
    //        Debug.Log("no outline");
    //        gameObject.AddComponent<CustomOutline>();
    //        gameObject.GetComponent<CustomOutline>().enabled = false;
    //        gameObject.GetComponent<CustomOutline>().SetPRECompute();
    //    }
    //    if (gameObject.GetComponent<BoxCollider>() == null)
    //    {
    //        Debug.Log("no boxcollider");
    //        gameObject.AddComponent<BoxCollider>();
    //    }
    //}

    public virtual void Awake()
    {
        Player = GameObject.FindWithTag("Body").transform;
    }

    public virtual void Start()
    {
        Initialize();
    }

    public virtual void Update()
    {
        if (GameCtrl.instance.IsStatus())
        {
            if (!isSetUp)
            {
                SetUpInventory();
                sceneChanged = false;
            }
            else if (isSetUp)
            {
                if (readyToUpgrade) SetPosUpgradePanel();
                else if (OutOfInventory) SetUpInventory();
            }
        }
        else
        {
            sceneChanged = true;
            if (GameCtrl.instance.IsBattle())
            {
                if (onFloor)
                {
                    StartCoroutine(OnFloor());
                }
                if (GameCtrl.instance.currentMap != 0 && !isTaken)
                    Drop();
            }
            else if (GameCtrl.instance.IsMap() || GameCtrl.instance.IsOpen())
            {
                //isSetUp = false;
            }
        }
        
    }

    protected override void Drop()
    {
        if (transform.position.y > 0.05f) transform.position += new Vector3(0, (Vy - (G * (Time.time - dropTime))) * Time.deltaTime, 0);
        if (onFloor) StartCoroutine(OnFloor());
    }

    protected override void Initialize()
    {
       
        //if (!IsSwordOrShield())
        //    transform.position += Vector3.up;
        //else
        //    transform.parent.position += Vector3.up;
        dropTime = Time.time;
        StatusPos = new Vector3(-90, Random.Range(-40.0f, 45.0f), Random.Range(-90.0f, -50.0f));
        StatusRot = AssignedModelRot;
        transform.rotation = Quaternion.Euler(StatusRot);
        //if (!IsSwordOrShield())
        //    transform.rotation = Quaternion.Euler(StatusRot);
        //else
        //{
        //    Debug.Log(transform.parent.name);
        //    transform.parent.rotation = Quaternion.Euler(StatusRot);
        //}
        while(true)
        {
            item.id = Random.Range(0, 10000).ToString();
            if (GameCtrl.instance.existID.ContainsKey(ID)) continue;
            GameCtrl.instance.existID[ID] = true;
            break;
        } 
    }

    private bool InventoryBoundIn()
    {
        Transform t = IsSwordOrShield() ? transform.parent : transform;
        if ((t.position.z > -50.0f || t.position.z < -90.0f)) return false;
        else return true;
    }

    protected override void SetUpInventory()
    {
        if (!Equip)
        {
            if (!IsSwordOrShield())
            {
                transform.position = StatusPos;
                transform.rotation = Quaternion.Euler(StatusRot);
                if(!isSetUp) transform.localScale *= scaleFactor;

            }
            else
            {
                transform.parent.position = StatusPos;
                transform.parent.rotation = Quaternion.Euler(StatusRot);
                if (!isSetUp) transform.parent.localScale *= scaleFactor;
            }
            isSetUp = true; OutOfInventory = false;
        }
    }

    public void SetEquippedPos(ItemType type)
    {
        if (!OutOfInventory) SaveInventoryPos();
        OutOfInventory = true;
        if(type == ItemType.Armor)
        {
            if (!GetEquipped())
            {
                transform.position = EArmor;
                Player.GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>("Items/Armor/"+transform.name);
            }
        }
        else if(type == ItemType.Weapon)
        {
            if (!GetEquipped())
            {
                transform.parent.position = EWeapon;
                Transform weapon = Instantiate(Resources.Load<GameObject>("Items/Sword/"+transform.name)).transform;
                weapon.parent = Player.GetChild(0);
                weapon.localRotation = Quaternion.identity;
                weapon.localPosition = Vector3.zero;
                weapon.localScale = Vector3.one;
            }
        }
        else if(type == ItemType.SubWeapon)
        {
            if (!GetEquipped())
            {
                transform.parent.position = ESub;
                Transform Sheild = Instantiate(Resources.Load<GameObject>("Items/Shield/" + transform.name)).transform;
                Sheild.parent = Player.GetChild(1);
                Sheild.localRotation = Quaternion.identity;
                Sheild.localPosition = Vector3.zero;
                Sheild.localScale = Vector3.one;

            }
        }
    }

    public void SetUnEquipItem()
    {
        if (item.typeName == ItemType.Armor)
        {
            Player.GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>("Items/Armor/armor001");
        }
        else if (item.typeName == ItemType.Weapon)
        {
            Destroy(Player.GetChild(0).GetChild(1).gameObject);
        }
        else if (item.typeName == ItemType.SubWeapon)
        {
            Destroy(Player.GetChild(1).GetChild(0).gameObject);
        }
    }

    public override string GetItemData()
    {
        return
           "ID : " + ID + "\n" +
           "TYPE : " + Type + "\n" +
           "STATS : " + Power + "/" + HP + "/" + Speed + "\n" +
           "ItemName : " + gameObject.name + "\n" +
           "Equipped : " + (Equip ? "Y" : "N") + "\n" +
           "Details\n  " + Detail;
    }

    public override void ApplyItemAbility(bool ADetach)
    {
        if (ADetach)
        {
            GameCtrl.instance.data[(int)Whose.Player].currState[(int)What.Power] += Power;
            GameCtrl.instance.data[(int)Whose.Player].currState[(int)What.MoveSpeed] += Speed;
            GameCtrl.instance.data[(int)Whose.Player].maxState[(int)What.Hp] += HP;
        }
        else
        {
            GameCtrl.instance.data[(int)Whose.Player].currState[(int)What.Power] -= Power;
            GameCtrl.instance.data[(int)Whose.Player].currState[(int)What.MoveSpeed] -= Speed;
            GameCtrl.instance.data[(int)Whose.Player].maxState[(int)What.Hp] -= HP;
        }
    }

    protected void SaveInventoryPos()
    {
        if (!IsSwordOrShield())
        {
            StatusPos = transform.position;
            StatusRot = transform.rotation.eulerAngles;
        }
        else
        {
            StatusPos = transform.parent.position;
            StatusRot = transform.parent.rotation.eulerAngles;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            BattleSceneCtrl.instance.memo.Add("GET "+gameObject.name);
            BattleSceneCtrl.instance.LogCycle();
            isTaken = true;
            if (IsSwordOrShield())
                GameCtrl.instance.GetItem(transform.parent.gameObject);
            else
                GameCtrl.instance.GetItem(gameObject);
        }
    }

    IEnumerator OnFloor()
    {
        onFloor = false;
        while (gameObject != null)
        {
            if (IsSwordOrShield()) transform.parent.Rotate(new Vector3(0, Time.deltaTime * 100f, 0), Space.World);
            else transform.Rotate(new Vector3(0, Time.deltaTime * 100f, 0), Space.World);
            yield return null;
        }
    }


    public ItemType GetItemType()
    {
        return item.typeName;
    }

    public bool GetEquipped()
    {
        return item.equipped;
    }

    public string GetItemDetail()
    {
        return item.detail;
    }

    public void SetEquip(bool state)
    {
        item.equipped = state;
    }

    public int GetSocketCount()
    {
        return item.socketList.Count;
    }

    public SocketColor GetSocketColor(int index)
    {
        if (index >= GetSocketCount()) return "index not assigned.";
        else return item.socketList[index];
    }

    public bool IsSwordOrShield()
    {
        if (transform.CompareTag("Sword") || transform.CompareTag("Shield") || 
            transform.parent.CompareTag("Sword") || transform.parent.CompareTag("Shield")) return true;
        else return false;
    }

    public void UpgradeAnimaion()
    {
        int dir;
        if (IsSwordOrShield()) dir = transform.parent.position.z > UCenter ? -5 : 5;
        else dir = transform.position.z > UCenter ? -5 : 5;
        StartCoroutine(UpgradeAnimator(dir));
    }

    IEnumerator UpgradeAnimator(int direction)
    {
        Transform t = IsSwordOrShield()? transform.parent : transform;
        while (Mathf.Abs(t.position.z-UCenter) >0.1f && !InventoryBoundIn())
        {
            t.position += direction * Vector3.forward * Time.deltaTime;
            yield return null;
        }
    }
}
