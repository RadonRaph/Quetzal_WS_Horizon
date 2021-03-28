using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWhenTriggerWIthTag : MonoBehaviour
{
    public string Tag;
    public GameObject prefab;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Heho");

            Instantiate(prefab, transform.position+offset, Quaternion.identity);
        
    }
}
