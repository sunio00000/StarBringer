using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : EnemyMgr
{
    public GameObject AttackObject;
    [SerializeField]
    private List<Collider> cols= new List<Collider>();
    private Transform Body; 

    private void OnValidate()
    {
        //HpImage.localScale = transform.localScale;

        while (Probablity.Count != Drops.Length) { 
            if (Probablity.Count < Drops.Length) Probablity.Add(0);
            else if (Probablity.Count > Drops.Length) Probablity.RemoveAt(Probablity.Count - 1);
        }
    }
    private void OnEnable()
    {
        if (gameObject.GetComponent<CustomOutline>() == null)
            gameObject.AddComponent<CustomOutline>();
        gameObject.GetComponent<CustomOutline>().OutlineColor = Color.red;
        gameObject.GetComponent<CustomOutline>().OutlineWidth = 3.0f;
        gameObject.GetComponent<CustomOutline>().enabled = false;
    }

    public virtual void Awake()
    {
        SetInit(transform.name);
        go = GameObject.FindGameObjectWithTag("Player").transform;
        AttackFunction = DefaultAttack;
        MoveFunction = DefaultMove;
    }

    void Start()
    {
        Body = transform.GetChild(0);
        HpDisplay();
    }

    public virtual void Update()
    {
        if (!IsDead())
        {
            AutoMove();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawWireSphere(transform.position, AgressiveRange);

        Gizmos.color = new Color(1, 1, 0, 0.3f);
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }

    private float HUDRotation()
    {
        // 90 - Camera.main -> body ( x = Camera.main.x) 와 hud -> body 사이의 각도
        // theta = cos^-1((a dot b)/|a|*|b|) real theta = min(theta, 180-theta)
        Vector3 a = new Vector3(0, (transform.position + Vector3.up * transform.GetChild(0).position.y).y - Camera.main.transform.position.y, transform.position.z - Camera.main.transform.position.z)
              , b = Vector3.up * Body.position.y;
        float theta =
            Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(a, b) / (a.magnitude * b.magnitude));
        theta = theta < (180 - theta) ? theta : 180 - theta;
        return theta < 90 ? theta : 90;
    }



    private void AutoMove()
    {
        HUD.LookAt(HUD.position + new Vector3(0, Camera.main.transform.position.y, Camera.main.transform.position.z));
        if (Vector3.Distance(transform.position, go.position) < AgressiveRange)
        {
            if (IsFreeze())
            {
                if (MeetTarget(go))
                {
                    transform.LookAt(go.position);
                    AttackFunction(true);
                }
                else if (MeetFreeze(cols) && !MeetTarget(go))
                {
                    transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
                }
                else if (!MeetFreeze(cols) && !MeetTarget(go))
                {
                    //Move Side
                    transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
                    transform.position += transform.right * Time.deltaTime * 2.0f;
                }
            }
            else
            {
                AttackFunction(false); MoveFunction(true);
                transform.LookAt(go.position);
                if (MeetFreeze(cols) && !MeetTarget(go))
                {
                    transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
                    MoveToPlayer();
                }
                else
                {
                    transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }
            }
        }
        else
        {
            MoveFunction(false);
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    protected override void MoveToPlayer()
    {
        transform.position += transform.forward * Time.deltaTime * MoveSpeed;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "ENEMY") cols.Add(col);
    }

    void OnTriggerExit(Collider col)
    {
        for(int i=0; i<cols.Count; ++i)
        {
            if (cols[i].name == col.name) cols.RemoveAt(i);
        }
    }

    bool OnAggro(Vector3 p)
    {
        if (Vector3.Distance(transform.position, go.position) < AgressiveRange) return true;
        else return false;
    }

    bool MeetTarget(Transform go)
    {
        if(Vector3.Distance(transform.position, go.position) >= AttackRange)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    bool MeetFreeze(List<Collider> cols)
    {
        if (cols.Count == 0) return true;
        foreach (var col in cols)
        {
            if (col.transform.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezeAll)
            {
                return false;
            }
        }
        return true;
    }

    bool IsFreeze()
    {
        if (transform.GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezeAll) return true;
        else return false;
    }

    protected override bool IsDead()
    {
        if(CurrHp <= 0.0f)
        {
            StartCoroutine(Death());
            return true;
        }
        return false;
    }
    public override void AnimAttackFucntion() { }
    protected override IEnumerator Death()
    {
        Trigger.enabled = false;
        transform.GetComponent<Animator>().SetBool("Dead", true);
        yield return new WaitForSeconds(AnimClip[ClipName["Dead"]].length - 0.1f);
        GameCtrl.instance.enemyCount--;
        DropItem(Probablity);
        DropStar();
        Trigger.enabled = true;
        gameObject.SetActive(false);
    }


    public override void LossHp(float damage) {
        CurrHp -= damage;
        damagedParticle.Play(true);
        HUD.GetComponent<EnemyDamagedText>().Damaged(damage);
        HpDisplay();
    }

    public void HpDisplay()
    {
        HpImage.GetChild(1).GetComponent<Image>().fillAmount = HpRatio();
    }

    IEnumerator TextTerm(Text t)
    {
        float f = Time.time;
        float y = t.transform.position.y;
        while (Time.time - f < 1.0f)
        {
            t.transform.position += Vector3.up * Time.deltaTime * 0.3f;
            t.GetComponent<Text>().color -= new Color(0, 0, 0, Time.deltaTime);
            t.GetComponent<Outline>().effectColor -= new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }
        t.enabled = false;
        t.transform.position = new Vector3(t.transform.position.x,y, t.transform.position.z);
        t.GetComponent<Text>().color += new Color32(0, 0, 0, 255);
        t.GetComponent<Outline>().effectColor += new Color32(0, 0, 0, 255);
    }

    // probablity 0.2 0.2  => 20% 20% (60%-> no drop)
    // float array count == drops count
    protected override void DropItem(List<float> probablity) {
        if (probablity == null) return;
        float seed = Random.Range(0.0f, 1.0f); 
        float pro = 0;
        int index = 0;
        foreach(var p in probablity) 
        {
            pro += p;
            if (seed < pro)
            {
                GameObject go = (GameObject)Instantiate(Drops[index]);
                go.name = Drops[index].name;
                go.GetComponent<Item>().DropVector = transform.position;
                Debug.Log(go.transform.position);
                break;
            }
            index++;
        }
    }

    protected override void DropStar()
    {
        // effect
        int p = Random.Range(0, 3);
        GameCtrl.instance.data[(int)Whose.Player].Star[p] += Random.Range(MinStar, MaxStar);

    }
    
    protected override void DefaultAttack(bool flag)
    {
        transform.GetComponent<Animator>().SetBool("Attack", flag);
    }

    protected override void DefaultMove(bool flag)
    {
        transform.GetComponent<Animator>().SetBool("Move", flag);
    }

    // "Attack animation End Time DEFAULT"
    protected override void AttackPlayer()
    {
        if(MeetTarget(go)) PlayerCtrl.instance.Damaged(Power);
    }


    protected override float HpRatio() {
        return CurrHp / MaxHp;
    }
}
