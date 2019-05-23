using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTest : MonoBehaviour
{
    public GameObject Fireball;
    private Transform child;

    void Start()
    {
        child = transform.GetChild(0);
        child.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        StartCoroutine(Shot());
    }

    private IEnumerator Shot()
    {
        while (true)
        {
            Instantiate(Fireball,child.position,child.rotation);
            yield return new WaitForSeconds(2.0f);
        }
    }
}
