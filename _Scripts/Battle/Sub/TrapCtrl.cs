using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCtrl : MonoBehaviour
{
    public int damage;
    private Dictionary<string, float> stayTimeAsName = new Dictionary<string, float>();
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<PlayerCtrl>().Damaged(damage);
        }
        else if (col.CompareTag("ENEMY"))
        {
            col.GetComponent<Enemy>().LossHp(damage);
        }
        stayTimeAsName[col.name] = Time.time;
    }
    private void OnTriggerStay(Collider col)
    {
        if (Time.time - stayTimeAsName[col.name] > 1.0f)
        {
            if (col.CompareTag("Player"))
            {
                col.GetComponent<PlayerCtrl>().Damaged(damage);
            }
            else if (col.CompareTag("ENEMY"))
            {
                col.GetComponent<Enemy>().LossHp(damage);
            }
            stayTimeAsName[col.name] = Time.time;
        }
    }
}
