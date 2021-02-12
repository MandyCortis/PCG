using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class PlaneMaker : MonoBehaviour
{
    [SerializeField]
    public float cellSize = 1f;

    [SerializeField]
    public int width = 24;

    [SerializeField]
    public int height = 24;

    [SerializeField]
    public int subMeshSize = 6;

    private List<Material> matList;
    public Color[] colors = { Color.green, Color.red, Color.white, Color.black, Color.cyan, Color.grey };

    Collider planeCollider;

    private void Start()
    {
        this.gameObject.AddComponent<BoxCollider>();
        planeCollider = GetComponent<Collider>();
        this.gameObject.tag = "Plane";

        AddMaterials(1);
    }


    // Update is called once per frame
    void Update()
    {
        GeneratePlane();
    }

    public void GeneratePlane()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        MeshGenerator meshGen = new MeshGenerator(subMeshSize);

        
        //create points of our plane
        Vector3[,] points = new Vector3[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                points[x, y] = new Vector3(
                    cellSize * x,   //x
                    0,              //y
                    cellSize * y    //z
                );
            }
        }

        //create the quads
        int submesh = 0;

        for (int x = 0; x < width - 1/*prevents out of bounds*/; x++)
        {
            for (int y = 0; y < height - 1; y++)
            { //each quad is made up of 4 points, so we need 4 points
                Vector3 br = points[x, y];
                Vector3 bl = points[x + 1, y];
                Vector3 tr = points[x, y + 1];
                Vector3 tl = points[x + 1, y + 1];

                //create 2 triangles that make up a quad
                meshGen.BuildTriangle(bl, tr, tl, submesh % subMeshSize);  //gives a remainder between 0 and 5, once it arrives at the 5th location, its starts from 0 again
                meshGen.BuildTriangle(bl, br, tr, submesh % subMeshSize);
            }
            submesh++;
        }
        meshFilter.mesh = meshGen.CreateMesh();
    }



    public void AddMaterials(int colourPicked)
    {
        matList = new List<Material>();
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();

        //5. Assign Materials to the mesh renderer's materials list
        for (int faceCount = 0; faceCount < subMeshSize; faceCount++)
        {
            Material matColor = new Material(Shader.Find("Specular"));
            matColor.color = colors[colourPicked];
            matList.Add(matColor);
            meshRenderer.materials = matList.ToArray();
        }
    }
}

