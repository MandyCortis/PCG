using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class TerrainTextureData
{
    public Texture2D terrainTexture;
    public float minHeight;
    public float maxHeight;
    public Vector2 tileSize;
}

[System.Serializable]
class TreeData
{
    public GameObject treeMesh;
    public float minHeight;
    public float maxHeight;
}

public class LoadHeightmapImage : MonoBehaviour
{
    //TERRAIN
    private TerrainData terrainData;
    [SerializeField] private List<TerrainTextureData> terrainTextureDataList;
    [SerializeField] private float terrainTextureBlendOffset = 0.01f;
    [SerializeField] private Vector3 heightMapScale = new Vector3(1, 1, 1);
    [SerializeField] private Texture2D[] heightMapImages;
    private Texture2D hmImage;
    public int chosenMap;


    //TREES
    [SerializeField] private List<TreeData> treeDataList;
    [SerializeField] private int maxTrees = 2000;
    [SerializeField] private int treeSpacing = 10;
    [SerializeField]  private float randomXRange = 5.0f;
    [SerializeField] private float randomZRange = 5.0f;
    [SerializeField] private int terrainLayerIndex = 8;


    //WATER
    [SerializeField] GameObject water;
    [SerializeField] float waterHeight = 0.1f;


    //CLOUDS & FOG
    [SerializeField] Transform cloudPrnt, fogPrnt;
    [SerializeField] List<GameObject> cloud, fog;
    [SerializeField] int cloudAmount, fogAmount;
    float x, y, z;


    //PLAYER
    [SerializeField] GameObject player;
    float xPos, yPos, zPos;


    void Awake()
    {
        terrainData = Terrain.activeTerrain.terrainData;

        chosenMap = Random.Range(0, heightMapImages.Length);
        hmImage = heightMapImages[chosenMap];

        UpdateHeightmap();
    }

    private void Start()
    {
        TerrainTexture();
        AddTrees();
        AddClouds();
        AddFog();
        Player();
    }

    void UpdateHeightmap()
    {
        AddWater();
        //creates a new empty 2D array of float based on the dimensions of heightmap resolution set in the settings
        //float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

        //gets the height map data that already exists in the terrain and loads it into a 2D array
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
                    heightMap[width, height] = hmImage.GetPixel((int)(width * heightMapScale.x),
                                                                       (int)(height * heightMapScale.z)).grayscale
                                                                       * heightMapScale.y;
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }

