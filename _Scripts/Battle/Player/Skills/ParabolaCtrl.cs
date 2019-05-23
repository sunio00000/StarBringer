using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ProjectileMgr를 상속받은 스크립트가 투사체를 컨트롤한다.
//Trigger판정 혹은 Active상태 등은 부모 클래스에서 관리한다.
public class ParabolaCtrl : ProjectileMgr
{
    private float zVelocity = 2.5f , yVelocity = 3f;

    protected override void Start()
    {
        base.Start();
    }

    //움직임 및 효과는 Update 이벤트 함수나 Coroutine을 이용해 제어한다.
    protected override void Update()
    {
        base.Update();
        if (CanMove)
        {
            transform.position += 
                (new Vector3(0, (yVelocity - Gravity * (Time.time - retainedTime)) * Time.deltaTime, 0)
                + transform.forward * zVelocity * Time.deltaTime);
            if (transform.position.y <= GroundBound) CanMove = false;
        }
    }

    //오버라이드 된 추상메소드
    public override int Damage()
    {
        return (int)GameCtrl.instance.data[(int)Whose.Player].currState[(int)What.Power];
    }
}
