using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackableJoystick : Joystick
{
    public GameObject TargetArea;
    private const float deltaMoveSpeed = 1.0f;
    public static float AttackAngle = 0.0f;
    public static bool ReadyToAttack = true;
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;
    [SerializeField] private JoystickType joystickType = JoystickType.Fixed;

    private Vector2 fixedPosition = Vector2.zero;

    public void SetMode(JoystickType joystickType)
    {
        this.joystickType = joystickType;
        if (joystickType == JoystickType.Fixed)
        {
            background.anchoredPosition = fixedPosition;
            background.gameObject.SetActive(true);
        }
        else
            background.gameObject.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        fixedPosition = background.anchoredPosition;
        SetMode(joystickType);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        TargetArea = GameObject.FindGameObjectWithTag("TargetArea");
        TargetArea.transform.GetChild(0).gameObject.SetActive(true);
        ReadyToAttack = true;
        if (joystickType != JoystickType.Fixed)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
        }
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        AttackAngle = Mathf.Atan2(Direction.y, -Direction.x) * Mathf.Rad2Deg;
        if (joystickType != JoystickType.Fixed)
            background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
        ReadyToAttack = false;
        TargetArea.GetComponent<AttackAreaCtrl>().area = TargetArea.transform.position;
        TargetArea.GetComponent<AttackAreaCtrl>().SetOrigin();
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (joystickType == JoystickType.Dynamic && magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            background.anchoredPosition += difference;
        }
        base.HandleInput(magnitude, normalised, radius, cam);
    }
}

public enum JoystickType { Fixed, Floating, Dynamic }