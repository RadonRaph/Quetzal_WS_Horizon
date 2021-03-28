using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using BetterAudio;
using UnityEngine.SceneManagement;

public class Controller_WithFocusPoint : MonoBehaviour
{
    public Transform Character;
    public Transform Camera;
    public Transform Focus;
    public BoxCollider QuetzalCollider;

    public Transform Quetzal;

    public Quetzal quetzalInputs;

    public LayerMask mask;

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

    [Header("Animations")]
    public Animator quetzalAnimator;

    [Header("Sensibility")]
    public float focusPositionLerp = 0.1f;
    public float targetPositionLerp = 0.1f;
    public float characterLerp = 0.05f;
    public float angleMulti = 1.5f;
    public float angleLerp = 0.3f;

    public bool GameRunning = false;


    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = Camera.position - Character.position;
        Cursor.lockState = CursorLockMode.Locked;
    }

    Vector3 target;
    float eventTime = 1.5f;
    float angle = 0;
    // Update is called once per frame
    void Update()
    {
        if (!GameRunning)
            return;


        eventTime -= Time.deltaTime;

        if (stickDirection.y > 0)
        {
            speed += Time.deltaTime;
            quetzalAnimator.SetBool("Flapping", true);
            if (eventTime < 0)
            {
                BetterAudio.BetterAudioEvent.SendEvent("Flapping");
                eventTime = 1.5f;
            }
        }
        else
        {
            speed -= Time.deltaTime;
            quetzalAnimator.SetBool("Flapping", false);
            if (eventTime < 0)
            {
                BetterAudio.BetterAudioEvent.SendEvent("Planning");
                //eventTime = 4.4f;
            }
        }
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

        Focus.LookAt(Character);
        Focus.forward *= -1;

       
        //On vérifie si le point de focus touche quelque chose si oui on le fais aller vers l'extrimité
        RaycastHit hit;
        if (Physics.Linecast(Character.position, Focus.position+Focus.forward*minDistance, out hit, mask)){
            Vector3 point = hit.collider.ClosestPoint(hit.point);

            Vector3 pointVec = point - hit.transform.position;

            Vector3 newPos1 = point + pointVec * 0.5f;

            Focus.position = Vector3.Lerp(Focus.position, newPos1, focusPositionLerp);

            
        }

           target = Vector3.Lerp(target, Focus.position, targetPositionLerp);
        

       

        //LE character s'oriente vers un point de target qui est une version smooth du point de focus
        Vector3 dir = target - Character.position;
        Character.forward = Vector3.Lerp(Character.forward, dir.normalized, characterLerp);


        //Celon l'angle entre le character et le point de focus on incline l'oiseau
        //Debug.Log((Vector2.Dot(Focus.forward, Character.forward) / (Focus.forward.magnitude * Character.forward.magnitude)));

        //float angle = (Character.InverseTransformDirection(Focus.forward - Character.forward)).x*angleMulti;
        /*
        float angle = Vector3.SignedAngle(Character.forward, Focus.position - Character.position, Character.up) * angleMulti;
        angle = Mathf.Clamp(Mathf.Abs(angle)-0.2f, 0, 0.1F)*Mathf.Sign(angle);*/

        angle = Mathf.Lerp(angle, mouseDelta.x, angleMulti);
        angle = Mathf.Clamp(angle, -3, 3);
        Character.eulerAngles = Vector3.Lerp(Character.eulerAngles, Character.eulerAngles + (-angle * 180) * Vector3.forward, angleLerp);

        //On calcule les force attendu
        if (mouseDelta.magnitude > 5f)
        {
            velocity += (Focus.right * mouseDelta.x + Focus.up * mouseDelta.y) * speed * MouseSensibility;
            quetzalAnimator.SetBool("Flapping", true);
            if (eventTime < 0)
            {
                BetterAudio.BetterAudioEvent.SendEvent("Flapping");
                eventTime = 1.5f;
            }
        }
        else if (stickDirection.y <= 0)
        {
            quetzalAnimator.SetBool("Flapping", false);
            if (eventTime < 0)
            {
                BetterAudio.BetterAudioEvent.SendEvent("Planning");
               // eventTime = 4.4f;
            }
        }
            

        velocity += Focus.forward * speed;



        //Drag to a minimum velocity
        velocity = Vector3.Lerp(velocity, Focus.forward * minSpeed, drag);

        //On modifie le focus selon les input
        Focus.position += velocity * Time.deltaTime;
        Focus.position = Vector3.Lerp(Focus.position, Character.position + Character.forward * minDistance, 0.1f);


        float dist = Vector3.Distance(Character.position, Focus.position);
        
        if (dist > maxDistance)
            Focus.position = Character.position + Vector3.ClampMagnitude(Focus.position - Character.position, maxDistance);

        //Interpolation Polynomial pour gradué la vitesse selon la distance
        float ratio = ((dist - minDistance) / ((maxDistance - minDistance) / 2f));
        ratio = Mathf.Clamp(ratio, 0, 1);

        
        Character.position += Character.forward * 0.1f * speed;


        //Offset de camera
        Camera.DOLookAt(Character.position + Character.forward, 0.5f);
        Camera.position = Vector3.Lerp(Camera.position, Character.TransformPoint(cameraOffset), 0.1f);

        Debug.DrawLine(Character.position, target, Color.magenta);
        Debug.DrawLine(target, Focus.position, Color.green);
    }

    public Vector3 GetPointOnEdge(Bounds bound, Vector3 pointOnbounds, float factor = 1)
    {
        Vector3 result = new Vector3(Mathf.Sign(pointOnbounds.x) * bound.extents.x * factor, Mathf.Sign(pointOnbounds.y) * bound.extents.y * factor, Mathf.Sign(pointOnbounds.z) * bound.extents.z * factor);
        return result;
       

    }

    public void DoABarrelRoll()
    {
        StartCoroutine(_DoBarrelRoll(1f));
        Debug.Log("Do a Barrel Rool");
    }

    IEnumerator _DoBarrelRoll(float seconds)
    {
        float t = 0;
        quetzalAnimator.Play("Figure");
        speed *= 2;
        Quetzal.DOLocalRotate(new Vector3(0, 90, 0), 0.3f);
        yield return new WaitForSeconds(0.3f);
        while (t < seconds)
        {
            
            t += Time.deltaTime;
            Quetzal.localEulerAngles = new Vector3((t / seconds * 360), 90, 0);
            yield return new WaitForEndOfFrame();
        }
        Quetzal.localEulerAngles = new Vector3(0, 90, 0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
