using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]


public class Maze : MonoBehaviour
{
    private Vector3 scaleChange1, scaleChange2, scaleChange3, scaleChange4, scaleChange5, scaleChange6, scaleChange7, scaleChange8, scaleChange9, scaleChange10, scaleChange11, scaleChange12;

    List<Vector3> startList = new List<Vector3>();
    List<Vector3> finishList = new List<Vector3>();

    Vector3 start1 = new Vector3(3.5f, 0, 3.5f);
    Vector3 start2 = new Vector3(18.5f, 0, 19.5f);
    Vector3 finish1 = new Vector3(19f, 0, 4f);
    Vector3 finish2 = new Vector3(4f, 0, 19);

    GameObject cube, pyramidStart, pyramidFinish;


    public void Awake()
    {
        scaleChange1 = new Vector3(0f, 0f, 10.5f);
        scaleChange2 = new Vector3(9.5f, 0f, 0f);
        scaleChange3 = new Vector3(4f, 0f, -0.5f);
        scaleChange4 = new Vector3(1.5f, 0f, -0.5f);
        scaleChange5 = new Vector3(-0.5f, 0f, 3f);
        scaleChange6 = new Vector3(0.5f, 0f, -0.5f);
        scaleChange7 = new Vector3(-0.5f, 0f, 3f);
        scaleChange8 = new Vector3(1f, 0f, -0.5f);
        scaleChange9 = new Vector3(-0.5f, 0f, 2.5f);
        scaleChange10 = new Vector3(1.5f, 0f, -0.5f);
    }


    public void Start()
    {
        GenerateMaze();
        GenerateStart();
        GenerateFinish();

        cube.AddComponent<Rigidbody>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            cube.transform.position -= new Vector3(1f, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            cube.transform.position += new Vector3(1f, 0);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            cube.transform.position += new Vector3(0, 0, 1f);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            cube.transform.position -= new Vector3(0, 0, 1f);
        }
    }


    void GenerateMaze()
    {     
        //PLANE
        GameObject plane = Instantiate(new GameObject("Plane"), Vector3.zero, Quaternion.identity);
        plane.AddComponent<PlaneMaker>();
        plane.GetComponent<PlaneMaker>().AddMaterials(2);
        plane.GetComponent<PlaneMaker>().GeneratePlane();

        //WALLS
        GameObject wallA = Instantiate(new GameObject("WallA"), new Vector3(1f, 1, 11.5f), Quaternion.identity);
        wallA.AddComponent<CubeMaker>();
        wallA.GetComponent<CubeMaker>().AddMaterials(4);
        wallA.GetComponent<CubeMaker>().GenerateCube();
        wallA.transform.localScale += scaleChange1;

        GameObject wallB = Instantiate(new GameObject("WallB"), new Vector3(22f, 1f, 11.5f), Quaternion.identity);
        wallB.AddComponent<CubeMaker>();
        wallB.GetComponent<CubeMaker>().AddMaterials(4);
        wallB.GetComponent<CubeMaker>().GenerateCube();
        wallB.transform.localScale += scaleChange1;

        GameObject wallC = Instantiate(new GameObject("WallC"), new Vector3(10.5f, 1f, 1f), Quaternion.identity);
        wallC.AddComponent<CubeMaker>();
        wallC.GetComponent<CubeMaker>().AddMaterials(4);
        wallC.GetComponent<CubeMaker>().GenerateCube();
        wallC.transform.localScale += scaleChange2;

        GameObject wallD = Instantiate(new GameObject("WallD"), new Vector3(10.5f, 1f, 22f), Quaternion.identity);
        wallD.AddComponent<CubeMaker>();
        wallD.GetComponent<CubeMaker>().AddMaterials(4);
        wallD.GetComponent<CubeMaker>().GenerateCube();
        wallD.transform.localScale += scaleChange2;

        GameObject wallE = Instantiate(new GameObject("WallE"), new Vector3(7f, 1f, 6.5f), Quaternion.identity);
        wallE.AddComponent<CubeMaker>();
        wallE.GetComponent<CubeMaker>().AddMaterials(4);
        wallE.GetComponent<CubeMaker>().GenerateCube();
        wallE.transform.localScale += scaleChange3;

        GameObject wallF = Instantiate(new GameObject("WallF"), new Vector3(18.5f, 1f, 6.5f), Quaternion.identity);
        wallF.AddComponent<CubeMaker>();
        wallF.GetComponent<CubeMaker>().AddMaterials(4);
        wallF.GetComponent<CubeMaker>().GenerateCube();
        wallF.transform.localScale += scaleChange4;

        GameObject wallG = Instantiate(new GameObject("WallG"), new Vector3(12.5f, 1f, 10f), Quaternion.identity);
        wallG.AddComponent<CubeMaker>();
        wallG.GetComponent<CubeMaker>().AddMaterials(4);
        wallG.GetComponent<CubeMaker>().GenerateCube();
        wallG.transform.localScale += scaleChange5;

        GameObject wallH = Instantiate(new GameObject("WallH"), new Vector3(14.5f, 1f, 10.5f), Quaternion.identity);
        wallH.AddComponent<CubeMaker>();
        wallH.GetComponent<CubeMaker>().AddMaterials(4);
        wallH.GetComponent<CubeMaker>().GenerateCube();
        wallH.transform.localScale += scaleChange6;

        GameObject wallI = Instantiate(new GameObject("WallI"), new Vector3(16.5f, 1f, 17f), Quaternion.identity);
        wallI.AddComponent<CubeMaker>();
        wallI.GetComponent<CubeMaker>().AddMaterials(4);
        wallI.GetComponent<CubeMaker>().GenerateCube();
        wallI.transform.localScale += scaleChange7;

        GameObject wallJ = Instantiate(new GameObject("wallJ"), new Vector3(14f, 1f, 17.5f), Quaternion.identity);
        wallJ.AddComponent<CubeMaker>();
        wallJ.GetComponent<CubeMaker>().AddMaterials(4);
        wallJ.GetComponent<CubeMaker>().GenerateCube();
        wallJ.transform.localScale += scaleChange8;

        GameObject wallK = Instantiate(new GameObject("wallK"), new Vector3(7.5f, 1f, 13.5f), Quaternion.identity);
        wallK.AddComponent<CubeMaker>();
        wallK.GetComponent<CubeMaker>().AddMaterials(4);
        wallK.GetComponent<CubeMaker>().GenerateCube();
        wallK.transform.localScale += scaleChange9;

        GameObject wallL = Instantiate(new GameObject("wallL"), new Vector3(4.5f, 1f, 16.5f), Quaternion.identity);
        wallL.AddComponent<CubeMaker>();
        wallL.GetComponent<CubeMaker>().AddMaterials(4);
        wallL.GetComponent<CubeMaker>().GenerateCube();
        wallL.transform.localScale += scaleChange10;
    }


