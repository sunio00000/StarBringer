using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatusSceneCtrl : MonoBehaviour
{
    static public StatusSceneCtrl instance;

    public GameObject Detail;
    public GameObject Upgrade;
    public Transform Inventory;
    public GameObject Items;
    private Transform GameMgr;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        GameMgr = GameObject.FindGameObjectWithTag("GameController").transform;
        Items = GameObject.FindGameObjectWithTag("Item");
    }

    void Start()
    {
        ItemsSetActive(true);
        ItemsSetParent(Inventory);
        Cursor.visible = true;
    }

    void Update()
    {
        if (MouseCtrl.instance.selectedItem != null)
        {
            if (MouseCtrl.instance.selectedItem.GetComponent<Item>().GetItemType() == ItemType.ETC)
            {
                Upgrade.SetActive(true);
            }
            else
            {
                Detail.SetActive(true);
            }
        }
        else
        {
            UpgradeItem.selectedSkill = false;
            Upgrade.GetComponent<UpgradeItem>().SkillPanel.SetActive(false);
            Upgrade.SetActive(false);
            Detail.SetActive(false);
        }
    }
    public void StatusToMap()
    {
        if (GameCtrl.instance.playerData().currState[(int)What.Hp] <= 0) ;
        else
        {
            ItemsSetActive(false);
            ItemsSetParent(GameMgr);
            LoadingScene.LoadScene("Battle");

        }
    }

    public void ItemsSetParent(Transform tr)
    {
        Items.transform.SetParent(tr);
    }

    private void ItemsSetActive(bool act)
    {
        for (int index = 0; index < Items.transform.childCount; ++index)
            Items.transform.GetChild(index).gameObject.SetActive(true);
    }

    public void Cure()
    {
        if (GameCtrl.instance.playerData().currState[(int)What.Hp]
            == GameCtrl.instance.playerData().maxState[(int)What.Hp]) Debug.LogError("체력이 가득합니다.");
        else
        {
            for (int i = 0; i < 3; ++i)
            {
                int upperNum = GameCtrl.instance.playerData().Star[i] / 10;
                int remainNum = GameCtrl.instance.playerData().Star[i] % 10;
                if (remainNum >= 5) GameCtrl.instance.playerData().Star[i] = upperNum + 1;
                else GameCtrl.instance.playerData().Star[i] = upperNum;
            }
            GameCtrl.instance.playerData().currState[(int)What.Hp]
                = GameCtrl.instance.playerData().maxState[(int)What.Hp];
            PlayerCtrl.instance.animator.SetTrigger("Idle");
        }
    }
}
