using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//player ray, mouse fellow, AnimatorCtrl
public class BodyCtrl : MonoBehaviour
{
    public TrailRenderer Weapon;
    private Collider Boss;
    private RaycastHit hit;
    private Vector3 rayToFoward;

    void Update()
    {
        if (GameCtrl.instance.IsBattle() && PlayerCtrl.instance.CanMove())
        {
            //Debug.DrawRay(transform.parent.position + transform.up * 0.1f, -transform.parent.forward * 3f + transform.parent.up * 0.1f, Color.white);
            if (Physics.Raycast(transform.parent.position + transform.up * 0.1f, -transform.parent.forward, out hit, 15.0f,1<<LayerMask.NameToLayer("Enemy")))
            {
                BattleSceneCtrl.instance.EnemyDisplay(hit.collider);
                Boss = hit.collider;
                if (!BattleSceneCtrl.instance.EnemyState.activeSelf)
                    BattleSceneCtrl.instance.EnemyStateActiveTime = Time.time;
            }
            if(GameCtrl.instance.currentMapType == StageType.BossStage)
               BattleSceneCtrl.instance.EnemyDisplay(Boss);
        }
        else if (GameCtrl.instance.IsStatus()) transform.localRotation = Quaternion.Euler(new Vector3(0, -90, 0));
    }

    public void AttackTrail(int state)
    {
        if (state == 1) Weapon.enabled = true;
        else if(state == 0) Weapon.enabled = false;
    }
}
    