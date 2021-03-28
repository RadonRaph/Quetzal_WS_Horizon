using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LineFromtransform : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public bool DrawGizmos;

    private void OnDrawGizmos()
    {
        if (!DrawGizmos)
            return;

        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.GetChild(i).position, 0.5f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            lineRenderer.positionCount = childCount;
            lineRenderer.SetPosition(i, transform.GetChild(i).position);
        }        
    }
}
