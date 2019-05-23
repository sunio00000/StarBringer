using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCtrl : MonoBehaviour
{
    
    public int monsterHp;
    public GameObject HPbar;
    public Image fillHPbar;
    public Transform target;
    public GameObject seed;
    public GameObject healthUp;
    public GameObject[] star = new GameObject[(int)StarColor.NumberOfType];

    private int monsterHpMax;
    private float moveSpeed;
    private float enemyToPlayer;
    private float seedRate;
    private float seedinit;
    

    void Start()
    {
        monsterHpMax = 3;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        moveSpeed = 3.0f;
        enemyToPlayer = 25.0f;
        seedRate = 5;
        seedinit = 2;
        monsterHp = 3;
    }

    void Update()
    {
        MonsterStatus();
        MoveToPlayerForShot();
    }


    void ShotSeed()
    {
        seedinit += Time.deltaTime;
        if (seedRate < seedinit)
        {
            GameObject go = (GameObject)Instantiate(seed);
            go.transform.position = transform.position;
            go.transform.rotation = transform.rotation;
            seedinit = 0;
        }
    }

    void MoveToPlayerForShot()
    {
        // 벽 , 다른 적 을 피해 움직이게 한다.
        transform.LookAt(target);
        if (Vector3.Distance(transform.position, target.position) > enemyToPlayer)
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        else ShotSeed();
    }

    void MonsterStatus()
    {
        HPbar.transform.LookAt(Camera.main.transform);
        HPbar.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2((float)monsterHp / monsterHpMax,0.2f);
        if (monsterHp <= 0)
        {
            // 확률
            int possible = Random.Range(0, 10); 
            GameCtrl.instance.playerCost++;
            GameCtrl.instance.enemyCount--;
            GameObject go = (GameObject)Instantiate(healthUp);
            go.transform.position = gameObject.transform.position;
            if (possible > 6)
            {
                int type = Random.Range(0, 3);
                GameObject s = (GameObject)Instantiate(star[type]);
                s.transform.position = gameObject.transform.position;
            }
            Destroy(gameObject);
        }
    }
}
