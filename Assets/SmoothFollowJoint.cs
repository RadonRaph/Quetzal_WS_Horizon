using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowJoint : MonoBehaviour
{
    public Transform target;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = target.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position-target.InverseTransformDirection(offset), 0.2f);
    }
}