    void TerrainTexture()
    {
        TerrainLayer[] terrainLayers = new TerrainLayer[terrainTextureDataList.Count];

        for (int i = 0; i < terrainTextureDataList.Count; i++)
        {

            terrainLayers[i] = new TerrainLayer();
            terrainLayers[i].diffuseTexture = terrainTextureDataList[i].terrainTexture;
            terrainLayers[i].tileSize = terrainTextureDataList[i].tileSize;


        }

        terrainData.terrainLayers = terrainLayers;


        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        float[,,] alphaMapList = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int height = 0; height < terrainData.alphamapHeight; height++)
        {
            for (int width = 0; width < terrainData.alphamapWidth; width++)
            {
                float[] splatmap = new float[terrainData.alphamapLayers];

                for (int i = 0; i < terrainTextureDataList.Count; i++)
                {
                    float minHeight = terrainTextureDataList[i].minHeight - terrainTextureBlendOffset;
                    float maxHeight = terrainTextureDataList[i].maxHeight + terrainTextureBlendOffset;

                    if (heightMap[width, height] >= minHeight && heightMap[width, height] <= maxHeight)
                    {
                        splatmap[i] = 1;
                    }
                }

                NormaliseSplatMap(splatmap);

                for (int j = 0; j < terrainTextureDataList.Count; j++)
                {
                    alphaMapList[width, height, j] = splatmap[j];
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, alphaMapList);
    }


    void NormaliseSplatMap(float[] splatmap)
    {
        float total = 0;

        for (int i = 0; i < splatmap.Length; i++)
        {
            total += splatmap[i];
        }

        for (int i = 0; i < splatmap.Length; i++)
        {
            splatmap[i] = splatmap[i] / total;
        }
    }



    void AddTrees()
    {
        TreePrototype[] trees = new TreePrototype[treeDataList.Count];

        for (int i = 0; i < treeDataList.Count; i++)
        {
            trees[i] = new TreePrototype();
            trees[i].prefab = treeDataList[i].treeMesh;
        }

        terrainData.treePrototypes = trees;

        List<TreeInstance> treeInstanceList = new List<TreeInstance>();


        for (int z = 0; z < terrainData.size.z; z += treeSpacing)
        {
            for (int x = 0; x < terrainData.size.x; x += treeSpacing)
            {
                for (int treePrototypeIndex = 0; treePrototypeIndex < trees.Length; treePrototypeIndex++)
                {
                    if (treeInstanceList.Count < maxTrees)
                    {
                        float currentHeight = terrainData.GetHeight(x, z) / terrainData.size.y;

                        if (currentHeight >= treeDataList[treePrototypeIndex].minHeight &&
                           currentHeight <= treeDataList[treePrototypeIndex].maxHeight)
                        {
                            float randomX = (x + Random.Range(-randomXRange, randomXRange)) / terrainData.size.x;
                            float randomZ = (z + Random.Range(-randomZRange, randomZRange)) / terrainData.size.z;

                            TreeInstance treeInstance = new TreeInstance();

                            treeInstance.position = new Vector3(randomX, currentHeight, randomZ);

                            Vector3 treePosition = new Vector3(treeInstance.position.x * terrainData.size.x,
                                                                treeInstance.position.y * terrainData.size.y,
                                                                treeInstance.position.z * terrainData.size.z) + this.transform.position;



                            RaycastHit raycastHit;
                            int layerMask = 1 << terrainLayerIndex;

                            if (Physics.Raycast(treePosition, Vector3.down, out raycastHit, 100, layerMask) ||
                                Physics.Raycast(treePosition, Vector3.up, out raycastHit, 100, layerMask))
                            {
                                float treeHeight = (raycastHit.point.y - this.transform.position.y) / terrainData.size.y;

                                treeInstance.position = new Vector3(treeInstance.position.x, treeHeight, treeInstance.position.z);

                                treeInstance.rotation = Random.Range(0, 360);
                                treeInstance.prototypeIndex = treePrototypeIndex;
                                treeInstance.color = Color.white;
                                treeInstance.lightmapColor = Color.white;
                                treeInstance.heightScale = 0.95f;
                                treeInstance.widthScale = 0.95f;

                                treeInstanceList.Add(treeInstance);
                            }
                        }
                    }
                }
            }
        }


        terrainData.treeInstances = treeInstanceList.ToArray();
    }


    void AddWater()
    {
        waterHeight = Random.Range(0f, 0.001f);

        Vector3 waterLoc = this.transform.position + new Vector3(
            terrainData.size.x / 2,
            waterHeight * terrainData.size.y,
            terrainData.size.z / 2);

        
        GameObject waterObj = Instantiate(water, waterLoc, Quaternion.identity);
        waterObj.transform.localScale = new Vector3(terrainData.size.x, 1, terrainData.size.z);
    }


    void AddClouds()
    {
        cloudAmount = Random.Range(5, 30);
        for (int i = 0; i < cloudAmount; i++)
        {
            x = Random.Range(transform.position.x, terrainData.size.x);
            y = terrainData.size.y;
            z = Random.Range(transform.position.x, terrainData.size.z);

            GameObject cloudObj = Instantiate(cloud[Random.Range(0, cloud.Count - 1)], new Vector3(x, y, z), Quaternion.identity);
            cloudObj.name = "Cloud";
            cloudObj.transform.parent = cloudPrnt;
        }
    }


    void AddFog()
    {
        fogAmount = Random.Range(5, 30);
        for (int i = 0; i < fogAmount; i++)
        {
            x = Random.Range(transform.position.x, terrainData.size.x);
            y = Random.Range(5f, terrainData.size.y - 300f);
            z = Random.Range(transform.position.x, terrainData.size.z);

            GameObject fogObj = Instantiate(fog[Random.Range(0, fog.Count - 1)], new Vector3(x, y, z), Quaternion.identity);
            fogObj.name = "Fog";
            fogObj.transform.parent = fogPrnt;
        }
    }


    void Player()
    {
        xPos = Random.Range(0, terrainData.size.x);
        yPos = Random.Range(200, terrainData.size.y);
        zPos = Random.Range(0, terrainData.size.z);

        Vector3 playerPos = new Vector3(xPos, yPos, zPos);
        Instantiate(player, playerPos, Quaternion.identity);
    }
}
