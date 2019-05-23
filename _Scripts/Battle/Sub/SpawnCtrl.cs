using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCtrl : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white - new Color(0,0,0,0.5f);
        Gizmos.DrawSphere(transform.position,0.5f);
    }
}
