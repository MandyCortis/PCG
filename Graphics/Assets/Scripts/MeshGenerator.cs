using System.Collections.Generic;
using UnityEngine;

//HELPER CLASS
public class MeshGenerator
{
    private List<Vector3> vertices = new List<Vector3>(); //list of vertices - store our points in our mesh
    private List<int> indices = new List<int>(); //list of indices that point to the index location in our vertices list
    private List<int>[] submeshIndices = new List<int>[] { }; //an array of submesh indices

    public MeshGenerator(int submeshCount) //defines how many submeshes we have
    {
        //initialise our array by the amount of submeshes we have
        submeshIndices = new List<int>[submeshCount];

        for(int i = 0; i < submeshCount; i++)
        {
            submeshIndices[i] = new List<int>();
        }
    }

    public void BuildTriangle(Vector3 p0, Vector3 p1, Vector3 p2, int subMesh)
    {
        for (int i = 0; i < 3; i++)
        {
            // index of each vertex within the list of vertices
            indices.Add(vertices.Count + 1);
            submeshIndices[subMesh].Add(vertices.Count + i);
        }

        // add each point to our vertices list
        vertices.Add(p0);
        vertices.Add(p1);
        vertices.Add(p2);
    }


    public Mesh CreateMesh()  //build our mesh
    {
        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.subMeshCount = submeshIndices.Length;

        for(int i = 0; i < submeshIndices.Length; i++)
        {
            if(submeshIndices[i].Count < 3) //instead of unity crashing or generating bunch of errors, generate a triangle
            {
                mesh.SetTriangles(new int[3] {0,0,0}, i);
            }
            else
            {
                mesh.SetTriangles(submeshIndices[i].ToArray(), i);
            }
        }
        mesh.RecalculateNormals(); //function inside Unity
        return mesh;
    }
}
