using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    protected const int Quarter = 4;
    protected List<AttackAnim> attackAnims = new List<AttackAnim>();
    protected List<Attack> attacks = new List<Attack>();
    protected List<Move> moves = new List<Move>();
    protected class Pattern
    {
        public float MoveSpeed { get; set; }
        public float Power { get; set; }
        public float AttackRange { get; set; }

        public Pattern(float M, float P, float R)
        {
            MoveSpeed = M;
            Power = P;
            AttackRange = R;
        }
    }

    protected List<Pattern> pattern = new List<Pattern>(); 
    protected void SetUpAbility(Pattern A)
    {
        MoveSpeed = A.MoveSpeed;
        Power = A.Power;
        AttackRange = A.AttackRange;
    }
    protected void SetUpAbility(float M)
    {
        MoveSpeed = M;
    }
    protected void SetUpAbility(float P, float R)
    {
        Power = P;
        AttackRange = R;
    }
}
