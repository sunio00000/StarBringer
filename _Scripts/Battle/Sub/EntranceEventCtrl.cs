using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceEventCtrl : MonoBehaviour
{
    public GameObject MapCollider;
    public GameObject EntranceTiles;
    public GameObject Entrance;

    public static bool StartOn = false;

    void OnTriggerExit(Collider col)
    {
        if(col.tag == "Player")
        {
            PlayEvent();
        }
    }

    void PlayEvent()
    {
        MapCollider.SetActive(true);
        Entrance.GetComponent<Animation>().Play();
        EntranceTiles.GetComponent<Animation>().Play();
        gameObject.SetActive(false);
    }
}
