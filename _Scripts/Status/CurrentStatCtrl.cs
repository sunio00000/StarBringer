using UnityEngine;
using UnityEngine.UI;

public class CurrentStatCtrl : MonoBehaviour
{
    public Text Power;
    public Text Hp;
    public Text Speed;
    public Text RedStar, BlueStar, YellowStar;
    void Update()
    {
        StatLog(Power, "POWER", GameCtrl.instance.data[(int)Whose.Player].currState[(int)What.Power].ToString());
        StatLog(Hp, "HP", GameCtrl.instance.data[(int)Whose.Player].currState[(int)What.Hp]
            + " / " + GameCtrl.instance.data[(int)Whose.Player].maxState[(int)What.Hp]);
        StatLog(Speed, "SPEED", GameCtrl.instance.data[(int)Whose.Player].currState[(int)What.MoveSpeed].ToString());
        StatLog(Color.red, RedStar, "", GameCtrl.instance.data[(int)Whose.Player].Star[(int)StarColor.Red].ToString());
        StatLog(Color.yellow, YellowStar, "", GameCtrl.instance.data[(int)Whose.Player].Star[(int)StarColor.Yellow].ToString());
        StatLog(Color.blue, BlueStar, "", GameCtrl.instance.data[(int)Whose.Player].Star[(int)StarColor.Blue].ToString());
    }

    void StatLog(Text obj, string n, string v)
    {
        obj.text = "";
        obj.text += v;
    }
    void StatLog(Color c, Text obj, string n, string v)
    {
        obj.color = c;
        obj.text = "";
        obj.text += v;
    }
}
