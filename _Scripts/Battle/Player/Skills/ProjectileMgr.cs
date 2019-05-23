using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
abstract public class ProjectileMgr : MonoBehaviour
{
    private const float MaximumRange = 45.0f;
    protected const float Gravity = 9.8f;
    protected const float GroundBound = 0.01f;

    protected bool CanMove = true;
    protected Transform Muzz, ProjectileStorage;
    protected float retainedTime;

    protected virtual void Start()
    {
        retainedTime = Time.time;
        Muzz = GameObject.FindGameObjectWithTag("Muzz").transform;
        ProjectileStorage = GameObject.FindGameObjectWithTag("Storage").transform;
        transform.SetParent(ProjectileStorage);
    }

    protected virtual void Update()
    {
        if (Time.time - retainedTime >= 5.0f || OutofRange())
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("ENEMY") || col.CompareTag("Boss"))
        {
            gameObject.SetActive(false);
            col.GetComponent<Enemy>().LossHp(GameCtrl.instance.Damage());
            col.GetComponent<Animator>().SetTrigger("Damaged");
        }
        else if (col.CompareTag("PILL"))
        {
            gameObject.SetActive(false);
        }
        else if(col.CompareTag("Obstacle"))
        {
            if (col.GetComponent<ObstacleCtrl>().isExist)
            {
                col.GetComponent<ObstacleCtrl>().Damaged(GameCtrl.instance.data[(int)Whose.Player].currState[(int)What.BulletDmg], transform.position);
                gameObject.SetActive(false);
            }
        }
    }

    private bool OutofRange()
    {
        if (Vector3.Distance(Muzz.transform.position, transform.position) > MaximumRange)
            return true;
        else return false;
    }

    // Need Equation related with Player's Power, Skill's Abillity
    abstract public int Damage();
}
