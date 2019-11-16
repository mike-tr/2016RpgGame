using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]


 [System.Serializable]
 public class TCSV
 {
    public string tile_name;
	public Sprite texture;
    public int Chance;
	public TDTile.Properties.Terain_type tile_type;
	public bool IsWakable = true;
	public bool CanPlantsGrow = true;
    public Vector2 Height;
    public float HeightScale;
 }
public class TGMap : MonoBehaviour {
	public bool MapCreated = false;
	public static TGMap instance;
	public static Vector2 MapWorldSize = Vector2.zero;
	public static TDMap Global_TDMap;
	public int size_x = 50;
	public int size_y = 100;
	public int GenMaxAtOnce = 250;
	public float tileSize = 1.0f;
	public int tileResolution = 16;
	public Texture2D terrainTiles;
	//public Sprite[] tiles;
	Dictionary< string, Vector2[]> TileTextures = new Dictionary< string, Vector2[]>();
	public Transform MapHolder;
	public Grid_creator grid;
	public TDMap.MapType MapTerrainType = TDMap.MapType.normal_areas;
	public TCSV[] TileChance;

    public int ChanceVegetation = 80;
    public int ChanceTree = 10;
    public int MaxHeightForPlants = 10;


	//NotRlyUsed
	public int MaxHeightLeap = 2;
	int TexInRow = 0;
	int TexNumRows = 0;
	int TexNum = 0;
	public bool Heights = false;
	// Use this for initialization
	void Start () {
		BuildMesh();
	}
	
	Vector2 GetTexture(string tile_name,int x, int y)
	{
		Vector2[] p;
		if(TileTextures.TryGetValue( tile_name, out p))
		{
			Vector2 t = new Vector2( p[x % 2].x, p[y % 2].y );
			return t;
		}

		print("ERROR VRONG VALUE!");
		return Vector2.zero;
	}

	Vector2 GetTexture2(int num, int x, int y)
	{
		float ty = (num / TexInRow);
		float tx = (num % TexInRow);
		ty /= TexNumRows;
		tx /= TexInRow;	
		return new Vector2( ((float)(x % 2))/TexInRow + tx, ((float)(y % 2))/TexNumRows + ty);
	}

	void CalcTex()
	{
		TileTextures = new Dictionary< string, Vector2[] >();
		MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
		TexInRow = terrainTiles.width/tileResolution;
		TexNumRows = terrainTiles.height/tileResolution;
		TexNum = TexInRow * TexNumRows;

		foreach(TCSV tc in TileChance)
		{
			Sprite s = tc.texture;
			Vector2[] add = new Vector2[2];
			add[0].x = s.textureRect.x/ s.texture.width;
			add[0].y = s.textureRect.y/ s.texture.height;
			add[1].x = s.textureRect.xMax / s.texture.width;
			add[1].y = s.textureRect.yMax / s.texture.height;
			TileTextures.Add( tc.tile_name, add );
			mesh_renderer.sharedMaterial.mainTexture = s.texture;
		}


	}
	// Update is called once per frame
	public void BuildMesh()
	{
        instance = this;
		MapCreated = false;
		MapWorldSize = new Vector2( size_x * tileSize, size_y * tileSize );
		if(grid != null)
		{
			grid.node_radius = tileSize / 2;
			grid.GridWorldSize = MapWorldSize;
		}

		CalcTex();

		Global_TDMap = new TDMap( size_x, size_y, MapTerrainType, TileChance, MaxHeightForPlants, MaxHeightLeap);

		int numTiles = size_x * size_y;
		int numTris = numTiles * 2;
		int vsize_x = size_x * 2;
		int vsize_y = size_y * 2;
		int numVerts = vsize_x * vsize_y;

		if(vsize_x * vsize_y > 65000)
		{
			size_x = 125;
			size_y = 125;
			return;
		}

		Vector3[] vertices = new Vector3[numVerts];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];

