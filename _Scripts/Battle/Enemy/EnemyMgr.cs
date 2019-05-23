using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public abstract class EnemyMgr : MonoBehaviour
{
    public Transform HpImage;
    public Transform go;
    public Collider Trigger;
    public ParticleSystem damagedParticle;
    protected string Name;
    public float MaxHp;
    [HideInInspector]
    public float CurrHp;
    public float Power;
    public float MoveSpeed;
    public float AttackRange;
    public float AgressiveRange;
    public List<AnimationClip> AnimClip = new List<AnimationClip>();
    protected Dictionary<string, int> ClipName = new Dictionary<string, int>()
    {
        {"Dead",0 }
    };
    public Transform HUD;
    public GameObject[] Drops;
    public List<float> Probablity;
    public int MinStar,MaxStar;
    protected delegate void AttackAnim();
    protected delegate void Attack(bool flag);
    protected delegate void Move(bool flag);
    protected AttackAnim AnimFunc;
    protected Attack AttackFunction;
    protected Move MoveFunction;

    protected void SetInit(string n)
    {
        CurrHp = MaxHp;
        Name = n;
        gameObject.layer = LayerMask.NameToLayer("Enemy");

    }
    public abstract void AnimAttackFucntion();
    protected abstract void DefaultAttack(bool flag);
    protected abstract void DefaultMove(bool flag);
    protected abstract void AttackPlayer();
    protected abstract void MoveToPlayer();
    protected abstract bool IsDead();
    protected abstract IEnumerator Death();
    protected abstract void DropItem(List<float> ps);
    protected abstract void DropStar();
    protected abstract float HpRatio();
    public abstract void LossHp(float damage);
}
