using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class GenerateTree : MonoBehaviour {

    public int size_x = 50;
    public int size_y = 100;
    public float treesize = 1.0f;
    public int TreeResolution = 16;
    public Sprite[] trees;
    List<Vector2[]> tilep = new List<Vector2[]>();
    public Grid_creator grid;
    int TexInRow = 0;
    int TexNumRows = 0;
    int TexNum = 0;
    // Use this for initialization
    void Start()
    {
        BuildMesh();
    }

    Vector2 GetTexture(int num, int x, int y)
    {
        if (tilep.Count > num)
        {
            Vector2 t = new Vector2(tilep[num][x % 2].x, tilep[num][y % 2].y);
            return t;
        }
        return Vector2.zero;
    }

    void CalcTex()
    {
        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        if (trees.Length > 0)
            foreach (Sprite s in trees)
            {
                Vector2[] add = new Vector2[2];
                add[0].x = s.textureRect.x / s.texture.width;
                add[0].y = s.textureRect.y / s.texture.height;
                add[1].x = s.textureRect.xMax / s.texture.width;
                add[1].y = s.textureRect.yMax / s.texture.height;
                tilep.Add(add);
                mesh_renderer.material.mainTexture = s.texture;
            }


    }

    // Update is called once per frame
    public void BuildMesh()
    {
        CalcTex();

        //int numtrees = size_x * size_y;
        //int numTris = numtrees * 2;
        //int vsize_x = size_x * 2;
        //int vsize_y = size_y * 2;
        //int numVerts = vsize_x * vsize_y;

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uv = new Vector2[4];

        int[] triangles = new int[2 * 3];

        int x, y;

        Vector3 sp = Vector3.zero;
        sp.x -= treesize * size_x / 2;
        sp.y -= treesize * size_x / 2;

        float treew = trees[0].textureRect.width;
        float treeh = trees[0].textureRect.height;


        vertices[0] = new Vector3(0 * treesize, 0 * treesize, 0);
        vertices[1] = new Vector3(treew * treesize, 0 * treesize, 0);
        vertices[2] = new Vector3(0 * treesize, treeh * treesize, 0);
        vertices[3] = new Vector3(treew * treesize, treeh * treesize, 0);
        normals[0] = new Vector3(0, 0, -1);
        normals[1] = new Vector3(0, 0, -1);
        normals[2] = new Vector3(0, 0, -1);
        normals[3] = new Vector3(0, 0, -1);
        uv[0] = GetTexture(0, 0, 0);
        uv[1] = GetTexture(0, 1, 0);
        uv[2] = GetTexture(0, 0, 1);
        uv[3] = GetTexture(0, 1, 1);


        triangles[0] = 0;
        triangles[2] = 3;
        triangles[1] = 2;

        triangles[3] = 0;
        triangles[5] = 1;
        triangles[4] = 3;

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = triangles;

        MeshFilter mesh_filter = GetComponent<MeshFilter>();
        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        MeshCollider mesh_collider = GetComponent<MeshCollider>();

        mesh_filter.mesh = mesh;
        mesh_collider.sharedMesh = mesh;
        Debug.Log("Done Mesh!");
        //BuildTexture();
    }
}
