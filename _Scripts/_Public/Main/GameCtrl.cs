using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SkillName = System.String;

// data list
[SerializeField]
public enum Whose
{
    Player,
    NumberOfType
}

[SerializeField]
public enum What
{
    Hp,
    Power,
    MoveSpeed,
    BulletRate,
    BulletDmg,
    BombNum,
    BombDmg,
    SurikenNum,
    SurikenDmg,
    NumberOfType
}

public enum StarColor
{
    Red,
    Yellow,
    Blue,
    NumberOfType
}

public enum SceneState
{
    Open,
    Status,
    Map,
    Battle,
    NumberOfType
}

[System.Serializable]
public class GameCtrl : MonoBehaviour
{
    // Serialize
    public Whose WhoList;
    public What WhatList;

    // 투사체 생성 델리게이트, 매개 변수는 기준이 될 위치의 Transform이며, 반환 값은 쿨타임과 관련있다.
    public delegate float SkillType(Transform t);

    // item id
    public Dictionary<string, bool> existID = new Dictionary<string, bool>();

    // map [stage,isClear?]
    public Dictionary<string, int> lvl = new Dictionary<string, int>()
    {
        // temporary name
        { "Spring",0 },
        { "Summer",1 },
        { "Fall",2 },
        { "Winter",3 },
        { "none", 10000 }
    };

    // constants
    public const int FirstScene = 0;
    private const int NumberOfSkill = 3, ITEM = 0;


    // singleton
    public static GameCtrl instance;

    // player's
    [System.Serializable]
    public class StatusDataList
    {
        // have to rewrite on AttackCtrl?
        public SkillName[] MySkills =
        {
            "FireBall",
            "PFireBall",
            "LightningBall",
            "Teleport",
            "Orb",
            "Meteor",
            "Heal"
        };
        // property
        public float[] currState = new float[(int)What.NumberOfType];
        public float[] maxState = new float[(int)What.NumberOfType]; 

        // item
        public int[] Star = new int[(int)StarColor.NumberOfType];
        public List<GameObject> Inventory = new List<GameObject>();
        // skills
        public GameObject[] Equipments = new GameObject[NumberOfSkill];
        public Dictionary<ItemType, bool> IsEquipped = new Dictionary<ItemType, bool>();

        public StatusDataList()
        {
            IsEquipped[ItemType.Weapon] = false;
            IsEquipped[ItemType.SubWeapon] = false;
            IsEquipped[ItemType.Armor] = false;

            currState[(int)What.MoveSpeed] = 1;

            maxState[(int)What.Hp] = 10.0f;
            currState[(int)What.Hp] = maxState[(int)What.Hp];

            maxState[(int)What.Power] = 5.0f;
            currState[(int)What.Power] = maxState[(int)What.Power];

            currState[(int)What.BulletDmg] = 5.0f;

            currState[(int)What.BulletRate] = 20.0f;

            currState[(int)What.BombNum] = 3.0f;

            currState[(int)What.BombDmg] = 1.0f;

            currState[(int)What.SurikenDmg] = 1.0f;

            currState[(int)What.SurikenNum] = 1.0f;

            Star[(int)StarColor.Red] = 0;
            Star[(int)StarColor.Yellow] = 0;
            Star[(int)StarColor.Blue] = 0;
        }

        public void SetCurrState(float value, params What[] whats)
        {
            foreach (var state in whats)
            {
                currState[(int)state] = value;
            }
        }
    }

    // variable
    public StatusDataList[] data = new StatusDataList[(int)Whose.NumberOfType];
    [HideInInspector]
    public string CurrentLevel;
    [HideInInspector]
    public int playerCost; // SCORE
    [HideInInspector]
    public int enemyCount;
    [HideInInspector]
    public int nextSceneNum = 1, sceneNum = 0, currentMap;
    public StageType currentMapType = StageType.TimeStage;
    public bool[] Scene = new bool[(int)SceneState.NumberOfType];
    public string SceneName, prevSceneName="";
    // 0 = Weapon, 1 = Sub, 2 = Armor
    public int CurrentSkill = 0; public bool IsFirstGameStart = true;

    void Awake()
    {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        
        //player
        data[(int)Whose.Player] = new StatusDataList();
        //scene
        Scene[(int)SceneState.Open] = true;
        //else
        playerCost = 0; currentMap = 0; CurrentLevel = "Spring";
    }

    void Update()
    {
        SceneName = SceneManager.GetActiveScene().name;
        if (SceneName != prevSceneName)
        {
            CurrentSkill = 0;
            MouseCtrl.onBtn = false;
            ButtonCtrl.OnBtn = false;
            prevSceneName = SceneName;
            SetSceneFalseAll();
            if (SceneName == "Status")
            {
                //transform.GetChild(ITEM).gameObject.SetActive(true);
                Scene[(int)SceneState.Status] = true;
            }
            else
            {
                //transform.GetChild(ITEM).gameObject.SetActive(false);
                if (SceneName == "Open") Scene[(int)SceneState.Open] = true;
                else if (SceneName == "Map") Scene[(int)SceneState.Map] = true;
                else if (SceneName == "Battle") Scene[(int)SceneState.Battle] = true;
            }
        }
    }
    public bool IsOpen()
    {
        return Scene[(int)SceneState.Open];
    }

    public bool IsMap()
    {
        return Scene[(int)SceneState.Map];
    }

    public bool IsStatus()
    {
        return Scene[(int)SceneState.Status];
    }

    public bool IsBattle()
    {
        return Scene[(int)SceneState.Battle];
    }

    public void SetSceneFalseAll()
    {
        Scene[(int)SceneState.Open] = false;
        Scene[(int)SceneState.Map] = false;
        Scene[(int)SceneState.Status] = false;
        Scene[(int)SceneState.Battle] = false;
    }


    public void GetItem(GameObject item)
    {
        const int Item = 0;
        data[(int)Whose.Player].Inventory.Add(item);
        item.transform.SetParent(transform.GetChild(Item));
        item.transform.localPosition = Vector3.zero;
    }

    public StatusDataList playerData()
    {
        return data[(int)Whose.Player];
    }

    public int Damage()
    {
        return (int)(Random.Range(0.8f, 1.5f) *
            (data[(int)Whose.Player].currState[(int)What.Power] +
            data[(int)Whose.Player].Equipments[CurrentSkill].GetComponent<EquipmentItem>().GetRGB("Red")));
    }

    public SkillName GetSkill()
    {
        if (data[(int)Whose.Player].Equipments[CurrentSkill] == null)
        {
            //Debug.LogError("장비를 착용하지 않았습니다.");
            return "DontEquipped";
        }
        else return data[(int)Whose.Player].Equipments[CurrentSkill].GetComponent<EquipmentItem>().GetSkill();
    }

    public EquipmentItem GetUseItem()
    {
        return data[(int)Whose.Player].Equipments[CurrentSkill].GetComponent<EquipmentItem>();
    }

}

