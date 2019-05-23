using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightCtrl : ProjectileMgr
{
    public float Velocity = 2f;
    protected override void Update()
    {
        if (CanMove)
            transform.Translate(Time.deltaTime * Vector3.forward * Velocity);
    }

    public override int Damage()
    {
        return 0;
    }
}
