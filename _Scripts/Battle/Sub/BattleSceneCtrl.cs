using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InteractionObject
{
    public delegate void Event();
    public Dictionary<string, string> keysForText = new Dictionary<string, string>();
    public Dictionary<string, Event> keysForEvent = new Dictionary<string, Event>();
    public string name = "";

    public InteractionObject()
    {
        keysForText[""] = "상호 작용할 대상이 없습니다.";
        keysForText["Legend"] = "포탈을 이용한다.";
        keysForText["BlackSmith"] = "상태 및 아이템을 관리한다.";
        keysForText["Warrior"] = "테스트할 몬스터가 활성화 됩니다.";
        keysForText["Tresure"] = "상자를 엽니다.";

        keysForEvent["Legend"] = () =>
        {
            LoadingScene.LoadScene("Map");
        };
        keysForEvent["BlackSmith"] = () =>
        {
            LoadingScene.LoadScene("Status");
        };
        keysForEvent["Warrior"] = () =>
        {
            //
        };
        keysForEvent["Tresure"] = () =>
        {
            GameObject.Find("Tresure").SetActive(false);
            BattleSceneCtrl.instance.CallEvent();
            BattleSceneCtrl.instance.interactionPanel.SetActive(false);
        };
    }

    public void SetValueAsKey(string name, string contents)
    {
        keysForText[name] = contents;
    }

    public string GetCurrnetText()
    {
        return keysForText[name];
    }

    public Event GetCurrnetEvent()
    {
        return keysForEvent[name];
    }
}

public class BattleSceneCtrl : MonoBehaviour
{
    public static BattleSceneCtrl instance;
    public InteractionObject CurrentIO = new InteractionObject();
    // value cal
    public delegate T StatusChange<T>(T status, T value);
    public T Calculator<T>(T status, T value, StatusChange<T> del)
    {
        return del(status, value);
    }
    public int Value(int a, int b) { return a + b; }
    public float Value(float a, float b) { return a + b; }
    public int MinusValue(int a, int b) { return a - b; }
    public float MinusValue(float a, float b) { return a - b; }

