using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EProjectileMgr : MonoBehaviour
{
    private const float MaximumRange = 45.0f;
    protected const float Gravity = 9.8f;
    protected const float GroundBound = 0.01f;
    protected Transform Muzz, ProjectileStorage;
    private float damage = 6;
    protected float retainedTime;
    public bool CanMove = true;

    void OnEnable()
    {
        retainedTime = Time.time;
    }

    protected virtual void Update()
    {
        if (Time.time - retainedTime >= 10.0f)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.GetComponent<PlayerCtrl>().Damaged(damage);
            gameObject.SetActive(false);

        }
        else if (col.gameObject.tag == "Obstacle")
        {
            if (col.GetComponent<ObstacleCtrl>().isExist)
            {
                col.GetComponent<ObstacleCtrl>().Damaged(damage, transform.position);
                gameObject.SetActive(false);
            }
        }
    }


    public void SetInit()
    {

    }

    private bool OutofRange()
    {
        if (Vector3.Distance(Muzz.transform.position, transform.position) > MaximumRange)
            return true;
        else return false;
    }

}
