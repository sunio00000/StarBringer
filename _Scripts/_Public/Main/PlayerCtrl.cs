using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public static PlayerCtrl instance;
    public GameObject spot;
    public GameObject UI;
    private const int Hp = 0, Cool =1;
    public GameObject SFX;
    public GameObject floor;
    public float rotBody;
    public Animator animator;
    private float fowardMoveSpeed;
    private float leftMoveSpeed;
    private int currentAnimation;
    public List<string> animations;
    public Vector3 mapInitVector;
    public bool OnMap = false, OnBattle = false;
    public CustomOutline CO;
    public ParticleSystem damagedParticle;
    private Vector3 behindVector = new Vector3(10000, 10000, 10000);
    private Vector3 statusVector = new Vector3(-420, -120, 50);
    private Vector3 statusSize = new Vector3(100f, 100f, 100f);
    private Vector3 mapPlayerSize = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 elsePlayerSize = new Vector3(0.8f, 0.8f, 0.8f);
    private Vector3 battleSize = new Vector3(0.2f, 0.2f, 0.2f);


    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        animations = new List<string>()
        {
            "Attack", //attack
            "Active", //stun
            "Passive" //dead
        };
    }
    void Update()
    {
        if (GameCtrl.instance.IsBattle())
        {
            if (OnBattle)
            {
                SetPlayer(GameObject.FindWithTag("PlayerSpawn").transform.position, Quaternion.identity, battleSize);
                SetPlayerChildPos(Vector3.zero, Quaternion.identity);
                GameObject.FindWithTag("MainCamera").GetComponent<CameraCtrl>().SetSceneInit(new Vector3(0, 2.5f, -2), new Vector3(55, 0, 0));
                CO.enabled = true;
                GetComponent<Rigidbody>().isKinematic = false;
                UIActive(false, true, true);
                OnBattle = false;
            }
            UIcheck();
        }
        else if (GameCtrl.instance.IsStatus())
        {
            CO.enabled = false;
            SetPlayer(statusVector, Quaternion.Euler(new Vector3(0, -90, 0)), statusSize);
            if(GameCtrl.instance.data[(int)Whose.Player].currState[(int)What.Hp] <= 0) animator.SetTrigger("Idle");
            else SetAnimationIdle();
            GetComponent<Rigidbody>().isKinematic = true;
            UIActive(true, false, false);
            SFXActive(false, true);
        }
        else if (GameCtrl.instance.IsMap() && OnMap)
        {
            CO.enabled = true;
            SetPlayer(mapInitVector, Quaternion.Euler(new Vector3(-90, 90, -90)), mapPlayerSize);
            SetPlayerChildPos(Vector3.zero, Quaternion.Euler(new Vector3(0, 0, 0)));
            GetComponent<Rigidbody>().isKinematic = true;
            AllOff();
            OnMap = false;
        }
        else if(GameCtrl.instance.IsOpen())
        {
            SetPlayer(behindVector, Quaternion.identity, elsePlayerSize);
            AllOff();
        }
        else
        {
            AllOff();
        }
    }

    void UIcheck()
    {
        UI.transform.GetChild(Hp).GetComponent<Image>().fillAmount = GameCtrl.instance.data[(int)Whose.Player].currState[(int)What.Hp] / GameCtrl.instance.data[(int)Whose.Player].maxState[(int)What.Hp];
        UI.transform.GetChild(Cool).GetComponent<Image>().fillAmount = AttackCtrl.instance.CoolTime;
    }

    void UIActive(bool _floor, bool _ui, bool _spot)
    {
        if (floor.activeSelf == _floor && UI.activeSelf == _ui && spot.activeSelf == _spot) return;

        floor.SetActive(_floor);
        UI.SetActive(_ui);
        spot.SetActive(_spot);
    }

    //MoveAfter = 0, VerticalStretch = 1;
    public void SFXActive(params bool[] act)
    {
        SFX.SetActive(true);
        for (int i = 0; i < act.Length; ++i)
        {
            SFX.transform.GetChild(i).gameObject.SetActive(act[i]);
        }
    }

    void AllOff()
    {
        if (floor.activeSelf == false && UI.activeSelf == false && spot.activeSelf == false) return;
        SFX.SetActive(false);
        UI.SetActive(false);
        floor.SetActive(false);
        spot.SetActive(false);
    }

    public void SetPlayer(Vector3 _position)
    {
        transform.position = _position;
    }

    public void SetPlayer(Vector3 _position, Quaternion _rotation)
    {
        transform.position = _position;
        transform.rotation = _rotation;
    }

    public void SetPlayer(Vector3 _position, Quaternion _rotation, Vector3 _size)
    {
        transform.position = _position;
        transform.rotation = _rotation;
        transform.localScale = _size;
    }

    public void SetPlayerChildPos(Vector3 v, Quaternion q)
    {
        transform.GetChild(0).localPosition = v;
        transform.GetChild(0).localRotation = q;
    }

    public void SetAnimationIdle()
    {
        animator.SetTrigger("Idle");
    }

    public bool CanMove()
    {
        if (Time.timeScale == 1.0f) return true;
        else return false;
    }

    public void Damaged(float DMG)
    {
        GameCtrl.instance.data[(int)Whose.Player].currState[(int)What.Hp] -= DMG;
        animator.SetTrigger("Damaged");
        damagedParticle.Play(true);
    }

}


