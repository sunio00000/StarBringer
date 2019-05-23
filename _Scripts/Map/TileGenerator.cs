using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TileValue
{
    seaTile,
    simpleTile,
    startTile,
    roadTile,
    endTile,
    NumberOfType
}

public class TileGenerator : MonoBehaviour
{
    public GameObject TilePos;
    public GameObject test;
    private int TileWidth = 20, TileHeight = 20;
    private const float XPadding = 0.885f, YPadding = 1.04f, 
        XStart = -4.4f, YOddStart = -3f, YStart = -3.5f;

    ///  FOR TEST
    void OnValidate()
    {
        //TileCreate();
    }

    void TileCreate()
    {
        for(int i=0; i<=TileWidth; ++i)
        {
            for(int j=0; j<=TileHeight; ++j)
            {
                if (i % 2 == 0) {
                    if (j == TileHeight) continue;
                    GameObject go;
                    if (GameObject.Find("Tile_" + j + "_" + i) == null)
                    {
                        go = Instantiate(TilePos);
                        go.transform.SetParent(transform);
                        go.name = "Tile_" + j + "_" + i;
                    }
                    else go = GameObject.Find("Tile_" + j + "_" + i);
                    go.transform.localPosition = new Vector3(XStart + i * XPadding, YOddStart + j * YPadding, 0);
                    go.transform.localRotation = Quaternion.Euler(new Vector3(120, 90, 90));
                    //GameObject tmp = Instantiate(test);
                    //tmp.transform.SetParent(go.transform);
                    //tmp.transform.localPosition = Vector3.zero-Vector3.up*0.2f;
                    //tmp.transform.localRotation = Quaternion.identity;
                    //tmp.transform.localScale += 0.1f * Vector3.one;
                }
                else
                {
                    GameObject go;
                    if (GameObject.Find("Tile_" + j + "_" + i) == null)
                    {
                        go = Instantiate(TilePos);
                        go.transform.SetParent(transform);
                        go.name = "Tile_" + j + "_" + i;
                    }
                    else go = GameObject.Find("Tile_" + j + "_" + i);
                    go.transform.localPosition = new Vector3(XStart + i * XPadding, YStart + j * YPadding, 0);
                    go.transform.localRotation = Quaternion.Euler(new Vector3(120, 90, 90));
                    //GameObject tmp = Instantiate(test);
                    //tmp.transform.SetParent(go.transform);
                    //tmp.transform.localPosition = Vector3.zero-Vector3.up * 0.2f;
                    //tmp.transform.localRotation = Quaternion.identity;
                    //tmp.transform.localScale += 0.1f * Vector3.one;
                }
            }
        }
    }
}
