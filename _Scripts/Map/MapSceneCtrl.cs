using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum StageType
{
    // hide
    // 
    Road, // enable to search, unenable to enter
    NormalStage, // enable to enter
    AllKillStage, // enable to enter, battle, allkill
    EndureStage, // enable to enter, battle, endure
    TimeStage, // enable to enter, battle, time
    BossStage, // enable to enter, battle, boss
    NumberOfType
}

[System.Serializable]
public class Map
{
    public string StageName;
    public int StageNum;
    public StageType StageType;
    public bool IsPlayed;
    public bool IsCleared;

    public Map()
    {
        StageName = "";
        StageNum = -1;
    }
}

public class MapSceneCtrl : MonoBehaviour
{
    public static MapSceneCtrl instance;
    public GameObject player;
    public Text mapStatus;
    public Text mapType;
    public Image[] StatusList;
    public Sprite[] Status;
    private Dictionary<string, Sprite> StatusImages=
        new Dictionary<string, Sprite>();
    public Text alarm; private float alpha;
    readonly private Vector3 DefaultVector = Vector3.forward * 0.3f;
    private Stack<Transform> pathToGoal = new Stack<Transform>();
    private Transform startTile;
    private Color alarmColor = new Color(1, 1, 0, 0);
    private Vector3 arrowPos, playerXY;
    private float jumpSpeed = 1.5f, fallSpeed = -1.5f;
    private List<IEnumerator> JumpList = new List<IEnumerator>();
    private IEnumerator current; private bool OnErrorMsg, OnMovePlayer = false;
    public GameObject EventTile;
    public GameObject EventTrigger;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        startTile = GameObject.FindGameObjectWithTag("StartTile").transform;
        PlayerCtrl.instance.mapInitVector = startTile.parent.position - DefaultVector;
        PlayerCtrl.instance.OnMap = true;
        InformStage(null);
        current = Empty();
        foreach(var s in Status)
        {
            StatusImages.Add(s.name, s);
        }
    }

    void Update()
    {
        if (!ButtonCtrl.OnBtn)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(!current.MoveNext() && pathToGoal.Count != 0)
            {
                MovePlayerTo(pathToGoal.Pop().GetChild(0));
            }
            else if (!current.MoveNext() && Physics.Raycast(ray, out hit, 10000.0f) && Input.GetMouseButtonDown(0))
            {
                Transform dest = hit.collider.transform;
                if (ClickTwice(dest.position)) StartCoroutine(cautionTouch("그건 나야."));
                else
                {
                    Debug.Log("Start : " + startTile.parent.name);
                    Debug.Log("Dest : " + dest.parent.name);
                    if(FindShortWayBFS(startTile.parent.name, dest.parent.name)) startTile = dest;
                }
            }
            else if (current.MoveNext() && Physics.Raycast(ray, out hit, 10000.0f) && Input.GetMouseButtonDown(0))
                StartCoroutine(cautionTouch("나 날 수있네!?"));
            else if (Input.GetMouseButtonDown(0) && !MouseCtrl.onBtn)
                StartCoroutine(cautionTouch("거긴 갈 수 없어."));
        }
    }

    public void TmpEvent()
    {
        if (startTile.GetComponent<HexagonCtrl>().Hexagon.StageType == StageType.Road 
            && startTile.GetComponent<HexagonCtrl>().Hexagon.StageNum == -1)
        {
            StartCoroutine(cautionTouch("이동할 수 있는 길이 생겼어!"));
            EventTrigger.SetActive(false);
            EventTile.SetActive(true);
        }
        else
        {
            StartCoroutine(cautionTouch("탐색할 것이 없어."));
        }
    }

    IEnumerator Empty(){ yield return "END"; }

    IEnumerator cautionTouch(string message)
    {
        alarm.text = message;
        if (!OnErrorMsg)
        {
            OnErrorMsg = true;
            while (alpha >= 0)
            {
                alarm.color = alarmColor + new Color(0, 0, 0, alpha -= 0.7f * Time.deltaTime);
                yield return null;
            }
            OnErrorMsg = false;
            alpha = 1;
        }
    }

    IEnumerator JumpToTile(Vector3 v)
    {
        OnMovePlayer = true;
        while (arrowPos.normalized == (v - playerXY).normalized)
        {
            playerXY = player.transform.position - new Vector3(0, 0, player.transform.position.z);
            player.transform.position += arrowPos * Time.deltaTime;
            if (arrowPos.magnitude / 2 > (v - playerXY).magnitude)
                player.transform.position += Vector3.forward * jumpSpeed * Time.deltaTime;
            else
                player.transform.position += Vector3.forward * fallSpeed * Time.deltaTime;
            yield return null;
        }
        pinnedPos(v);
        OnMovePlayer = false;
    }

    public void MapToStatus()
    {
        LoadingScene.LoadScene("Status");
    }

    public void EnterMap()
    {
        if (GameCtrl.instance.currentMap == -1)
        {
            alpha = 1;
            StartCoroutine(cautionTouch("이 맵은 들어갈 수 없어."));
        }
        else if (GameCtrl.instance.currentMap == -100)
            MapToStatus();
        else LoadingScene.LoadScene("Battle");
    }

    public void pinnedPos(Vector3 v)
    {
        player.transform.position = v - DefaultVector;
    }

    public bool ClickTwice(Vector3 v)
    {
        if (player.transform.position.x == v.x && player.transform.position.y == v.y) return true;
        else return false;
    }

    void MovePlayerTo(Transform dest)
    {
        playerXY = player.transform.position - new Vector3(0, 0, player.transform.position.z);
        arrowPos = dest.position - playerXY;
        LookFoward(arrowPos);
        current = JumpToTile(dest.position);
        InformStage(dest);
    }
    //!
    void LookFoward(Vector3 arrow)
    {
        //up
        if (arrow.x == 0 && arrow.y >= 1.0f) { player.transform.rotation = Quaternion.Euler(90, 90, -90); }
        //upright
        else if (arrow.x >= 0.8f && arrow.y >= 0.5f) { player.transform.rotation = Quaternion.Euler(150, 90, -90); }
        //bottomright
        else if (arrow.x >= 0.8f && arrow.y <= -0.5f) { player.transform.rotation = Quaternion.Euler(210, 90, -90); }
        //bottom
        else if (arrow.x == 0 && arrow.y <= -1.0f) { player.transform.rotation = Quaternion.Euler(270, 90, -90); }
        //bottomleft
        else if (arrow.x <= -0.8f && arrow.y <= -0.5f) { player.transform.rotation = Quaternion.Euler(330, 90, -90); }
        //upleft
        else if (arrow.x <= -0.8f && arrow.y >= 0.5f) { player.transform.rotation = Quaternion.Euler(30, 90, -90); }
        //left
        else if (arrow.x <= -0.8f && arrow.y == 0.0f) { player.transform.rotation = Quaternion.Euler(0, 90, -90); }
        //right
        else if (arrow.x >= 0.8f && arrow.y == 0.0f) { player.transform.rotation = Quaternion.Euler(180, 90, -90); }

    }
    private bool FindShortWayBFS(string Start, string Goal)
    {
        string Default = "Tile_", center, top, topleft, topright, bottom, bottomleft, bottomright;
        string[] sNumber = new string[4];
        int x , y;
        Dictionary<string, string> visitedFrom = new Dictionary<string, string>();
        Queue<GameObject> Storage = new Queue<GameObject>();
        visitedFrom[Start] = null;
        Storage.Enqueue(GameObject.Find(Start));
        while (Storage.Count!=0)
        {
            center = Storage.Dequeue().name;
            sNumber = center.Split('_');
            x = int.Parse(sNumber[2]); y = int.Parse(sNumber[1]);
            if (x % 2 == 0)
            {
                top = Default + (y - 1) + "_" + x;
                topleft = Default + y + "_" + (x - 1);
                topright = Default + y + "_" + (x + 1);
                bottom = Default + (y + 1) + "_" + x;
                bottomleft = Default + (y + 1) + "_" + (x - 1);
                bottomright = Default + (y + 1) + "_" + (x + 1);
            }
            else
            {
                top = Default + (y - 1) + "_" + x;
                topleft = Default + (y - 1) + "_" + (x - 1);
                topright = Default + (y - 1) + "_" + (x + 1);
                bottom = Default + (y + 1) + "_" + x;
                bottomleft = Default + y + "_" + (x - 1);
                bottomright = Default + y + "_" + (x + 1);
            }
            if (top == Goal)
            {
                visitedFrom[Goal] = center;
                SavePath(visitedFrom, Goal);
                return true;
            }
            else if (!visitedFrom.ContainsKey(top) &&
                GameObject.Find(top).transform.childCount!=0 &&
                GameObject.Find(top).transform.GetChild(0).gameObject.activeSelf)
            {
                Storage.Enqueue(GameObject.Find(top));
                visitedFrom[top] = center;
            }
            if (topleft == Goal)
            {
                visitedFrom[Goal] = center;
                SavePath(visitedFrom, Goal);
                return true;
            }
            else if (!visitedFrom.ContainsKey(topleft) &&
                GameObject.Find(topleft).transform.childCount != 0 &&
                GameObject.Find(topleft).transform.GetChild(0).gameObject.activeSelf)
            {
                Storage.Enqueue(GameObject.Find(topleft));
                visitedFrom[topleft] = center;
            }
            if (topright == Goal)
            {
                visitedFrom[Goal] = center;
                SavePath(visitedFrom, Goal);
                return true;
            }
            else if (!visitedFrom.ContainsKey(topright) &&
                GameObject.Find(topright).transform.childCount != 0 &&
                GameObject.Find(topright).transform.GetChild(0).gameObject.activeSelf)
            {
                Storage.Enqueue(GameObject.Find(topright));
                visitedFrom[topright] = center;
            }
            if (bottom == Goal)
            {
                visitedFrom[Goal] = center;
                SavePath(visitedFrom, Goal);
                return true;
            }
            else if (!visitedFrom.ContainsKey(bottom) && 
                GameObject.Find(bottom).transform.childCount != 0 &&
                GameObject.Find(bottom).transform.GetChild(0).gameObject.activeSelf)
            {
                Storage.Enqueue(GameObject.Find(bottom));
                visitedFrom[bottom] = center;
            }
            if (bottomleft == Goal)
            {
                visitedFrom[Goal] = center;
                SavePath(visitedFrom, Goal);
                return true;
            }
            else if (!visitedFrom.ContainsKey(bottomleft) &&
                GameObject.Find(bottomleft).transform.childCount != 0 &&
                GameObject.Find(bottomleft).transform.GetChild(0).gameObject.activeSelf)
            {
                Storage.Enqueue(GameObject.Find(bottomleft));
                visitedFrom[bottomleft] = center;
            }
            if (bottomright == Goal)
            {
                visitedFrom[Goal] = center;
                SavePath(visitedFrom, Goal);
                return true;
            }
            else if (!visitedFrom.ContainsKey(bottomright) &&
                GameObject.Find(bottomright).transform.childCount != 0 &&
                GameObject.Find(bottomright).transform.GetChild(0).gameObject.activeSelf)
            {
                Storage.Enqueue(GameObject.Find(bottomright));
                visitedFrom[bottomright] = center;
            }
        }
        StartCoroutine(cautionTouch("연결된 길이 없다.."));
        return false;
    }

    //Function
    private void SavePath(Dictionary<string,string> dss, string Goal)
    {
        string curr = Goal;
        // 목적지 부터 이전 경로를 추적하여 스택에 저장한다.
        // dss["Start"] 의 Value 는 null 이므로, 시작점이 가장 위에 쌓인 스택이 된다.
        while (dss[curr] != null)
        {
            // Stack<Transform> pathToGoal;
            pathToGoal.Push(GameObject.Find(curr).transform);
            curr = dss[curr];
        }
    }

    private void InformStage(Transform dest)
    {
        if(dest == null || dest.name == "startTile")
        {
            GameCtrl.instance.currentMap = -1;
            mapType.text = "Normal";
            mapStatus.text = "Start Of Map";
        }
        else
        {
            GameCtrl.instance.currentMap = dest.GetComponent<HexagonCtrl>().Hexagon.StageNum;
            GameCtrl.instance.currentMapType = dest.GetComponent<HexagonCtrl>().Hexagon.StageType;
            mapType.text = dest.GetComponent<HexagonCtrl>().Hexagon.StageType.ToString();
            mapStatus.text = dest.GetComponent<HexagonCtrl>().Hexagon.StageName;
            SetStageType(dest.GetComponent<HexagonCtrl>().Hexagon.StageType);
        }
    }

    private void SetStageType(StageType type)
    {
        foreach (var sl in StatusList) sl.enabled = true;
        switch (type)
        {
            case StageType.Road:
                StatusList[0].sprite = StatusImages["DontEnter"];
                StatusList[1].sprite = StatusImages["Search"]; 
                StatusList[2].enabled = false;
                StatusList[3].enabled = false;
                break;
            case StageType.NormalStage:
                StatusList[0].sprite = StatusImages["Enter"];
                StatusList[1].enabled = false;
                StatusList[2].enabled = false;
                StatusList[3].enabled = false;
                break;
            case StageType.BossStage:
                StatusList[0].sprite = StatusImages["Enter"];
                StatusList[1].sprite = StatusImages["Battle"]; 
                StatusList[2].sprite = StatusImages["Boss"];
                StatusList[3].enabled = false;
                break;
            case StageType.AllKillStage:
                StatusList[0].sprite = StatusImages["Enter"];
                StatusList[1].sprite = StatusImages["Battle"];
                StatusList[2].sprite = StatusImages["KillAll"];
                StatusList[3].enabled = false;
                break;
            case StageType.TimeStage:
                StatusList[0].sprite = StatusImages["Enter"];
                StatusList[1].sprite = StatusImages["Battle"];
                StatusList[2].sprite = StatusImages["Time"];
                StatusList[3].enabled = false;
                break;
            case StageType.EndureStage:
                StatusList[0].sprite = StatusImages["Enter"];
                StatusList[1].sprite = StatusImages["Battle"];
                StatusList[2].sprite = StatusImages["Endure"];
                StatusList[3].enabled = false;
                break;
            default:
                break;
        }
    }
}
