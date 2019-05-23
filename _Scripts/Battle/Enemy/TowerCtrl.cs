using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCtrl : MonoBehaviour
{
    public GameObject Weapon;
    public float attackRate, attackInterval;
    public int attackCount;
    private Transform Muzz;
    private GameObject Storage;
    [HideInInspector]
    public bool CanShot = true; // event

    void Awake()
    {
        Muzz = transform.GetChild(0);
        Muzz.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
    }

    void Start()
    {
        StartCoroutine(Shot(attackRate, attackInterval, attackCount));
    }

    private IEnumerator Shot(float rate, float interval, int count)
    {
        while (CanShot) {
            for (int i = 0; i < count; ++i)
            {
                AttackCtrl.PoolCreateAndManage(Weapon,Muzz,transform.name);
                yield return new WaitForSeconds(rate);
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
