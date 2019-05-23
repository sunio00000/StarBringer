using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamagedText : MonoBehaviour
{
    private const int maxValueLength = 7;
    private int MaxText;
    public List<Sprite> font = new List<Sprite>();
    public int currText = 0;
    public Transform TextGroup;

    void Start()
    {
        MaxText = transform.childCount - 1;
    }

    public void Damaged(float Damage)
    {
        SetValueToSprite(Damage);
    }

    public void SetValueToSprite(float Damage)
    {
        float tmp = (Damage - 0.8f* GameCtrl.instance.playerData().currState[(int)What.Power]) /
            0.7f * GameCtrl.instance.playerData().currState[(int)What.Power];
        TextGroup.GetChild(0).transform.localPosition = new Vector3(0, 0.95f, 0);
        if (Damage > Mathf.Pow(10, maxValueLength))
        {
            Debug.LogError("Damage has more over i think.");
            return;
        }
        else if(Damage == 0)
        {
            TextGroup.GetChild(0).GetChild(0).GetComponent<Image>().sprite = font[0];
            return;
        }
        else
        {
            for(int index = 0; index < maxValueLength; ++index)
            {
                bool isEnable = TextGroup.GetChild(0).GetChild(index).GetComponent<Image>().enabled;
                if (Damage == 0)
                {
                    if (isEnable == true)
                    {
                        TextGroup.GetChild(0).GetChild(index).GetComponent<Image>().enabled = false;
                    }
                }
                else
                {
                    if (index != 0)
                        TextGroup.GetChild(0).transform.localPosition += new Vector3(0.2f, 0, 0);

                    if (isEnable == false)
                        TextGroup.GetChild(0).GetChild(index).GetComponent<Image>().enabled = true;
                    TextGroup.GetChild(0).GetChild(index).GetComponent<Image>().sprite = font[(int)(Damage % 10)];
                    TextGroup.GetChild(0).GetChild(index).GetComponent<Image>().color = new Color(1, 1-tmp, 0);
                    Damage = (int)(Damage / 10);
                }
            }
            TextGroup.GetChild(0).GetComponent<Animation>().Play();
        }
    }

    public Text GetText()
    {
        return TextGroup.GetChild(currText + 1).GetComponent<Text>();
    }

    public Text GetText(int num)
    {
        return TextGroup.GetChild(num + 1).GetComponent<Text>();
    }

    public void NextText()
    {
        currText++;
        currText %= MaxText;
    }
}
