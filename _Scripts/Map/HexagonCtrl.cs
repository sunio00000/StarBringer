using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonCtrl : MonoBehaviour
{
    public Map Hexagon = new Map();
    private void Start()
    {
        if (Hexagon.StageName == "") Hexagon.StageName = transform.name;
        if (Hexagon.StageType == StageType.Road) Hexagon.StageNum = -1;
    }
}
