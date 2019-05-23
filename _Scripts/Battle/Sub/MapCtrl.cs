using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCtrl : MonoBehaviour
{
    public GameObject start;
    private static GameObject Portal;
    private GameObject Map;
    private GameObject[] itemList;
    void Awake()
    {
        Map = (GameObject)Instantiate
            (
            Resources.Load("Maps/"
            + GameCtrl.instance.CurrentLevel + "/"
            + "Map_" + GameCtrl.instance.currentMap)
            );
        if (!GameCtrl.instance.IsFirstGameStart) {
            itemList = GameObject.FindGameObjectsWithTag("ItemList");
            for (int i = 0; i < itemList.Length; ++i) itemList[i].SetActive(false);
        }
        Portal = (GameObject)Instantiate(start);
        Portal.transform.position = Map.transform.GetChild(0).position + new Vector3(0,Random.Range(-0.2f,0.2f), Random.Range(-0.2f, 0.2f));
        Portal.SetActive(false);
    }


    public static void CallPortal()
    {
        GameCtrl.instance.currentMap = 0;
        Portal.SetActive(true);
        Portal.GetComponent<Animation>().Play();
        Portal.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0.2f,0 ,0.2f);
    }

    public void PortalOn()
    {
        Portal.SetActive(true);
        Portal.GetComponent<Animation>().Play();
        Portal.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0.2f, 0, 0.2f);
    }
}
