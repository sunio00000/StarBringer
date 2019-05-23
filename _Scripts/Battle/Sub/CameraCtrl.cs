using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraCtrl : MonoBehaviour
{
    private float rot;
    public static GameObject target;
    private Vector3 diff;
    Vector3 pos;

    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("StartTile");
            pos = Vector3.zero;
        }

        if (SceneManager.GetActiveScene().name == "Battle")
        {
            //transform.position = target.transform.localPosition + new Vector3(0, 2.5f,-2);
            //transform.rotation = Quaternion.Euler(new Vector3(55, 0, 0));
            //diff = target.transform.position - transform.position;
        }
        else if (SceneManager.GetActiveScene().name == "Map")
        {
            transform.position = target.transform.position + new Vector3(0, -2, -4);
            transform.rotation = Quaternion.Euler(new Vector3(-30, 0, 0));
            diff = target.transform.position - transform.position;
        }
    }


    void LateUpdate()
    {
        if (target== null || target.tag != "Player")
        {
            target = GameObject.FindGameObjectWithTag("Player");
            
        }
        if (SceneManager.GetActiveScene().name == "Battle") transform.position = Vector3.SmoothDamp(transform.position, target.transform.position - diff, ref pos, Time.smoothDeltaTime * 3f);
        else transform.position = Vector3.SmoothDamp(transform.position, target.transform.position - diff, ref pos, Time.smoothDeltaTime * 0.3f);

    }

    public void SetSceneInit(Vector3 pos, Vector3 rot)
    {
        if (target == null) target = GameObject.FindGameObjectWithTag("Player");
        transform.position = target.transform.localPosition + pos;
        transform.rotation = Quaternion.Euler(rot);
        diff = target.transform.position - transform.position;
    }
}