    // variable
    public List<GameObject> SkillSibling =new List<GameObject>();
    public List<Image> SkillImgStorage = new List<Image>();
    public List<string> memo = new List<string>();
    public GameObject EnemyState;
    public GameObject FirstItems, EventItems;
    public float EnemyStateActiveTime=0;
    public Text log;
    private Dictionary<int, AnimationState> SkillAnim = new Dictionary<int, AnimationState>();
    static public float MaxEnemyCount, enemyCount, StartTime;
    private GameObject[] enemy;
    public GameObject playerAttack;
    public GameObject playerBody;
    public GameObject interactionPanel;
    public Text gameState;
    private float delayTime, logY;
    private bool isRealOver = false, isNotFinished = true;
    private float EndValue, CurrValue;
    
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        for (int i = 0; i < GameCtrl.instance.playerData().Equipments.Length; ++i) {
            if (GameCtrl.instance.playerData().Equipments[i] == null ||
                GameCtrl.instance.playerData().Equipments[i].GetComponent<EquipmentItem>().GetSkill() == "") continue;
            SkillImgStorage[i].sprite 
                = Resources.Load<Sprite>("Skills/" +
                GameCtrl.instance.playerData().Equipments[i].GetComponent<EquipmentItem>().GetSkill());
        }
        playerAttack = GameObject.FindWithTag("Muzz");
        playerBody = GameObject.FindWithTag("Body");
        playerAttack.GetComponent<AttackCtrl>().mousePos = GameObject.FindWithTag("Mouse").transform;
        StartTime = -1;
        memo.Add("Stage Start");
    }

    private void Start()
    {
        if (GameCtrl.instance.IsFirstGameStart) FirstScene();
        UIAnimationCheck();
        MaxEnemyCount = EnemyCheck();
        NewScene();
        PlayerStatusCheck();
    }

    private void Update()
    {
        if (GameCtrl.instance.IsBattle())
        {
            if (GameCtrl.instance.sceneNum == GameCtrl.instance.nextSceneNum)
            {
                NewScene();
                GameCtrl.instance.nextSceneNum++;
            }
            if (BattleEnd())
            {
                Debug.Log(isNotFinished);
                if (isNotFinished) FinishCheck();
                if (isRealOver) BattleToStatus();
            }
            EnemyCheck(); PlayerStatusCheck();
        }
    }

    private void NewScene()
    {
        PlayerCtrl.instance.OnBattle = true;
        delayTime = Time.realtimeSinceStartup;
        StartCoroutine(StartTimer());
    }

    private void FirstScene()
    {
        GameCtrl.instance.IsFirstGameStart = false;
        Instantiate(FirstItems);
    }

    public void CallEvent()
    {
        GameObject go = Instantiate(EventItems);
    }

    private float EnemyCheck()
    {
        enemy = GameObject.FindGameObjectsWithTag("ENEMY");
        if (enemy == null) return enemyCount = 0;
        else return enemyCount = enemy.Length;
    }

    static public float EnemyRate()
    {
        return enemyCount / MaxEnemyCount;
    }

    private bool BattleEnd()
    {
        return (ProgressCtrl.GameClear || GameCtrl.instance.data[(int)Whose.Player].currState[(int)What.Hp] <= 0.0f);
    }

    private void FinishCheck()
    {
        isNotFinished = false;
        if (GameCtrl.instance.data[(int)Whose.Player].currState[(int)What.Hp] <= 0.0f)
        {
            GameCtrl.instance.data[(int)Whose.Player].currState[(int)What.Hp] = 0;
            delayTime = Time.realtimeSinceStartup;
            StartCoroutine(DieTimer());
        }
        else if (ProgressCtrl.GameClear)
        {
            delayTime = Time.realtimeSinceStartup;
            StartCoroutine(ClearTimer());
        }
    }
    private void BattleToStatus()
    {
        GameCtrl.instance.currentMap = 0;
        GameCtrl.instance.currentMapType = StageType.NormalStage;
        LoadingScene.LoadScene("Status");
    }

    public void EnemyDisplay(Collider enemy)
    {
        if (enemy.CompareTag("Boss"))
        {
            EnemyState.SetActive(true);
            EnemyState.transform.GetChild(0).GetComponent<Text>().text = enemy.name;
            if (enemy.GetComponent<Enemy>().CurrHp == 0)
            {
                EnemyState.transform.GetChild(1).GetComponent<Image>().color = Color.clear;
                EnemyState.transform.GetChild(1).GetComponent<Image>().fillAmount = 1;
                EnemyState.transform.GetChild(2).GetComponent<Text>().color = Color.red;
                EnemyState.transform.GetChild(2).GetComponent<Text>().text = "DEAD.";
            }
            else
            {
                EnemyState.transform.GetChild(1).GetComponent<Image>().color = Color.red;
                EnemyState.transform.GetChild(1).GetComponent<Image>().fillAmount = enemy.GetComponent<Enemy>().CurrHp / enemy.GetComponent<Enemy>().MaxHp;
                EnemyState.transform.GetChild(2).GetComponent<Text>().color = Color.black;
                EnemyState.transform.GetChild(2).GetComponent<Text>().text = enemy.GetComponent<Enemy>().CurrHp.ToString() + " / " + enemy.GetComponent<Enemy>().MaxHp.ToString();
            }
        }
        else if (enemy.GetComponent<CustomOutline>().enabled != true)
        {
            StartCoroutine(EnemyOutline(enemy));
        }
    }

    private void UIAnimationCheck()
    {
        for (int index = 0; index < SkillSibling.Count; ++index)
        {
            int num = 1;
            foreach (AnimationState state in SkillSibling[index].GetComponent<Animation>())
            {
                SkillAnim[3 * index + (num++)] = state;
            }
        }
    }
    // log detail
    private void PlayerStatusCheck()
    {
        log.text = "";
        foreach(var m in memo)
        {
            log.text += (m+"\n");
        }
        if (GameCtrl.instance.data[(int)Whose.Player].currState[(int)What.Hp] >= GameCtrl.instance.data[(int)Whose.Player].maxState[(int)What.Hp])
            GameCtrl.instance.data[(int)Whose.Player].currState[(int)What.Hp] = GameCtrl.instance.data[(int)Whose.Player].maxState[(int)What.Hp];
    }

    private IEnumerator EnemyOutline(Collider enemy)
    {
        enemy.GetComponent<CustomOutline>().enabled = true;
        yield return new WaitForSeconds(1.5f);
        enemy.GetComponent<CustomOutline>().enabled = false;
    }

    private IEnumerator StartTimer()
    {
        //Time.timeScale = 0.0f;
        int t = 0;
        while (t <= 2)
        {
            t = (int)(Time.realtimeSinceStartup - delayTime);
            if(t==0) gameState.text = GetGameGoal();
            yield return null;
        }
        gameState.enabled = false;
        //Time.timeScale = 1.0f;
        StartTime = Time.time;
    }

    private string GetGameGoal()
    {
        switch (GameCtrl.instance.currentMapType)
        {
            case StageType.AllKillStage:
                return "모든 적을 제압하세요.";
            case StageType.BossStage:
                return "필드 보스를 제압하세요.";
            case StageType.TimeStage:
                return "제한 시간동안 목적을 달성하세요.";
            case StageType.EndureStage:
                return "목표를 추론하고 완료하세요.";
            default:
                return "";
        }
    }

    private IEnumerator ClearTimer()
    {
        PlayerCtrl.instance.animator.SetTrigger("Idle");
        Time.timeScale = 0.0f;
        gameState.enabled = true;
        gameState.text = "Clear";
        int t = 0;
        while (t <= 1)
        {
            t = (int)(Time.realtimeSinceStartup - delayTime);
            yield return null;
        }
        Time.timeScale = 1.0f;
        MapCtrl.CallPortal();
    }

    private IEnumerator DieTimer()
    {
        playerBody.GetComponent<Animator>().SetTrigger("Dead");
        yield return new WaitForSeconds(1.2f);
        playerBody.GetComponent<Animator>().SetTrigger("Idle");
        Time.timeScale = 0.0f;
        gameState.enabled = true;
        gameState.text = "Dead";
        int t = 0;
        while (t <= 2)
        {
            t = (int)(Time.realtimeSinceStartup - delayTime);
            yield return null;
        }
        Time.timeScale = 1.0f;
        isRealOver = true;
    }

    public void LogCycle()
    {
        if (memo.Count > 4) memo.RemoveAt(0);
    }

    public void SkillSwitch()
    {
        for (int index = 0; index < SkillSibling.Count; ++index)
        {
            SkillSibling[index].GetComponent<Animation>().clip = SkillAnim[3 * index + (GameCtrl.instance.CurrentSkill + 1)].clip;
            if (SkillSibling[index].GetComponent<Animation>().clip.name == "Skill_StoM") SkillSibling[index].transform.SetAsLastSibling();
            SkillSibling[index].GetComponent<Animation>().Play();
        }
        GameCtrl.instance.CurrentSkill++;
        GameCtrl.instance.CurrentSkill %= 3;
    }

    public void PlayInteraction()
    {
        if (CurrentIO.name == "") {
            gameState.enabled = true;
            StartCoroutine(cautionTouch(CurrentIO.GetCurrnetText()));
        }
        else
        {
            interactionPanel.SetActive(true);
            Debug.Log(CurrentIO.GetCurrnetText());
            interactionPanel.transform.GetChild(0).GetComponent<Text>().text = CurrentIO.GetCurrnetText();
        }
    }

    public void InteractionOFF()
    {
        interactionPanel.SetActive(false);
    }

    IEnumerator cautionTouch(string message)
    {
        gameState.text = message;
        float alpha = 1f;
        bool OnErrorMsg = false;
        Color alarmColor = Color.yellow;
        if (!OnErrorMsg)
        {
            OnErrorMsg = true;
            while (alpha >= 0)
            {
                gameState.color = alarmColor + new Color(0, 0, 0, alpha -= 0.7f * Time.deltaTime);
                yield return null;
            }
            OnErrorMsg = false;
            alpha = 1;
        }
        gameState.enabled = false;
    }

    public void PlayEvent()
    {
        CurrentIO.GetCurrnetEvent()();
    }

    public void RunPlayer() { }

}
