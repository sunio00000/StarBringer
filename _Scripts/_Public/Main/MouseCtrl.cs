
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCtrl : MonoBehaviour
{
    public static MouseCtrl instance;
    public LayerMask plane;
    public Transform Player;
    public GameObject canvas;
    public GameObject selectedItem,upgradeItem;
    private GameObject temp;
    private GameObject Inventory;
    private const float Sensivity = 10.0f;
    private Vector3 prev=Vector3.zero, curr;
    [SerializeField]
    public bool ItemClick = false, UpgradeReady = false;
    public static bool onBtn = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        //if (Input.mousePresent && Cursor.visible != false)
        //    Cursor.visible = false;

        if (GameCtrl.instance.IsBattle())
        {
            //check & setup
            if (canvas.GetComponent<Canvas>().renderMode != RenderMode.WorldSpace)
                canvas.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
            if (canvas.transform.GetChild(1).gameObject.activeSelf)
                canvas.transform.GetChild(1).gameObject.SetActive(false);
            if (!canvas.transform.GetChild(0).gameObject.activeSelf) ;
                //canvas.transform.GetChild(0).gameObject.SetActive(true);
            canvas.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 10000.0f, plane))
            {
                //  transform.position = hit.point;
                transform.position = Vector3.Lerp(transform.position, hit.point, Time.deltaTime * 25.0f);
                canvas.transform.rotation = Quaternion.LookRotation(transform.position - Player.position);
            }
        }
        else
        {
            if (canvas.GetComponent<Canvas>().renderMode == RenderMode.WorldSpace)
                canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            if (canvas.transform.GetChild(0).gameObject.activeSelf)
                canvas.transform.GetChild(0).gameObject.SetActive(false);
            if (!canvas.transform.GetChild(1).gameObject.activeSelf)
                canvas.transform.GetChild(1).gameObject.SetActive(true);
            
            //item select
            if (GameCtrl.instance.IsStatus() && !onBtn)
            {
                // upgrade item 이 없는데 실행된다. 
                if (Input.GetMouseButtonDown(0))
                {
                    Inventory = GameObject.FindGameObjectWithTag("Inventory");
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (ItemClick)
                    {
                        bool isContinue = true;
                        if (upgradeItem != null)
                        {
                            selectedItem.GetComponent<Item>().OutOfInventory = true;
                            upgradeItem.GetComponent<Item>().OutOfInventory = true;
                            upgradeItem = null;
                        }
                        if (UpgradeReady)
                        {
                            if ((selectedItem.GetComponent<Item>().Class != "SkillBook") || UpgradeItem.selectedSkill){
                                if (Physics.Raycast(ray, out hit, 10000.0f, 1 << LayerMask.NameToLayer("Item"))
                                    && !hit.collider.transform.GetComponent<Item>().GetEquipped())
                                {
                                    upgradeItem = hit.collider.gameObject;
                                    selectedItem.GetComponent<Item>().useToUpgrade = true;
                                    upgradeItem.GetComponent<Item>().readyToUpgrade = true;
                                    UpgradeReady = false; isContinue = false;
                                }
                            }
                        }
                        if (isContinue)
                        {
                            selectedItem.GetComponent<CustomOutline>().enabled = false;
                            selectedItem = null;
                            canvas.transform.GetChild(1).gameObject.SetActive(true);
                            prev = Vector3.zero;
                            ItemClick = false; UpgradeReady = false;
                        }
                    }
                    else if (Physics.Raycast(ray, out hit, 10000.0f, 1 << LayerMask.NameToLayer("Item")))
                    {
                        selectedItem = hit.collider.gameObject;
                        selectedItem.GetComponent<CustomOutline>().enabled = true;
                        selectedItem.GetComponent<CustomOutline>().OutlineWidth = 4.0f;
                        ItemClick = true;
                    }
                    else if (Physics.Raycast(ray, out hit, 10000.0f, 1 << LayerMask.NameToLayer("CosumableItem")))
                    {
                        selectedItem = hit.collider.gameObject;
                        selectedItem.GetComponent<CustomOutline>().enabled = true;
                        selectedItem.GetComponent<CustomOutline>().OutlineWidth = 4.0f;
                        ItemClick = true;
                        UpgradeReady = true;
                    }
                    else
                    {
                        selectedItem = null; upgradeItem = null;
                    }
                }
                // when put on, mouse position ????
                if (Input.GetMouseButtonUp(0) && ItemClick)
                {
                    selectedItem.GetComponent<CustomOutline>().OutlineColor = Color.yellow;
                    prev = Vector3.zero;
                }
                else if (Input.GetMouseButton(0) && ItemClick 
                    && !selectedItem.GetComponent<Item>().OutOfInventory && upgradeItem == null)
                {
                    selectedItem.GetComponent<CustomOutline>().OutlineColor = Color.red;
                    canvas.transform.GetChild(1).gameObject.SetActive(false);
                    curr = Input.mousePosition;
                    if (prev == Vector3.zero)
                    {
                        prev = Input.mousePosition;
                    }
                    if (selectedItem.GetComponent<Item>().IsSwordOrShield())
                    {
                        selectedItem.transform.parent.position = new Vector3(-90,
                            Mathf.Clamp(selectedItem.transform.parent.position.y + (curr.y - prev.y) * Time.deltaTime * Sensivity, -40.0f, 45.0f),
                            Mathf.Clamp(selectedItem.transform.parent.position.z + (curr.x - prev.x) * Time.deltaTime * Sensivity, -90.0f, -50.0f)
                            );
                    }
                    else
                    {
                        selectedItem.transform.position = new Vector3(-90,
                            Mathf.Clamp(selectedItem.transform.position.y + (curr.y - prev.y) * Time.deltaTime * Sensivity, -40.0f, 45.0f),
                            Mathf.Clamp(selectedItem.transform.position.z + (curr.x - prev.x) * Time.deltaTime * Sensivity, -90.0f, -50.0f)
                            );
                    }
                    prev = curr;
                }
            }
            canvas.transform.GetChild(1).position = new Vector3(
                Mathf.Clamp(
                    Camera.main.ScreenToViewportPoint(Input.mousePosition).x * Camera.main.pixelWidth+10,
                    0,
                    Camera.main.pixelWidth),
                Mathf.Clamp(
                    Camera.main.ScreenToViewportPoint(Input.mousePosition).y * Camera.main.pixelHeight-25,
                    0,
                    Camera.main.pixelHeight));
        }
    }
}
