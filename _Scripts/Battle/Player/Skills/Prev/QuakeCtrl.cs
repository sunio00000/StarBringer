using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuakeCtrl : MonoBehaviour
{
    public float createDelayTime;
    private float upSpeed = 5.0f;
    private float createTime;


    void Start()
    {
        transform.GetComponent<MeshRenderer>().enabled = false;
        createTime = Time.time;
    }

    void Update()
    {
        if (createDelayTime < Time.time - createTime)
        {
            transform.GetComponent<MeshRenderer>().enabled = true;
            StartCoroutine(ToTheSky());
        }
    }

    IEnumerator ToTheSky()
    {
        while(transform.position.y < 2.0f)
        {
            transform.position += (Time.deltaTime * Vector3.up * upSpeed);
            yield return null;
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "ENEMY")
        {
            col.GetComponent<EnemyCtrl>().monsterHp--;
            Destroy(gameObject);
        }
    }
}
