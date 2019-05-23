using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListSpawn : MonoBehaviour
{
    public GameObject something;

    void Update()
    {
        if(something.transform.parent.CompareTag("Shield") || something.transform.parent.CompareTag("Sword"))
        {
            something.transform.parent.position = Vector3.zero;
        }
        else something.transform.position = Vector3.zero;
    }
}
