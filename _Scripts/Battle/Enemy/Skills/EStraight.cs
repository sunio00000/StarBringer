using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EStraight : EProjectileMgr
{
    public float Velocity = 5f;
    protected override void Update()
    {
        base.Update();
        if (CanMove)
            transform.Translate(Time.deltaTime * Vector3.forward * Velocity);
    }
}
