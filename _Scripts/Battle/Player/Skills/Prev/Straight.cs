using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straight : MonoBehaviour
{
    private float speed = 1.0f;
    protected bool canMove = true;

    protected virtual void Update()
    {
        if(canMove) transform.Translate(Vector3.forward * speed * Time.deltaTime * 5.0f);
    }
}
