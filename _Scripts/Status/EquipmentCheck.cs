using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentCheck : MonoBehaviour
{
    public ItemType type;
    public List<GameObject> socket = new List<GameObject>();
    public Sprite[] socketStar = new Sprite[(int)StarColor.NumberOfType];

    void Start()
    {
        for (int i = 0; i < transform.GetChild(0).childCount; ++i)
        {
            socket.Add(transform.GetChild(0).GetChild(i).gameObject);
            transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (GameCtrl.instance.data[(int)Whose.Player].IsEquipped[type])
        {
            DisplayInformation();
        }
        else
        {
            foreach (var s in socket) s.SetActive(false);
        }
    }

    private void DisplayInformation()
    {
        Item item = GameCtrl.instance.data[(int)Whose.Player].Equipments[(int)type].GetComponent<Item>();
        for (int i=0; i < item.GetSocketCount(); ++i)
        {
            socket[i].SetActive(true);
            string color = item.GetSocketColor(i);
            if (color == "") continue;
            else
            {
                socket[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                if (color == "Red")
                {
                    socket[i].transform.GetChild(0).GetComponent<Image>().sprite = socketStar[(int)StarColor.Red];
                }
                else if (color == "Yellow")
                {
                    socket[i].transform.GetChild(0).GetComponent<Image>().sprite = socketStar[(int)StarColor.Yellow];
                }
                else if (color == "Blue")
                {
                    socket[i].transform.GetChild(0).GetComponent<Image>().sprite = socketStar[(int)StarColor.Blue];
                }
            }
        }
    }
}
