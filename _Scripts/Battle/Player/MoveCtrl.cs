using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCtrl : MonoBehaviour
{

    const float deltaMoveSpeed = 1f; //5
    const float deltaRotate = 1.5f;
    const float Accel = 4.0f;
    private float prevFoward, fowardMoveSpeed;
    private float prevLeft, leftMoveSpeed;
    private Rigidbody rigid;
    private float prevAngle;


    void Start()
    {
        fowardMoveSpeed = 0f;
        leftMoveSpeed = 0f;
        prevAngle = 0f;
        rigid = GetComponent<Rigidbody>();
        prevFoward = fowardMoveSpeed;
        prevLeft = leftMoveSpeed;
    }

    void LateUpdate()
    {
        if (GameCtrl.instance.IsBattle() && PlayerCtrl.instance.CanMove()) MovePlayer();
        else
        {
            fowardMoveSpeed = 0; leftMoveSpeed = 0;
            StopAllCoroutines();
        }
    }

    void MovePlayer()
    {
        if (Input.GetKey(KeyCode.W))
        {
            leftMoveSpeed = deltaMoveSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                leftMoveSpeed += Accel;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            leftMoveSpeed = -deltaMoveSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                leftMoveSpeed -= Accel;
            }
        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            leftMoveSpeed = 0;
        }
        if (Input.GetKey(KeyCode.A))
        {
            fowardMoveSpeed = -deltaMoveSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                fowardMoveSpeed -= Accel;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            fowardMoveSpeed = deltaMoveSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                fowardMoveSpeed += Accel;
            }
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            fowardMoveSpeed = 0;
        }
        transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(fowardMoveSpeed, 0, leftMoveSpeed), Time.deltaTime);
        if (fowardMoveSpeed != 0 || leftMoveSpeed != 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, GetAngle(transform.position, transform.position + new Vector3(leftMoveSpeed, 0, fowardMoveSpeed)), 0));
            PlayerCtrl.instance.animator.SetTrigger("Move");
            PlayerCtrl.instance.SFXActive(true, false);
        }
        else
        {
            PlayerCtrl.instance.animator.SetTrigger("Idle");
            PlayerCtrl.instance.SFXActive(false, false);
        }
    }

    private float GetAngle(Vector3 curr, Vector3 target)
    {
        Vector3 v = target - curr;
        return Mathf.Atan2(v.z, v.x) * Mathf.Rad2Deg < 0 ?
            Mathf.Atan2(v.z, v.x) * Mathf.Rad2Deg + 540 : Mathf.Atan2(v.z, v.x) * Mathf.Rad2Deg + 180;
    }
}
