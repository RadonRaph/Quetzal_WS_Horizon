using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SoundByDistance : MonoBehaviour
{
    public Transform player;
    public Collider collider;
    public AudioSource source;
    public float maxDistance = 50;

    public float dist;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 closestPoint = collider.ClosestPoint(player.position);
        dist = Vector3.Distance(player.position, closestPoint);


        if (dist > maxDistance)
        {
            source.mute = true;
        }
        else
        {
            source.mute = false;
            source.volume = 1 - (dist / maxDistance);
        }
    }
}
