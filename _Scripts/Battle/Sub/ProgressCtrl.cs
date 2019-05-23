using UnityEngine;
using System;
using UnityEngine.UI;

public class ProgressCtrl : MonoBehaviour
{
    public static bool GameClear;
    public Sprite[] sprites = new Sprite[4];
    public Sprite[] barSprites = new Sprite[4];
    public Image image;
    public Image progress;
    public Scrollbar bar;
    private static float playTime, startValue;
    private Boss boss; // castle
    private delegate float Current();
    Current SceneState = null;

    void Start()
    {
        GameClear = false;
        switch (GameCtrl.instance.currentMapType)
        {
            case StageType.AllKillStage:
                image.sprite = sprites[0];
                progress.sprite = barSprites[0];
                startValue = 1; SetProgress(1); 
                SceneState = EnemyCount;
                break;
            case StageType.BossStage:
                boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<DragonBoss>();
                image.sprite = sprites[1];
                progress.sprite = barSprites[1];
                startValue = 1; SetProgress(1); 
                SceneState = BossHealth;
                break;
            case StageType.TimeStage:
                playTime = GameObject.FindGameObjectWithTag("Time").transform.localPosition.z;
                image.sprite = sprites[2];
                progress.sprite = barSprites[2];
                startValue = 0; SetProgress(0); 
                SceneState = TimeLength;
                break;
            case StageType.EndureStage:
                //castle = GameObject.FindGameObjectWithTag("Castle").GetComponent<Castle>();
                image.sprite = sprites[3];
                progress.sprite = barSprites[3];
                startValue = 1; SetProgress(1); 
                SceneState = CastleHealth;
                break;
        }
    }

    void Update()
    {
        if(GameCtrl.instance.currentMap !=0)
            SetProgress(SceneState());
    }

    delegate bool Condition(float value);
    private Condition condition = (float value) =>
    {
        if (startValue == 1 && value <= 0) return true;
        else if (startValue == 0 && value >= 1) return true;
        else return false;
    };

    private void SetProgress(float value)
    {
        progress.fillAmount = value;
        bar.value = value;
        GameClear = condition(value);
    }

    private float EnemyCount()
    {
        return BattleSceneCtrl.EnemyRate();
    }

    private float BossHealth()
    {
        return boss.CurrHp / boss.MaxHp;
    }

    private float CastleHealth()
    {
        return 0;
    }

    private float TimeLength()
    {
        return BattleSceneCtrl.StartTime > 0 ? (Time.time - BattleSceneCtrl.StartTime) / playTime : 0;
    }


}
