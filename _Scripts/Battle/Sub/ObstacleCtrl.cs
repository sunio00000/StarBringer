using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObsType
{
    Normal,
    Breakable,
    Movable,
    NumberOfType
}

public class ObstacleCtrl : MonoBehaviour
{
    public ObsType type;
    public float Hp;
    public GameObject Potion;
    public ParticleSystem Damage, Break;
    private const float MovePad = 0.05f;
    public bool isExist = true;

    void Awake()
    {
        if (Damage == null || Break == null) Debug.Log("??????");
        if (Damage != null ) Damage.Stop();
        if (Break != null) Break.Stop();
        transform.tag = "Obstacle";
        if (type != ObsType.Breakable) Hp = -1f;
    }

    public void Damaged(float DMG, Vector3 Spot)
    {
        Damage.Play();
        if (type == ObsType.Normal) { }
        else
        {
            transform.GetComponent<Animation>().Play("Damaged");
            if (type == ObsType.Breakable)
            {
                Hp -= 5;
                if (Hp <= 0)
                {
                    isExist = false;
                    transform.GetComponent<Animation>().Play("Break");
                    Break.Play();
                }
            }
            else if (type == ObsType.Movable)
            {
                Move(Spot);
            }
        }
    }

    public void SetOFF()
    {
        gameObject.SetActive(false);
    }

    private void Move(Vector3 Spot)
    {
        Vector3 arrow = transform.parent.position - Spot;
        Vector3 dest;
        if (Mathf.Abs(arrow.x) >= Mathf.Abs(arrow.z)) 
        {
            if (arrow.x >= 0) dest = transform.parent.position + Vector3.left * -MovePad;
            else dest = transform.position + Vector3.left * MovePad;
            Debug.Log("x");
        }
        else
        {
            if (arrow.z >= 0) dest = transform.parent.position + Vector3.forward * MovePad;
            else dest = transform.parent.position + Vector3.forward * -MovePad;
            Debug.Log("z");
        }
        transform.parent.position = Vector3.Lerp(transform.parent.position, dest, 1f);
        //StartCoroutine(MoveCube(dest));
    }

    private IEnumerator MoveCube(Vector3 destination)
    {
        bool PlayCor = true;  
        while (PlayCor)
        {
            transform.parent.position = Vector3.Lerp(transform.parent.position, destination, 1f);
            if (transform.parent.position.x == destination.x 
                || transform.parent.position.z == destination.z) PlayCor = false;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void DropSomething()
    {
        Instantiate(Potion,transform);
    }
}
