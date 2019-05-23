using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : Straight
{

    protected override void Update()
    {
        base.Update();
        if (!transform.GetChild(2).gameObject.GetComponent<ParticleSystem>().isPlaying
            && !transform.GetChild(0).gameObject.activeSelf)
            gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "ENEMY")
        {
            canMove = false;
            col.GetComponent<Enemy>().LossHp(GameCtrl.instance.Damage());
            if (transform.GetChild(2).gameObject.activeSelf == false)
            {
                transform.GetComponent<SphereCollider>().enabled = false;
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else if (col.gameObject.tag == "PILL")
        {
            Destroy(gameObject);
        }
    }
}
