using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCtrl : MonoBehaviour
{
    // parabola 
    const float initXPos = -9.0f;
    const float groundBound = 0.1f;
    const float Gravity = 15.0f;

    public GameObject muzzle;
    public float velocityX;
    public float velocityY;
    private bool canFly;
    private float t;

    void Start()
    {
        canFly = true;
        transform.position = muzzle.transform.position;
        t = Time.time;
    }

    void Update()
    {
        if (canFly)
        {
            // 던져서 터지는 방식
            if((velocityY - Gravity * (Time.time - t)) * Time.deltaTime < groundBound)
                transform.localPosition = transform.localPosition + new Vector3(0, (velocityY - Gravity * (Time.time - t)) * Time.deltaTime, velocityX * Time.deltaTime);
            else transform.localPosition = transform.localPosition + new Vector3(0, (velocityY - Gravity * (Time.time - t)) * Time.deltaTime, velocityX * Time.deltaTime);
        }
        if (transform.position.x != initXPos && transform.position.y <= groundBound)
        {
            canFly = false;
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "ENEMY")
        {
            col.gameObject.GetComponent<EnemyCtrl>().monsterHp--;
        }
        else if(col.gameObject.tag == "PILL")
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
