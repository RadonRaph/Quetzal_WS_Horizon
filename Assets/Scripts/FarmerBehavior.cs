using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FarmerBehavior : MonoBehaviour
{
    public Animator farmerAnimator;
    Coroutine lookForQuetzal;

    Transform quetzal;
    public bool quetzalFinded;
    bool grounded;

    [Header("Base farmer status")]
    public bool working;

    // Start is called before the first frame update
    void Start()
    {
        AssignQuetzal();
        if (working)
        {
            farmerAnimator.SetBool("Working", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (quetzalFinded)
        {
            if (Vector3.Distance(quetzal.position, transform.position) < 30f)
            {
                //Debug.Log("near farmer");
                if (working)
                {
                    farmerAnimator.SetTrigger("Sign");
                }
                else
                {
                    if (!grounded)
                    {
                        farmerAnimator.SetBool("OnGround", true);
                        grounded = true;
                        farmerAnimator.SetTrigger("Suprised");

                        DOVirtual.DelayedCall(5f, () =>
                        {
                            farmerAnimator.SetBool("OnGround", false);
                            grounded = false;
                        });
                    }
                }
            }
        }

        
    }

    IEnumerator LookingForQuetzal()
    {
        while (true)
        {
            AssignQuetzal();
            yield return new WaitForSecondsRealtime(10f);
        }
    }

    void AssignQuetzal()
    {

        Transform quetzalTransform = GameObject.FindWithTag("Quetzal").transform;
        
        if (quetzalTransform == null)
        {
            if(lookForQuetzal == null)
            {
                lookForQuetzal = StartCoroutine(LookingForQuetzal());
                //Debug.Log("Coroutine started");
            }
        }
        else
        {
            quetzal = quetzalTransform;
            quetzalFinded = true;
            if (lookForQuetzal != null)
            {
                StopCoroutine(lookForQuetzal);
                //Debug.Log("Coroutine stoped");
            }
        }
        
    }
}
