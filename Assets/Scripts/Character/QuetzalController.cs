using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuetzalController : MonoBehaviour
{
    private Quetzal quetzalInputs;

    [Header("Body Elements")]
    public GameObject body;
    public GameObject quetzalCamera;

    [Header("Fly Parameters")]
    public float maxSpeed;
    float currentSpeed;
    public float decelerationOverTime = 0.05f; //Réduction de la vitesse naturelle, aucune touche enfoncée.

    private Vector2 stickDirection; //Direction WASD en Vector2.
    private Vector2 mouseDelta;
    private float timeCount;
    [Space]
    public float minDeltaForX; //Delta min pour détecter un changement en X (Mouse Delta)
    public float minDeltaForY; //Delta min pour détecter un changement en Y
    [Space]
    public float RotSpeedX;
    public float RotSpeedY;
    public float RotSpeedZ;
    public float maxXRotAngle = 45f; //Rotation maximale de l'oiseau lors d'une ascension/descente.
    public float maxZRotAngle = 60f; //Rotation max de l'oiseau en cas de virage. (Touches Q et D par exemple)

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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Si des touches directionnelles (WASD/ZQSD) sont enfoncées
        if(stickDirection.magnitude > 0) 
        {
            MoveQuetzal(stickDirection);
        }

        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        /*La vitesse ralentit avec le temps. Si la touche Forward n'est plus appliquée, l'oiseau finit par faire du surplace.
        if (currentSpeed > 0f)
        {
            currentSpeed -= Time.deltaTime / 0.3f;
        }
        else
        {
            currentSpeed = 0f;
        }*/

        if (mouseDelta.magnitude > 0)
        {
            //Debug.Log("MouseDelta : " + mouseDelta); //Faire en sorte d'avoir quelque chose de + intéressant.
            //Ne pas avoir besoin de tout le temps rebouger la souris.
            RotateQuetzal(mouseDelta);
        }
        
        //Graduellement retourner vers une rotation "normale" pour un oiseau en vol. (Être parallèle au sol)
        transform.rotation = Quaternion.Slerp( //Consomme trop...
            transform.rotation,
            transform.rotation * Quaternion.AngleAxis(0 - transform.rotation.z, Vector3.forward) * Quaternion.AngleAxis(0 - transform.rotation.x, Vector3.right),
            timeCount
            );
        timeCount += Time.deltaTime;
        
    }

    /// <summary>
    /// Gère la vitesse et les rotations en Z du Quetzal.
    /// </summary>
    /// <param name="direction">Mouvement directionnel en Vector2 (ZQSD)</param>
    private void MoveQuetzal(Vector2 direction)
    {
        if (direction.x > 0) //Rotate en Z (négatif) pour s'orienter vers la droite.
        {

            transform.rotation *= Quaternion.Lerp(
                transform.rotation,
                Quaternion.AngleAxis(-1f, Vector3.forward),
                Time.time * RotSpeedZ
                );
        }
        else if (direction.x < 0) //Rotate en Z (positif) pour s'orienter vers la gauche.
        {

            transform.rotation *= Quaternion.Lerp(
                transform.rotation,
                Quaternion.AngleAxis(1f, Vector3.forward),
                Time.time * RotSpeedZ
                );
        }

        if (direction.y > 0) //Accélération
        {
            if (currentSpeed < maxSpeed)
            {
                currentSpeed += 0.2f;
            }
        }
        else if (direction.y < 0) //Décélération
        {
            if (currentSpeed > 0)
            {
                currentSpeed -= 0.2f;

                if (currentSpeed < 0f) currentSpeed =  0f;
            }
        }
    }

    /// <summary>
    /// Faire une rotation selon le delta de la souris. haut/bas, gauche/droite
    /// </summary>
    /// <param name="rotateOffset">mouse delta en Vector2</param>
    private void RotateQuetzal(Vector2 rotateOffset)
    {
        if(rotateOffset.x > minDeltaForX)
        {
            //Ajoute une rotation positive en Y
            transform.rotation *= Quaternion.Lerp(
                transform.rotation,
                Quaternion.AngleAxis(1f, Vector3.up),
                Time.time * RotSpeedY
                );
        }
        else if (rotateOffset.x < -minDeltaForX)
        {
            //Ajoute une rotation négative en Y
            transform.rotation *= Quaternion.Lerp(
                transform.rotation,
                Quaternion.AngleAxis(-1f, Vector3.up),
                Time.time * RotSpeedY
                );
        }

        if (rotateOffset.y > minDeltaForY)
        {
            //Ajoute une rotation positive en X
            transform.rotation *= Quaternion.Lerp(
                transform.rotation,
                Quaternion.AngleAxis(1f, Vector3.right),
                Time.time * RotSpeedX
                );
        }
        else if (rotateOffset.y < -minDeltaForY)
        {
            //Ajoute une rotation négative en X
            transform.rotation *= Quaternion.Lerp(
                transform.rotation,
                Quaternion.AngleAxis(-1f, Vector3.right),
                Time.time * RotSpeedX
                );
        }
    }
}
