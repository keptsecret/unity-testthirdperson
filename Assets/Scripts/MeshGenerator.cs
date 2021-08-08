using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    private Mesh _mesh;

    private Vector3[] _vertices;
    private int[] _triangles;

    public int NumRows = 100;
    public int NumColumns = 100;

    public IMapGenerator MapGenerator;

    void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        // TODO: should probably un-hard code this at some point
        MapGenerator = new MapGen_CellularAutomata(NumColumns + 1, NumRows + 1, 3, 50);
        MapGenerator.GenerateMap();

        CreateShape();
        UpdateMesh();
    }

    private void CreateShape()
    {
        _vertices = new Vector3[(NumColumns + 1) * (NumRows + 1)];
        int[,] heightMap = MapGenerator.GetMap();

        // fill in vertices and height from ca
        for (int i = 0, z = 0; z <= NumRows; z++)
        {
            for (int x = 0; x <= NumColumns; x++)
            {
                //_vertices[i] = new Vector3(x, (heightMap[x, z] == 1) ? 0 : 1, z);   // inverted heightmap
                _vertices[i] = new Vector3(x, heightMap[x, z], z);
                i++;
            }
        }

        // connect vertices with triangles
        int verts = 0;
        int tris = 0;
        _triangles = new int[NumColumns * NumRows * 6];

        for (int z = 0; z < NumRows; z++)
        {
            for (int x = 0; x < NumColumns; x++)
            {
                _triangles[tris + 0] = verts + 0;
                _triangles[tris + 1] = verts + NumColumns + 1;
                _triangles[tris + 2] = verts + 1;
                _triangles[tris + 3] = verts + 1;
                _triangles[tris + 4] = verts + NumColumns + 1;
                _triangles[tris + 5] = verts + NumColumns + 2;

                verts++;
                tris += 6;
            }

            verts++;
        }
    }

    private void UpdateMesh()
    {
        _mesh.Clear();

        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.RecalculateNormals();

        _mesh.RecalculateBounds();
        GetComponent<MeshCollider>().sharedMesh = _mesh;
    }

    /*
    void OnDrawGizmos()
    {
        if (_vertices == null) return;

        foreach (var v in _vertices)
        {
            Gizmos.DrawSphere(v, 0.1f);
        }
    }
    */
}
