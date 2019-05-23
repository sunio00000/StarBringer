using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalCtrl : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            GameCtrl.instance.currentMap = 0;
            GameCtrl.instance.currentMapType = StageType.NormalStage;
            LoadingScene.LoadScene("Status");
        }
    }
}
