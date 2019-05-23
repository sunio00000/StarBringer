using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpCtrl : MonoBehaviour
{
    public int CureHpValue = 10;
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            GameCtrl.instance.playerData().currState[(int)What.Hp] += CureHpValue;
            gameObject.SetActive(false);
        }
    }
}
