using UnityEngine;
using UnityEngine.UI;


public class SpriteTest : MonoBehaviour
{
    public GameObject[] list = new GameObject[3];
    private float start;
    void Start()
    {
        start = Time.time;
    }

    void Update()
    {
        if (Time.time - start >= 1){
            foreach (var l in list) l.SetActive(false);
            for(int i=0; i < 3; ++i)
            {
                list[i].SetActive(true);
                list[i].transform.localPosition = new Vector3(Random.Range(-25, 25), Random.Range(-30, 30), 0);
            }
            start = Time.time;
        }
    }
}
