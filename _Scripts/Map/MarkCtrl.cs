using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkCtrl : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        while (true)
        {
            transform.Rotate(0,Time.deltaTime*100f, 0);
            yield return Time.deltaTime;
        }
    }
}
