using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorCtrl : ProjectileMgr
{
    public Vector3 dest;
    private Vector3 arrow;
    private float droptime;
    protected override void Start()
    {
        base.Start();
        droptime = Random.Range(0, 0.5f);
        transform.position = dest + new Vector3(Random.Range(-0.3f, 0.3f) - 0.5f, 2.0f, Random.Range(-0.3f, 0.3f) - 0.5f);
        arrow = new Vector3(0.5f, -2.0f, 0.5f);
        //arrow = dest - transform.position;
    }

    protected override void Update()
    {
        base.Update();
        if (transform.position.y < 0.0f) gameObject.SetActive(false);
        if(droptime < Time.time - retainedTime)
            transform.position += Time.deltaTime * arrow;
    }

    public override int Damage()
    {
        return 0;
    }
}
