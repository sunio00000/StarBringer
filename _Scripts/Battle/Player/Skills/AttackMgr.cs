using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillName = System.String;

abstract public class AttackMgr : MonoBehaviour
{

    public List<GameObject> Projectiles = new List<GameObject>();
    public List<ProjectileMgr> System = new List<ProjectileMgr>();

    // [WARNING] SkillType.Count == SkillSystem.Count Always TRUE
    protected readonly Dictionary<string, GameCtrl.SkillType> SkillType
        = new Dictionary<string, GameCtrl.SkillType>();
    protected readonly Dictionary<string, ProjectileMgr> SkillSystem
        = new Dictionary<string, ProjectileMgr>();
    protected readonly Dictionary<string, GameObject> SkillObject
        = new Dictionary<string, GameObject>();

    public Transform mousePos;
    public GameObject body;
    public GameObject muzz;
    protected int R, G, B;
    // socket from gameCtrl.instance import
    protected static float ShotCycleRate, ShotCurrRate;

    public virtual void Awake()
    {
        InitializeSkill();
    }

    protected float CurrSkillCreation()
    {
        SkillName Skill = GameCtrl.instance.GetSkill();
        if (Skill == "DontEquipped") return -1f;
        else if (Skill == "")
        {
            Debug.LogError("사용할 수 있는 스킬이 없습니다.");
            return -1f;
        }
        else return SkillType[Skill](transform);
    }

    protected System.Type CurrSkillSystem()
    {
        return SkillSystem[GameCtrl.instance.GetSkill()].GetType();
    }

    private void InitializeSkill()
    {
        for (int i = 0; i < Projectiles.Count; ++i)
            SkillObject.Add(Projectiles[i].name, Projectiles[i]);

        // [WARNING] Carefully, modified this code. //
        // When you add Skill, you have to add Skill list also. //
        //////////////////////////////////////////////////////////
        SkillType.Add("FireBall", new GameCtrl.SkillType(AttackCtrl.instance.ShotSector));
        SkillSystem.Add("FireBall", new StraightCtrl());
        // FireBall
        // R
        // G
        // B
        //////////////////////////////////////////////////////////
        SkillType.Add("PFireBall", new GameCtrl.SkillType(AttackCtrl.instance.ShotSector));
        SkillSystem.Add("PFireBall", new ParabolaCtrl());
        // SkillName
        // R
        // G
        // B
        //////////////////////////////////////////////////////////
        SkillType.Add("LightningBall", new GameCtrl.SkillType(AttackCtrl.instance.ShotSector));
        SkillSystem.Add("LightningBall", new StraightCtrl());
        // SkillName
        // R
        // G
        // B
        //////////////////////////////////////////////////////////
        SkillType.Add("Teleport", new GameCtrl.SkillType(AttackCtrl.instance.MoveFace));
        SkillSystem.Add("Teleport", new TeleportCtrl());
        // SkillName
        // R
        // G
        // B
        //////////////////////////////////////////////////////////
        SkillType.Add("Orb", new GameCtrl.SkillType(AttackCtrl.instance.AroundSuriken));
        SkillSystem.Add("Orb", new AroundCtrl());
        // SkillName
        // R
        // G
        // B//////////////////////////////////////////////////////////
        SkillType.Add("Meteor", new GameCtrl.SkillType(AttackCtrl.instance.ShotMeteor));
        SkillSystem.Add("Meteor", new MeteorCtrl());
        // SkillName
        // R
        // G
        // B
        SkillType.Add("Heal", new GameCtrl.SkillType(AttackCtrl.instance.ShotHeal));
        SkillSystem.Add("Heal", null);
        // SkillName
        // R
        // G
        // B
    }
}
