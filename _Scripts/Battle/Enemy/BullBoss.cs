using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullBoss : Boss
{
    public GameObject Attack1;
    public GameObject Attack2;
    private int tmp = 0;
    // Run Move
    // Attack Attack1 Attack2
    public override void Awake()
    {
        base.Awake();
        pattern.Add(new Pattern(MoveSpeed, Power, AttackRange));
        pattern.Add(new Pattern(3, 3, 3));
        pattern.Add(new Pattern(3, 7, 4));
        attacks.Add(DefaultAttack);
        attacks.Add(OneAttack);
        attacks.Add(TwoAttack);
        moves.Add(DefaultMove);
        moves.Add(OneMove);
        moves.Add(OneMove);
    }
    public override void Update()
    {
        base.Update();
        CheckQuarter();
    }

    private void CheckQuarter()
    {
        for (int i=1; i<Quarter-1; ++i)
        {
            if (CurrHp <= (i / (float)Quarter) * MaxHp)
            {
                if (tmp != i)
                {
                    int state = Quarter - 1 - i;
                    AttackFunction(false); MoveFunction(false);
                    Debug.Log(state + " 단계 그로기");
                    SetUpAbility(pattern[state]);
                    AttackFunction = attacks[state];
                    MoveFunction = moves[state];
                    tmp = i;
                }
                break;
            }
        }
    }

    private void OneAttack(bool flag)
    {
        transform.GetComponent<Animator>().SetBool("Attack1", flag);
    }

    private void TwoAttack(bool flag)
    {
        transform.GetComponent<Animator>().SetBool("Attack2", flag);
    }

    private void OneMove(bool flag)
    {
        transform.GetComponent<Animator>().SetBool("Run", flag);
    }

    public void MakeOneAttack()
    {
        GameObject go = Instantiate(Attack1);
        go.transform.position = transform.position + Vector3.up * 2.0f;
        go.transform.rotation = Quaternion.Euler(0,transform.eulerAngles.y,0);
    }

    public void MakeTwoAttack()
    {
        GameObject go = Instantiate(Attack2);
        go.transform.position = transform.position + Vector3.up * 2.0f;
        go.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }
}