		int[] triangles  = new int[ numTris * 3];

		int x,y;
		
		Vector3 sp = Vector3.zero;
		sp.x -= tileSize * size_x / 2;
		sp.y -= tileSize * size_x / 2;
		float r = Random.Range(-2f, 2f);
		for(y = 0; y < vsize_y; y ++){
			for(x = 0; x < vsize_x; x++)		
			{	
				vertices[y * vsize_x + x ] = new Vector3( (x + 1) / 2 * tileSize, (y + 1) / 2 * tileSize, 0) + sp;
				normals[y * vsize_x + x ] = new Vector3( 0, 0, -1);
				uv[y * vsize_x + x] = GetTexture( Global_TDMap.GetTile( x / 2, y / 2 ).type ,x,y);
			}
		}


		if(Heights)
		{
		for(y = 1; y < vsize_y - 1; y += 2)
			for(x = 1; x < vsize_x - 1; x += 2)
			{
				r = Random.Range(0,0);
				
				if(Global_TDMap.GetIfWater(x/2,  y/2))
					r = -Random.Range(-8f , -10f);
				else 
				{
					r = Global_TDMap.GetIfMountine((x + 0) /2, (y + 0)/2);
				}
				vertices[y * vsize_x + x].z = r;
				vertices[y * vsize_x + x + 1].z = r;
				vertices[(y + 1) * vsize_x + x].z = r;
				vertices[(y + 1) * vsize_x + x + 1].z = r;
			}
		
		for(y = 0; y < vsize_y; y++)
		{
			vertices[y * vsize_x].z = vertices[y * vsize_x + 1].z;
			vertices[y * vsize_x + vsize_x -1].z = vertices[y * vsize_x + vsize_x - 2].z;
		}

		for(x = 0; x < vsize_x; x++)
		{
			vertices[x].z = vertices[vsize_x + x].z;
			vertices[(vsize_y - 1) * vsize_x + x].z = vertices[(vsize_y - 2) * vsize_x + x].z;
		}
		
		print("End OF Heights!");

		}
		for(y = 0; y < size_y; y++){
			for(x = 0; x < size_x; x++)
			{
				//x = 1
				int squareIndex = y * size_x +x;
				int triOffset = squareIndex * 6;
				triangles[triOffset + 0] = (y * 2) * vsize_x + (x * 2) + 0;
				triangles[triOffset + 1] = (y * 2) * vsize_x + (x * 2) + vsize_x + 0;
				triangles[triOffset + 2] = (y * 2) * vsize_x + (x * 2) + vsize_x + 1;

				triangles[triOffset + 3] = (y * 2) * vsize_x + (x * 2) + 0;
				triangles[triOffset + 4] = (y * 2) * vsize_x + (x * 2) + vsize_x + 1;
				triangles[triOffset + 5] = (y * 2) * vsize_x + (x * 2) + 1;
			}
		}

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

		OnMapGenerator GenerateTrees = new OnMapGenerator(Global_TDMap, size_x, size_y, tileSize, MapHolder);
		StartCoroutine(GenerateTreesEI(GenerateTrees));
		//MapCreated = true;
	}

	public IEnumerator GenerateTreesEI(OnMapGenerator gen)
	{
		yield return gen.GenerateTrees(GenMaxAtOnce);
		MapCreated = true;
	}
	public TDTile TileFromWorldPoint(Vector2 pos)
    {
        float precentX = (pos.x + MapWorldSize.x / 2) / MapWorldSize.x;
        float precentY = (pos.y + MapWorldSize.y / 2) / MapWorldSize.y;
        precentX = Mathf.Clamp01(precentX);
        precentY = Mathf.Clamp01(precentY);

        int x = Mathf.RoundToInt((size_x - 1) * precentX);
        int y = Mathf.RoundToInt((size_y - 1) * precentY);

        return TDMap.GlobalMap.GetTile(x, y);
    }

}
