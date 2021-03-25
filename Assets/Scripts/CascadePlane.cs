using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;

public class CascadePlane : MonoBehaviour
{
    public int widthVertices = 100;
    public int lengthVertices = 100;

    public float length = 10;
    public float height = 100;

     NativeArray<float> heightMax;
    public float[] h;



    NativeArray<Vector3> m_Vertices;
    NativeArray<Vector3> m_Normals;

    Vector3[] m_ModifiedVertices;
    Vector3[] m_ModifiedNormals;

    MeshModJob m_MeshModJob;
    JobHandle m_JobHandle;

    Mesh m_Mesh;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y + (height/2f), transform.position.z), new Vector3(length, height, 1));
    }


    protected void Start()
    {
        GenerateMesh();

        m_Mesh = gameObject.GetComponent<MeshFilter>().mesh;
        m_Mesh.MarkDynamic();

        // this persistent memory setup assumes our vertex count will not expand
        m_Vertices = new NativeArray<Vector3>(m_Mesh.vertices, Allocator.Persistent);
        m_Normals = new NativeArray<Vector3>(m_Mesh.normals, Allocator.Persistent);

        m_ModifiedVertices = new Vector3[m_Vertices.Length];
        m_ModifiedNormals = new Vector3[m_Vertices.Length];

        heightMax = new NativeArray<float>(widthVertices+1, Allocator.Persistent);
        h = new float[widthVertices+1];
    }

    protected void GenerateMesh()
    {
        GetComponent<MeshFilter>().mesh = m_Mesh = new Mesh();
        m_Mesh.name = "Procedural Grid";

        Vector3[] vertices = new Vector3[(widthVertices+1)*(lengthVertices+1)];
        for (int i = 0, x = 0; x <= widthVertices; x++)
        {
            for (int y = 0; y<= lengthVertices; y++, i++)
            {
                float rPosX = x / (float)widthVertices * length - (length / 2f);
                float rPosY = y / (float)lengthVertices * height;

                vertices[i] = new Vector3(rPosX, rPosY, 0);
            }
        }
        m_Mesh.vertices = vertices;

        int[] triangles = new int[(widthVertices + 1) * (lengthVertices + 1) * 6];
        for (int ti = 0, vi = 0, y = 0; y < lengthVertices; y++, vi++)
        {
            for (int x = 0; x < widthVertices; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + widthVertices + 1;
                triangles[ti + 5] = vi + widthVertices + 2;
            }
        }
        m_Mesh.triangles = triangles;
    }

    struct MeshModJob : IJobParallelFor
    {
        public NativeArray<Vector3> vertices;
        //public NativeArray<Vector3> normals;

        public int width;
        public int length;
        [ReadOnly]
        public NativeArray<float> maxHeight;


        public void Execute(int i)
        {
            var vertex = vertices[i];
            Vector2Int coord = RaphExtends.oneDtoTwoD(i, width);

            if (coord.y<maxHeight.Length)    
             vertex.y = (coord.x / (float)length) * maxHeight[coord.y];

            vertices[i] = vertex;

        }
    }


    int raycastIndex = 0;
    public void Update()
    {
        for (int i = 0; i < 10; i++)
        {


            float posX = raycastIndex / (float)widthVertices * length - (length / 2f);

            RaycastHit hit;
            if (Physics.Linecast(transform.position + new Vector3(posX, 0, 0), transform.position + new Vector3(posX, height, 0), out hit))
            {
                //Debug.DrawLine(transform.position + new Vector3(posX, 0, 0), hit.point, Color.red);
                heightMax[raycastIndex] = (hit.point - transform.position).y;
            }
            else
            {
                //Debug.DrawLine(transform.position + new Vector3(posX, 0, 0), transform.position + new Vector3(posX, height, 0), Color.green, 0.1f);
                heightMax[raycastIndex] = height;
            }

            raycastIndex++;
            if (raycastIndex >= heightMax.Length)
                raycastIndex = 0;
        }


        m_MeshModJob = new MeshModJob()
        {
            vertices = m_Vertices,
            maxHeight = heightMax,
            width = widthVertices,
            length = lengthVertices
            //normals = m_Normals
        };

        m_JobHandle = m_MeshModJob.Schedule(m_Vertices.Length, 64);

        heightMax.CopyTo(h);
       // UpdateMesh();
    }

    public void LateUpdate()
    {
        
        m_JobHandle.Complete();

        // copy our results to managed arrays so we can assign them
        m_MeshModJob.vertices.CopyTo(m_ModifiedVertices);
        //m_MeshModJob.normals.CopyTo(m_ModifiedNormals);

        m_Mesh.vertices = m_ModifiedVertices;

        m_Mesh.RecalculateNormals();
        m_Mesh.RecalculateUVDistributionMetric(0);
        //m_Mesh.normals = m_ModifiedNormals;
    }

    private void OnDisable()
    {
        m_Vertices.Dispose();
        m_Normals.Dispose();
        heightMax.Dispose();
    }

    private void OnDestroy()
    {
        // make sure to Dispose() any NativeArrays when we're done
       // m_Vertices.Dispose();
       // heightMax.Dispose();
        //m_Normals.Dispose();
    }
}
