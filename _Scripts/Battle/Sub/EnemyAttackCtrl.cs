using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCtrl : Straight
{
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            GameCtrl.instance.data[(int)Whose.Player].currState[(int)What.Hp]--;
            BattleSceneCtrl.instance.memo.Add("player has damaged.");
            BattleSceneCtrl.instance.LogCycle();
            Destroy(gameObject.transform.parent.gameObject);
        }
        else if(col.gameObject.tag == "PILL")
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
