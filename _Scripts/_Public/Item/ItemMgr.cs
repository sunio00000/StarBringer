using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketColor = System.String;

public enum ItemType
{
    Weapon,
    SubWeapon,
    Armor,
    ETC, // socket, usable, buff
    NumberOfType
}

abstract public class ItemMgr : MonoBehaviour
{
    [SerializeField]
    protected ItemData item;
    [System.Serializable]
    protected class ItemData
    {
        public string id;
        public ItemType typeName;
        public string className;
        public string name;
        public float power;
        public float speed;
        public int hp;
        public string detail;
        public bool equipped;
        public List<SocketColor> socketList = new List<SocketColor>();

        private ItemData()
        {
            // Type 에 따라 다른 경우
        }
    }
    protected Vector3 StatusPos;
    protected Vector3 StatusRot;
    protected Vector3 StatusScale;

    abstract protected void Drop();
    abstract protected void Initialize();
    abstract protected void SetUpInventory();
    abstract public string GetItemData();
    // attach : 1 , detach : 0
    abstract public void ApplyItemAbility(bool AD);
    

}
