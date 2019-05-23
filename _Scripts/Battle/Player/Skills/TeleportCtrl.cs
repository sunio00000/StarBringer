using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCtrl : ProjectileMgr
{
    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (Time.time - retainedTime > 0.5f)
        {
            gameObject.SetActive(false);
            Vector3 dest = new Vector3(transform.position.x, 0, transform.position.z);
            PlayerCtrl.instance.SetPlayer(dest, transform.rotation);
        }
    }

    public override int Damage() { return 0; }
}