    void GeneratePlayer()
    {
        cube.AddComponent<CubeMaker>();
        cube.GetComponent<CubeMaker>().AddMaterials(3);
        cube.GetComponent<CubeMaker>().GenerateCube();
        cube.tag = "Player";
        
        cube.transform.localScale += new Vector3(-0.5f, -0.5f, -0.5f);
    }

    void GenerateStart()
    {
        startList.Add(start1);
        startList.Add(start2);

        //PYRAMID
        Vector3 position = startList[Random.Range(0,2)];
        pyramidStart = Instantiate(new GameObject("Start"), position, Quaternion.identity);
        pyramidStart.AddComponent<PyramidMaker>();
        pyramidStart.GetComponent<PyramidMaker>().AddMaterials(1);
        pyramidStart.GetComponent<PyramidMaker>().GeneratePyramid();
        pyramidStart.transform.localScale += new Vector3(-0.8f, -0.8f, -0.8f);

        
        cube = Instantiate(new GameObject("Player"), position, Quaternion.identity);

        if (position == startList[0])
        {
            cube.transform.position = new Vector3(position.x + 1, 0.5f, position.z);
            pyramidStart.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            cube.transform.position = new Vector3(position.x, 0.5f, position.z - 1);
        }
        GeneratePlayer();
    }

    void GenerateFinish()
    {
        finishList.Add(finish1);
        finishList.Add(finish2);

        //PYRAMID
        Vector3 position = finishList[Random.Range(0, 2)];
        pyramidFinish = Instantiate(new GameObject("Finish"), position, Quaternion.identity);
        pyramidFinish.AddComponent<PyramidMaker>();
        pyramidFinish.GetComponent<PyramidMaker>().AddMaterials(0);
        pyramidFinish.GetComponent<PyramidMaker>().GeneratePyramid();
        pyramidFinish.AddComponent<MeshCollider>();
        pyramidFinish.GetComponent<MeshCollider>().convex = true;
        pyramidFinish.GetComponent<MeshCollider>().isTrigger = true;

        pyramidFinish.tag = "Finish";
        pyramidFinish.transform.localScale += new Vector3(-0.6f, -0.6f, -0.6f);

        if (position == finishList[0])
        {
            pyramidFinish.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            pyramidFinish.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }
}
