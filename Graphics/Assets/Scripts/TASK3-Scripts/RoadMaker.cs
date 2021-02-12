using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class RoadMaker : MonoBehaviour
{
    private float radius = 100f; // this defines the radius of the path  
    private float segments = 300f;
    private float lineWidth = 0.3f; // middle white line road marker
    private float roadWidth = 8f; // width of the road on each side of the line  
    private float edgeWidth = 1f; // widht of our road barrier at the edge of our road 
    private float edgeHeight = 1f;
    private int submeshSize = 6;
    private float wavyness = 5f;
    private float waveScale = 0.1f;
    private Vector2 waveOffset;
    private Vector2 waveStep = new Vector2(0.01f, 0.01f);

    public Vector3 randNo;
    public List<Vector3> points;

    private bool stripeCheck = true;

    public GameObject car, start, finish, startWP, finishWP;

    void Start()
    {
        GenerateRoad();

        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        meshRenderer.materials = MaterialsList().ToArray();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }


    void InstantiateCar()
    {
        if (SceneManager.GetActiveScene().name == "Level1" || SceneManager.GetActiveScene().name == "Level2" || SceneManager.GetActiveScene().name == "Level3")
        {
            if (gameObject.tag == "Track2" || gameObject.tag == "Track3")
            {
                transform.position = new Vector3(0, 0, 0);
            }
            StartWP();
            FinishWP();

            int randNo = Random.Range(0, points.Count - 1);

            car = Resources.Load("Standard Assets/Vehicles/Car/Prefabs/Car") as GameObject;
            car.transform.position = points[randNo];
            car.transform.LookAt(points[randNo+1]);
            print(randNo);
            Instantiate(car, car.transform.position + new Vector3(0f, 0.01f, 0f), car.transform.rotation * Quaternion.Euler(0f, 90f, 0f));

            //Instantiate(car, car.transform.position, Quaternion.LookRotation(new Vector3(90f, 0f, 0f)));
            startWP.transform.position = car.transform.position;
            finishWP.transform.position = startWP.transform.position + new Vector3(4f, 0.01f, 0f);
        }
    }


    public void GenerateRoad()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        MeshCollider meshCollider = this.GetComponent<MeshCollider>();
        MeshGenerator meshGen = new MeshGenerator(submeshSize);

        segments = Random.Range(200, 300);
        wavyness = Random.Range(50, 100);
        waveScale = Random.Range(3f, 7f);


        if (gameObject.tag == "Track1")
        {
            radius = 200f;
        }
        else if (gameObject.tag == "Track2")
        {
            radius = 300f;
            transform.position = new Vector3(0, 0, -800);
        }
        else if (gameObject.tag == "Track3")
        {
            radius = 400f;
            transform.position = new Vector3(0, 0, -1800);
        }

        float segmentDegrees = 360f / segments;

        
        points = new List<Vector3>();

        for (float degrees = 0; degrees < 360f; degrees += segmentDegrees)
        {
            Vector3 point = Quaternion.AngleAxis(degrees, Vector3.up) * Vector3.forward * radius;
            points.Add(point);
            randNo = points[Random.Range(0, points.Count)];
        }

        Vector2 wave = this.waveOffset;

        for (int i = 0; i < points.Count; i++)
        {
            wave += waveStep;

            Vector3 point = points[i];
            Vector3 centreDirection = point.normalized;

            float noise = Mathf.PerlinNoise(wave.x * waveScale, wave.y * waveScale);
            noise *= wavyness;

            float control = Mathf.PingPong(i, points.Count / 2f) / (points.Count / 2f);

            points[i] += centreDirection * noise * control;
        }

        for (int i = 1; i < points.Count + 1; i++)
        {
            Vector3 pPrev = points[i - 1];
            Vector3 pCurr = points[i % points.Count];
            Vector3 pNext = points[(i + 1) % points.Count];

            ExtrudeRoad(meshGen, pPrev, pCurr, pNext);
        }


        InstantiateCar();

        meshFilter.mesh = meshGen.CreateMesh();

        meshCollider.sharedMesh = meshFilter.mesh;

    }

    private void ExtrudeRoad(MeshGenerator meshGen, Vector3 pPrev, Vector3 pCurr, Vector3 pNext)
    {
        //Road Line
        Vector3 offset = Vector3.zero;
        Vector3 targetOffset = Vector3.forward * lineWidth;

        MakeRoadQuad(meshGen, pPrev, pCurr, pNext, offset, targetOffset, 0);

        //Road
        offset += targetOffset;
        targetOffset = Vector3.forward * roadWidth;

        MakeRoadQuad(meshGen, pPrev, pCurr, pNext, offset, targetOffset, 1);

        int stripeSubmesh = 2;

        if (stripeCheck)
        {
            stripeSubmesh = 3;
        }

        stripeCheck = !stripeCheck;

        //Edge
        offset += targetOffset;
        targetOffset = Vector3.up * edgeHeight;

        MakeRoadQuad(meshGen, pPrev, pCurr, pNext, offset, targetOffset, stripeSubmesh);

        offset += targetOffset;
        targetOffset = Vector3.forward * edgeWidth;

        MakeRoadQuad(meshGen, pPrev, pCurr, pNext, offset, targetOffset, stripeSubmesh);

        offset += targetOffset;
        targetOffset = -Vector3.up * edgeHeight;

        MakeRoadQuad(meshGen, pPrev, pCurr, pNext, offset, targetOffset, stripeSubmesh);

    }

    //4. Create each quad
    private void MakeRoadQuad(MeshGenerator meshGen, Vector3 pPrev, Vector3 pCurr, Vector3 pNext,
                              Vector3 offset, Vector3 targetOffset, int submesh)
    {
        Vector3 forward = (pNext - pCurr).normalized;
        Vector3 forwardPrev = (pCurr - pPrev).normalized;

        //Build Outer Track
        Quaternion perp = Quaternion.LookRotation(Vector3.Cross(forward, Vector3.up));
        Quaternion perpPrev = Quaternion.LookRotation(Vector3.Cross(forwardPrev, Vector3.up));

        Vector3 topLeft = pCurr + (perpPrev * offset);
        Vector3 topRight = pCurr + (perpPrev * (offset + targetOffset));

        Vector3 bottomLeft = pNext + (perp * offset);
        Vector3 bottomRight = pNext + (perp * (offset + targetOffset));

        meshGen.BuildTriangle(topLeft, topRight, bottomLeft, submesh);
        meshGen.BuildTriangle(topRight, bottomRight, bottomLeft, submesh);

        //Build Inner Track
        perp = Quaternion.LookRotation(Vector3.Cross(-forward, Vector3.up));
        perpPrev = Quaternion.LookRotation(Vector3.Cross(-forwardPrev, Vector3.up));

        topLeft = pCurr + (perpPrev * offset);
        topRight = pCurr + (perpPrev * (offset + targetOffset));

        bottomLeft = pNext + (perp * offset);
        bottomRight = pNext + (perp * (offset + targetOffset));

        meshGen.BuildTriangle(bottomLeft, bottomRight, topLeft, submesh);
        meshGen.BuildTriangle(bottomRight, topRight, topLeft, submesh);
    }



    public void StartWP()
    {
        startWP = new GameObject("StartWaypoint");
        startWP.AddComponent<BoxCollider>();
        startWP.GetComponent<BoxCollider>().size = new Vector3(0.1f, 3f, 100f);
        startWP.GetComponent<BoxCollider>().center = new Vector3(5f, 0f, 6f);

        start = Resources.Load("Prefabs/start-banner") as GameObject;
        //start.transform.position = car.transform.position;
        //Instantiate(start, start.transform.position, start.transform.rotation);
    }

    public void FinishWP()
    {
        finishWP = new GameObject("FinishWaypoint");
        finishWP.tag = "Finish";
        finishWP.AddComponent<FinishLine>();
        finishWP.AddComponent<BoxCollider>();
        finishWP.GetComponent<BoxCollider>().size = new Vector3(0.1f, 1f, 100f);
        finishWP.GetComponent<BoxCollider>().center = new Vector3(5f, 0f, 0f);
        finishWP.GetComponent<BoxCollider>().isTrigger = true;

        finish = Resources.Load("Prefabs/finish-banner") as GameObject;
        //finish.transform.position = finishWP.transform.position;
        //Instantiate(finish, finish.transform.position, finish.transform.rotation);
    }


    public List<Material> MaterialsList()
    {
        List<Material> materialsList = new List<Material>();

        Material redMat = new Material(Shader.Find("Specular"));
        redMat.color = Color.red;

        Material greenMat = new Material(Shader.Find("Specular"));
        greenMat.color = Color.green;

        Material blueMat = new Material(Shader.Find("Specular"));
        blueMat.color = Color.blue;

        Material yellowMat = new Material(Shader.Find("Specular"));
        yellowMat.color = Color.yellow;

        Material magentaMat = new Material(Shader.Find("Specular"));
        magentaMat.color = Color.magenta;

        Material whiteMat = new Material(Shader.Find("Specular"));
        whiteMat.color = Color.white;

        Material blackMat = new Material(Shader.Find("Specular"));
        blackMat.color = Color.black;

        Material cyanMat = new Material(Shader.Find("Specular"));
        cyanMat.color = Color.cyan;


        if (gameObject.tag == "Track1")
        {
            materialsList.Add(whiteMat);
            materialsList.Add(blackMat);
            materialsList.Add(blueMat);
            materialsList.Add(yellowMat);
        }
        else if (gameObject.tag == "Track2")
        {
            materialsList.Add(whiteMat);
            materialsList.Add(blackMat);
            materialsList.Add(greenMat);
            materialsList.Add(magentaMat);
        }
        else if (gameObject.tag == "Track3")
        {
            materialsList.Add(whiteMat);
            materialsList.Add(blackMat);
            materialsList.Add(redMat);
            materialsList.Add(cyanMat);
        }

        return materialsList;
    }
}