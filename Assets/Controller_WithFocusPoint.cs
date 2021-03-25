using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Controller_WithFocusPoint : MonoBehaviour
{
    public Transform Character;
    public Transform Camera;
    public Transform Focus;

    public Quetzal quetzalInputs;

    Vector2 stickDirection;
    Vector2 mouseDelta;

    public float MouseSensibility = 0.1f;

    #region SettingUpQuetzalInput
    public void Awake()
    {
        quetzalInputs = new Quetzal();
    }

    private void OnEnable()
    {
        quetzalInputs.Enable();
        quetzalInputs.Player.Moving.performed += context =>
        {
            stickDirection = context.ReadValue<Vector2>();
        };

        quetzalInputs.Player.Look.performed += ctx =>
        {
            mouseDelta = ctx.ReadValue<Vector2>();
        };

        quetzalInputs.Player.Moving.canceled += ctx => { stickDirection = Vector2.zero; };
    }

    private void OnDisable()
    {
        quetzalInputs.Disable();
    }
    #endregion

    [SerializeField]
    private Vector3 velocity;
    [SerializeField]
    private float speed;

    private Vector3 cameraOffset;

    [Header("Fligh properties")]
    public float drag = 0.1f;
    public float minSpeed = 2;
    public float maxSpeed = 5;
    public float minDistance = 30;
    public float maxDistance = 50;


    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = Camera.position - Character.position;
        Cursor.lockState = CursorLockMode.Locked;
    }

    Vector3 dir;
    // Update is called once per frame
    void Update()
    {
        if (stickDirection.y > 0)
        {
            speed += Time.deltaTime;
        }
        else
        {
            speed -= Time.deltaTime;
        }
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

        Focus.LookAt(Character);
        Focus.forward *= -1;






        RaycastHit hit;
        //  if (Physics.BoxCast(Character.position, Vector3.one*2, dir, out hit, Quaternion.identity, minDistance))
        //{
        //Debug.Log(hit.transform.name);
        if (Physics.Raycast(Character.position, Character.forward,  out hit, minDistance))
        {
            dir = Vector3.Lerp(dir, Vector3.Reflect(dir, hit.normal) + hit.normal, 0.2f);
            // Focus.position = Vector3.Lerp(Focus.position, Character.position+dir, 0.01f);
        }
        else
        {
            dir = Vector3.Lerp(dir, (Focus.position - Character.position), 0.1f);
        }
        //}





        Character.forward = Vector3.Lerp(Character.forward, dir.normalized, 0.01f);
        Debug.DrawRay(Character.position, dir, Color.blue);



        float angle = (Character.InverseTransformDirection(Focus.forward - Character.forward)).x;
        Character.eulerAngles = Character.eulerAngles + (-angle * 90) * Vector3.forward;


        if (mouseDelta.magnitude > 5f)
            velocity += (Focus.right * mouseDelta.x + Focus.up * mouseDelta.y) * speed * MouseSensibility;

        velocity += Focus.forward * speed;



        //Drag to a minimum velocity
        velocity = Vector3.Lerp(velocity, Focus.forward * minSpeed, drag);

        Focus.position += velocity * Time.deltaTime;

        Vector3 path = Focus.position - Character.position;

        float dist = Vector3.Distance(Character.position, Focus.position);

        if (dist > maxDistance)
            Focus.position = Character.position + Vector3.ClampMagnitude(Focus.position - Character.position, maxDistance);

        //Interpolation Polynomial pour gradué la vitesse selon la distance
        float ratio = ((dist - minDistance) / ((maxDistance - minDistance) / 2f));
        ratio = Mathf.Clamp(ratio, 0, 2);

        Character.position += Character.forward * ratio * speed;
        Debug.DrawLine(Character.position, Focus.position - ratio * path, Color.green);

        Camera.DOLookAt(Character.position + Character.forward, 0.5f);
        Camera.position = Vector3.Lerp(Camera.position, Character.TransformPoint(cameraOffset), 0.1f);


    }
}
