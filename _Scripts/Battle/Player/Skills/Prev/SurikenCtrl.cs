
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurikenCtrl : MonoBehaviour
{
    private float rotateRate;
    private int currRotate;
    private int maxRotate;

    void Start()
    {
        rotateRate = 15.0f;
        currRotate = 0;
        maxRotate = 360;
}

    void Update()
    {
        if (currRotate == 0)
        {
            StartCoroutine(SelfRotate());
        }
    }

    IEnumerator SelfRotate()
    {
        while (currRotate < maxRotate) {
            transform.rotation = Quaternion.Euler(new Vector3(90, currRotate, 0));
            currRotate += (int)rotateRate;
            yield return Time.deltaTime;
        }
        currRotate = 0;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "ENEMY")
        {
            col.GetComponent<Enemy>().LossHp(GameCtrl.instance.Damage());
        }
    }
}
