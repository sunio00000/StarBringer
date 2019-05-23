using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudAnimation : MonoBehaviour
{
    private float MaxZ = -230.0f;
    private float MinZ = 50.0f;

    void LateUpdate()
    {
        transform.position -= transform.forward *Time.deltaTime * 3.0f;
        if (transform.localPosition.z <= MaxZ) transform.localPosition = new Vector3(transform.localPosition.x, Random.Range(80,150), MinZ);
    }
}
