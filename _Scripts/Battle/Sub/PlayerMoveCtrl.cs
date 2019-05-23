using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCtrl : MonoBehaviour
{
    private const float deltaMoveSpeed = 1.0f;

    public VariableJoystick moveJoy;
    public AttackableJoystick attackJoy;
    public GameObject Player;
    public GameObject Target;

    void Start()
    {
    }

    void LateUpdate()
    {
        if(Player==null) Player = GameObject.FindWithTag("Player");
        if(Target==null) Target = GameObject.FindWithTag("TargetArea");

        if (GameCtrl.instance.IsBattle() && PlayerCtrl.instance.CanMove())
        {
            PlayerControl();
            MoveArea();
        }
        else StopAllCoroutines();
    }

    private void PlayerControl()
    {
        Player.transform.position = Vector3.Lerp(Player.transform.position,
            Player.transform.position + deltaMoveSpeed * new Vector3(moveJoy.Direction.x, 0, moveJoy.Direction.y), Time.deltaTime);
        Player.transform.LookAt(Player.transform.position + new Vector3(-moveJoy.Direction.x, 0, -moveJoy.Direction.y));
        if (moveJoy.Direction.x != 0 || moveJoy.Direction.y != 0) PlayerCtrl.instance.animator.SetTrigger("Move");
    }

    private void MoveArea()
    {
        Target.transform.position = Vector3.Lerp(Target.transform.position,
            Target.transform.position + deltaMoveSpeed * new Vector3(attackJoy.Direction.x, 0, attackJoy.Direction.y), Time.deltaTime);
        //transform.LookAt(transform.position + new Vector3(moveJoy.Direction.x, 0, moveJoy.Direction.y));
    }

    private float GetAngle(Vector3 curr, Vector3 target)
    {
        Vector3 v = target - curr;
        return Mathf.Atan2(v.z, v.x) * Mathf.Rad2Deg;
    }
}
