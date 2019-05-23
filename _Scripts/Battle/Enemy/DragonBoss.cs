using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBoss: Boss
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
        pattern.Add(new Pattern(1, 5, 3));
        pattern.Add(new Pattern(1, 7, 4));
        attackAnims.Add(MakeZeroAttack);
        attackAnims.Add(MakeOneAttack);
        attackAnims.Add(MakeTwoAttack);
        attacks.Add(DefaultAttack);
        attacks.Add(OneAttack);
        attacks.Add(TwoAttack);
        moves.Add(DefaultMove);
        moves.Add(OneMove);
        moves.Add(OneMove);
        AnimFunc = MakeZeroAttack;
    }
    public override void Update()
    {
        base.Update();
        CheckQuarter();
    }

    private void CheckQuarter()
    {
        for (int i = 1; i < Quarter - 1; ++i)
        {
            if (CurrHp <= (i / (float)Quarter) * MaxHp)
            {
                if (tmp != i)
                {
                    int state = Quarter - 1 - i;
                    AttackFunction(false); MoveFunction(false);
                    Debug.Log(state + " 단계 그로기");
                    SetUpAbility(pattern[state]);
                    AnimFunc = attackAnims[state];
                    AttackFunction = attacks[state];
                    MoveFunction = moves[state];
                    tmp = i;
                }
                break;
            }
        }
    }

    public override void AnimAttackFucntion()
    {
        AnimFunc();
    }

    protected override void DefaultAttack(bool flag)
    {
        transform.GetComponent<Animator>().SetBool("Attack", flag);
    }

    private void OneAttack(bool flag)
    {
        transform.GetComponent<Animator>().SetBool("Attack", flag);
    }

    private void TwoAttack(bool flag)
    {
        transform.GetComponent<Animator>().SetBool("Attack", flag);
    }

    private void OneMove(bool flag)
    {
        transform.GetComponent<Animator>().SetBool("Move", flag);
    }

    private void MakeZeroAttack()
    {
        AttackCtrl.instance.ShotSector(AttackObject, 5, transform);
    }

    private void MakeOneAttack()
    {
        AttackCtrl.instance.ShotSector(Attack2, 8, transform);
    }

    private void MakeTwoAttack()
    {
        AttackCtrl.instance.ShotSector(Attack1, 12, transform);
    }
}
