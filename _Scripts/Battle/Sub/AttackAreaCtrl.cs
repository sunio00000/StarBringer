using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaCtrl : MonoBehaviour
{
    public Vector3 area;

    private void OnEnable()
    {
        transform.localPosition = new Vector3(0, 0.1f, 0);
    }

    public  void SetOrigin()
    {
        transform.localPosition = new Vector3(0, 0.1f, 0);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
