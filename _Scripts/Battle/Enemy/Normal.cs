using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal : Enemy
{
    void Start()
    {
    }

    public override void AnimAttackFucntion()
    {
        if (AttackObject != null)
        {
            AttackCtrl.instance.ShotSector(AttackObject,3, transform);
        }
    }
}
