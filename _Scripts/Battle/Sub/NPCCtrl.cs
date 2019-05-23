using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCtrl : MonoBehaviour
{
    public GameObject talk;

    private void OnTriggerEnter(Collider col)
    {
        BattleSceneCtrl.instance.CurrentIO.name = gameObject.name;
        if(talk != null)
            talk.SetActive(true);
    }

    private void OnTriggerExit(Collider col)
    {
        BattleSceneCtrl.instance.CurrentIO.name = "";
        BattleSceneCtrl.instance.InteractionOFF();
        if (talk != null)
            talk.SetActive(false);

    }
}
