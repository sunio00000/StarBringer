using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCtrl : AttackMgr
{
    public Transform Player;
    public Transform TargetArea;
    public static AttackCtrl instance;
    public float rot, CoolTime;
    const float BulletVelocityInit = 25.0f;
    const float BombVelocityInit = 4.0f;
    const float ShotRateBound = 10.0f;
    const float Sensitivity = 2f;

    private Vector3 currPosition;
    private float bombVelocity;
    private float bulletRate;
    private float bulletRateCurr;
    private int bombNum;

    public override void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        base.Awake();
    }

    //
    void Update()
    {
        currPosition = body.transform.position;
        bombNum = (int)GameCtrl.instance.data[(int)Whose.Player].currState[(int)What.BombNum];
        bulletRate = GameCtrl.instance.data[(int)Whose.Player].currState[(int)What.BulletRate];
        if(GameCtrl.instance.IsBattle() && PlayerCtrl.instance.CanMove()){
            CoolTime = 0;
            if (true)//Input.GetMouseButton(0) && !ButtonCtrl.OnBtn)
            {
                CoolTime = CurrSkillCreation();
            }
            PlayerCtrl.instance.animator.SetTrigger("Idle");
        }
    }

    // static 메소드로 여러 오브젝트에서 이용 가능하다.
    // 매개변수 (대상이 될 오브젝트, 대상의 트랜스폼, 임의의 이름)
    static public void PoolCreateAndManage(GameObject Object, Transform Muzz, string n)
    {
        // 재활용 가능한 오브젝트가 있다면 이용한다.
        if (Pooling(n + "_weapon", Muzz)) { return; }
        // 아니라면 오브젝트를 생성하고 이니셜라이즈 한다.
        else
        {
            GameObject Weapon = Instantiate(Object, Muzz.position, Muzz.rotation);
            Weapon.name = n + "_weapon";
            GameObject Basket = GameObject.Find(Weapon.name + "_Pool");
            // 담을 Pool이 없다면 새로 생성한다.
            if (Basket == null) Basket = new GameObject(Weapon.name + "_Pool");
            Weapon.transform.parent = Basket.transform;
        }
    }
    static public bool Pooling(string Weapon, Transform Muzz)
    {
        GameObject Basket = GameObject.Find(Weapon + "_Pool");
        if (Basket == null) return false;
        // 풀이 존재한다면 리스트를 확인한다.
        for (int i = 0; i < Basket.transform.childCount; ++i) {
            // 대상 오브젝트가 있다면,
            GameObject w = Basket.transform.GetChild(i).gameObject;
            // 오브젝트가 이용 중이면 다음 것을 확인하고,
            if (w.activeSelf) continue;
            // 사용 가능한 오브젝트가 있다면, 그 것을 이용한다.
            else
            {
                w.transform.position = Muzz.position;
                w.transform.rotation = Muzz.rotation;
                w.SetActive(true);
                return true;
            }
        }
        return false;
    }

    // 델리게이트에 연결한 투사체 메소드(Default)
    public float ShotDefault(Transform t)
    {
        bulletRateCurr += bulletRate * Time.deltaTime;
        if (bulletRateCurr >= ShotRateBound && !AttackableJoystick.ReadyToAttack)
        {
            PlayerCtrl.instance.animator.SetTrigger("Attack");
            GameObject go = (GameObject)Instantiate(SkillObject["?"]);
            go.AddComponent(CurrSkillSystem());
            // Start Logic
            // .
            // .
            // ...
            // End Logic
            AttackableJoystick.ReadyToAttack = true;
            bulletRateCurr = 0;
        }
        return ShotCurrRate <= ShotRateBound ? ShotCurrRate / ShotRateBound : 1;
    }

    public float MoveFace(Transform t)
    {
        ShotCycleRate = 20.0f;
        ShotCurrRate += ShotCycleRate * Time.deltaTime;
        if (ShotCurrRate >= ShotRateBound && !AttackableJoystick.ReadyToAttack)
        {
            PlayerCtrl.instance.animator.SetTrigger("Attack");
            GameObject go = (GameObject)Instantiate(SkillObject["Teleport"]);
            go.AddComponent(CurrSkillSystem());
            go.transform.localPosition = t.position;
            go.transform.localRotation = Quaternion.Euler(t.eulerAngles);
            go.transform.localPosition += t.forward * 2.0f;
            ShotCurrRate = 0;
        }
        return ShotCurrRate <= ShotRateBound ? ShotCurrRate / ShotRateBound : 1;
    }

    public float ShotFace(Transform t)
    {
        ShotCycleRate = 10.0f;
        ShotCurrRate += ShotCycleRate * Time.deltaTime;
      
        if (ShotCurrRate >= ShotRateBound && !AttackableJoystick.ReadyToAttack)
        {
            GameObject go = (GameObject)Instantiate(SkillObject["FireBall"]);
            go.AddComponent(CurrSkillSystem());
            //Player.rotation = Quaternion.Euler(0, -AttackableJoystick.AttackAngle, 0);
            go.transform.localPosition = t.position;
            go.transform.localRotation = t.rotation;
            //go.transform.localRotation = Quaternion.Euler(0,AttackableJoystick.AttackAngle,0);
            PlayerCtrl.instance.animator.SetTrigger("Attack");
            ShotCurrRate = 0;
            AttackableJoystick.ReadyToAttack = true;
           }
        return ShotCurrRate <= ShotRateBound ? ShotCurrRate / ShotRateBound : 1;
    }

    public float ShotSector(Transform t)
    {
        ShotCycleRate = 10.0f;
        ShotCurrRate += ShotCycleRate * Time.deltaTime;
        if (ShotCurrRate >= ShotRateBound && !AttackableJoystick.ReadyToAttack)
        {
            PlayerCtrl.instance.animator.SetTrigger("Attack");
            GameObject[] bomb = new GameObject[1 + GameCtrl.instance.GetUseItem().GetRGB("Yellow")];
            int deg, degCount;
            if (bomb.Length == 1)
            {
                deg = 0;
                degCount = 0;
            }
            else
            {
                deg = 90 / (bomb.Length - 2 + 1);
                degCount = -45;
            }
            for (int i = 0; i < bomb.Length; ++i)
            {
                bomb[i] = (GameObject)Instantiate(SkillObject["FireBall"]);
                bomb[i].AddComponent(CurrSkillSystem());
                bomb[i].transform.localPosition = t.position;
                bomb[i].GetComponent<Transform>().rotation =
                    Quaternion.Euler(transform.rotation.eulerAngles + Vector3.up * degCount);
                degCount += deg;
            }

            ShotCurrRate = 0;
            AttackableJoystick.ReadyToAttack = true;
        }
        return ShotCurrRate <= ShotRateBound ? ShotCurrRate / ShotRateBound : 1;
    }

    //for enemy
    public float ShotSector(GameObject go, int number, Transform t)
    {
        GameObject[] bomb = new GameObject[number];
        int deg, degCount;
        if (bomb.Length == 1)
        {
            deg = 0;
            degCount = 0;
        }
        else
        {
            deg = 90 / (bomb.Length - 2 + 1);
            degCount = -45;
        }
        for (int i = 0; i < bomb.Length; ++i)
        {
            bomb[i] = (GameObject)Instantiate(go);
            bomb[i].transform.localPosition = t.position+ Vector3.up*0.05f;
            bomb[i].GetComponent<Transform>().rotation =
                Quaternion.Euler(t.rotation.eulerAngles + Vector3.up * degCount);
            degCount += deg;
        }
        return 0;
    }
    public float AroundSuriken(Transform t)
    {
        bulletRateCurr += bulletRate * Time.deltaTime;
        if (bulletRateCurr >= ShotRateBound && !AttackableJoystick.ReadyToAttack)
        {
            PlayerCtrl.instance.animator.SetTrigger("Attack");
            GameObject[] surikens = new GameObject[3 + GameCtrl.instance.GetUseItem().GetRGB("Yellow")];
            int deg, degCount; float padding = 1.0f;
            if (surikens.Length == 1)
            {
                deg = 0;
                degCount = 0;
            }
            else
            {
                deg = 360 / (surikens.Length);
                degCount = 0;
            }
            for (int i = 0; i < surikens.Length; ++i)
            {
                surikens[i] = (GameObject)Instantiate(SkillObject["Orb"]);
                surikens[i].AddComponent(CurrSkillSystem());
                surikens[i].transform.position = transform.parent.position + new Vector3(padding * Mathf.Cos(Mathf.Deg2Rad * degCount), 0.1f, padding * Mathf.Sin(Mathf.Deg2Rad * degCount));
                degCount += deg;
            }
            bulletRateCurr = 0;
            AttackableJoystick.ReadyToAttack = true;
        }
        return bulletRateCurr <= ShotRateBound ? bulletRateCurr / ShotRateBound : 1;
    }

    public float AroundSuriken(GameObject go, int number, Transform t)
    {
        GameObject[] surikens = new GameObject[number];
        int deg, degCount; float padding = 1.0f;
        if (surikens.Length == 1)
        {
            deg = 0;
            degCount = 0;
        }
        else
        {
            deg = 360 / (surikens.Length);
            degCount = 0;
        }
        for (int i = 0; i < surikens.Length; ++i)
        {
            surikens[i] = (GameObject)Instantiate(go);
            surikens[i].transform.position = transform.parent.position + new Vector3(padding * Mathf.Cos(Mathf.Deg2Rad * degCount), 0.1f, padding * Mathf.Sin(Mathf.Deg2Rad * degCount));
            degCount += deg;
        }
        return 0;
    }

    public float ShotMeteor(Transform t)
    {
        bulletRateCurr += bulletRate * Time.deltaTime;

        if (bulletRateCurr >= ShotRateBound && !AttackableJoystick.ReadyToAttack)
        {
            Vector3 targetPos = TargetArea.GetComponent<AttackAreaCtrl>().area;
            PlayerCtrl.instance.animator.SetTrigger("Attack");
            GameObject[] meteors = new GameObject[5 + GameCtrl.instance.GetUseItem().GetRGB("Yellow")];
            for (int i = 0; i < meteors.Length; ++i)
            {
                meteors[i] = (GameObject)Instantiate(SkillObject["Meteor"]);
                meteors[i].AddComponent(CurrSkillSystem());
                meteors[i].GetComponent<MeteorCtrl>().dest = targetPos;
            }
            bulletRateCurr = 0;
            AttackableJoystick.ReadyToAttack = true;
            TargetArea.GetComponent<AttackAreaCtrl>().SetOrigin();
        }
        return bulletRateCurr <= ShotRateBound ? bulletRateCurr / ShotRateBound : 1;
    }

    public float ShotHeal(Transform t)
    {
        bulletRateCurr += bulletRate * Time.deltaTime;
        if (bulletRateCurr >= ShotRateBound && !AttackableJoystick.ReadyToAttack)
        {
            GameObject heal = (GameObject)Instantiate(SkillObject["Heal"]);
            heal.transform.position = transform.position + Vector3.up * 0.2f;
            GameCtrl.instance.playerData().currState[(int)What.Hp] += 5;
            bulletRateCurr = 0;
            AttackableJoystick.ReadyToAttack = true;
        }
        return bulletRateCurr <= ShotRateBound ? bulletRateCurr / ShotRateBound : 1;
    }

    public float ShotQuake()
    {
        PlayerCtrl.instance.animator.SetTrigger("Attack");
        const int LEFT=0, RIGHT=1, FORWARD=2, BACK=3;
        const float PAD = 1.0f;
        bulletRateCurr += bulletRate * Time.deltaTime;
        if (bulletRateCurr >= ShotRateBound)
        {
            GameObject[,] Quakes = new GameObject[5,4];
            for (int i = 0; i < Quakes.Length/4; ++i)
            {
                for (int j = 0; j < Quakes.Length/5; ++j)
                {
                    Quakes[i, j] = (GameObject)Instantiate(SkillObject["FireBall"]);
                    Quakes[i, j].GetComponent<QuakeCtrl>().createDelayTime = i * 0.1f;
                    Quakes[i, j].transform.position = body.transform.position- Vector3.up * PAD;
                    if (j == LEFT)
                    {
                        Quakes[i, j].transform.position += Vector3.left * PAD * (i + 1);
                    }
                    else if(j == RIGHT)
                    {
                        Quakes[i, j].transform.position += Vector3.right * PAD * (i + 1);
                    }
                    else if(j == FORWARD)
                    {
                        Quakes[i, j].transform.position += Vector3.forward * PAD * (i + 1);
                    }
                    else if(j == BACK)
                    {
                        Quakes[i, j].transform.position += Vector3.back * PAD * (i + 1);
                    }
                }
            }
            bulletRateCurr = 0;
        }
        return bulletRateCurr <= ShotRateBound ? bulletRateCurr / ShotRateBound : 1;
    }

    public float ShotScrewLight()
    {
        PlayerCtrl.instance.animator.SetTrigger("Attack");
        bulletRateCurr += bulletRate * Time.deltaTime;
        if (bulletRateCurr >= ShotRateBound)
        {
            GameObject[] light = new GameObject[2];
            foreach(var l in light)
            {
                //l = (GameObject)Instantiate();
            }
            bulletRateCurr = 0;
        }
        return bulletRateCurr <= ShotRateBound ? bulletRateCurr / ShotRateBound : 1;
    }
}
