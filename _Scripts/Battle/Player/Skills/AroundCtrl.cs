using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AroundCtrl : ProjectileMgr
{
    private float selfRotateRate = 15.0f;
    private int selfCurrRotate = 0;
    private int selfMaxRotate = 360;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(surikenRotate(gameObject));
    }

    protected override void Update()
    {
        base.Update();
    }

    IEnumerator surikenRotate(GameObject suriken)
    {
        Vector3 pos = Muzz.transform.position;
        float maxRotate = 360, rotRate = 200f;
        for (float currRotate = 0; currRotate < maxRotate; currRotate += Time.deltaTime * rotRate)
        {
            if (suriken == null)
                break;
            suriken.transform.RotateAround(pos, Vector3.up, Time.deltaTime * rotRate);
            yield return Time.deltaTime;
        }
        Destroy(suriken);
    }

    IEnumerator SelfRotate()
    {
        while (selfCurrRotate < selfMaxRotate)
        {
            transform.rotation = Quaternion.Euler(new Vector3(90, selfCurrRotate, 0));
            selfCurrRotate += (int)selfRotateRate;
            yield return Time.deltaTime;
        }
        selfCurrRotate = 0;
    }

    public override int Damage() { return 0; }
}
