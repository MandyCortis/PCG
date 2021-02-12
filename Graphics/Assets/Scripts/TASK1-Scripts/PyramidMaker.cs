using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class PyramidMaker : MonoBehaviour
{
    [SerializeField]
    private float pyramidSize = 5f;
    public int meshSize = 4;

    private List<Material> matList;
    private Color[] colors = { Color.green, Color.red, Color.white, Color.black, Color.cyan, Color.grey };

    public void Start()
    {
        gameObject.AddComponent<BoxCollider>();
        gameObject.tag = "Pyramid";

        AddMaterials(0);
    }


    void Update()
    {
        GeneratePyramid();
    }

    public void GeneratePyramid()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        MeshGenerator meshGen = new MeshGenerator(meshSize);

        //Add points
        Vector3 top = new Vector3(0, pyramidSize, 0);

        Vector3 base0 = Quaternion.AngleAxis(0f, Vector3.up) * Vector3.forward * pyramidSize; //an angle/rotation form our top point (height 5) //rotating aroudn the y axis

        Vector3 base1 = Quaternion.AngleAxis(240f, Vector3.up) * Vector3.forward * pyramidSize;

        Vector3 base2 = Quaternion.AngleAxis(120f, Vector3.up) * Vector3.forward * pyramidSize;


        //Build the triangles for our pyramid
        meshGen.BuildTriangle(base0, base1, base2, 0);

        meshGen.BuildTriangle(base1, base0, top, 1);

        meshGen.BuildTriangle(base2, top, base0, 2);

        meshGen.BuildTriangle(top, base2, base1, 3);

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

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }

}
