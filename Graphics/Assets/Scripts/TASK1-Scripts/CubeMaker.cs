using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class CubeMaker : MonoBehaviour
{
    [SerializeField]
    public Vector3 size = Vector3.one;

    [SerializeField]
    public int meshSize = 6; 

    private List<Material> matList;
    private Color[] colors = { Color.green, Color.red, Color.white, Color.black, Color.cyan, Color.grey };


    void Start()
    {
        gameObject.AddComponent<BoxCollider>();
        AddMaterials(5);
    }

    void Update()
    {
        GenerateCube();
    }

    public void GenerateCube()
    {
        //1. Initialise MeshFilter
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();

        //2. Initialise MeshBuilder
        MeshGenerator meshGen = new MeshGenerator(meshSize); //number of submeshes, 1 submesh to each face = 6 new materials

        //4. Build our cube - deifne our 8 points
        //  top vertices
        Vector3 t0 = new Vector3(size.x, size.y, -size.z); //top left point
        Vector3 t1 = new Vector3(-size.x, size.y, -size.z); //top right point
        Vector3 t2 = new Vector3(-size.x, size.y, size.z); //bottom right point (of top square)
        Vector3 t3 = new Vector3(size.x, size.y, size.z); //bottom left point (of top square)

        //  bottom vertices
        Vector3 b0 = new Vector3(size.x, -size.y, -size.z); //bottom left point
        Vector3 b1 = new Vector3(-size.x, -size.y, -size.z); //bottom right point
        Vector3 b2 = new Vector3(-size.x, -size.y, size.z); //bottom right point (of bottom square)
        Vector3 b3 = new Vector3(size.x, -size.y, size.z); //bottom left point (of bottom square)

        //top square
        //   2 triangles
        meshGen.BuildTriangle(t0, t1, t2, 0);  //0 is submesh
        meshGen.BuildTriangle(t0, t2, t3, 0);  //1  .. to view triangles (with meshSize 12)

        //bottom square
        meshGen.BuildTriangle(b2, b1, b0, 1);  //2 
        meshGen.BuildTriangle(b3, b2, b0, 1);  //3

        //back square
        meshGen.BuildTriangle(b0, t1, t0, 2);  //4
        meshGen.BuildTriangle(b0, b1, t1, 2);  //5

        //left-side square
        meshGen.BuildTriangle(b1, t2, t1, 3);  //6
        meshGen.BuildTriangle(b1, b2, t2, 3);  //7

        //right-side square
        meshGen.BuildTriangle(b2, t3, t2, 4);
        meshGen.BuildTriangle(b2, b3, t3, 4);

        //front square
        meshGen.BuildTriangle(b3, t0, t3, 5);
        meshGen.BuildTriangle(b3, b0, t0, 5);

        //3. Set teh mesh filter's mesh to the mesh generated from our mesh builder
        meshFilter.mesh = meshGen.CreateMesh();
    }


    public void AddMaterials(int colourPicked)
    {
        matList = new List<Material>();
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();

        //5. Assign Materials to the mesh renderer's materials list
        for (int faceCount = 0; faceCount < meshSize; faceCount++)
        {
            Material matColor = new Material(Shader.Find("Specular"));
            matColor.color = colors[colourPicked];
            matList.Add(matColor);
            meshRenderer.materials = matList.ToArray();
        }
    }
}
