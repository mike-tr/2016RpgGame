using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMapGenerator {
	static public OnMapGenerator instance;
	int size_y;
	int size_x;
	float tile_size;
	Transform TreePrefab;
	GameObject plant_prefab;
	GameObject[] trees;
	TDMap map;
	Transform MapTransform;
	Dictionary<natureType, List<Sprite>> nature = new Dictionary<natureType, List<Sprite>>();
    // Use this for initialization


    private int ChanceTree;
    private int ChanceVegetation;
    public OnMapGenerator(TDMap map, int size_x, int size_y, float tile_size, Transform MapTransform)
	{
		this.size_x = size_x;
		this.size_y = size_y;
		this.map = map;
		this.MapTransform = MapTransform;
		this.tile_size = tile_size;

        ChanceTree = TGMap.instance.ChanceTree;
        ChanceVegetation = TGMap.instance.ChanceVegetation;

		DeleteOld(MapTransform);

		plant_prefab = (GameObject)Resources.Load("Prefabs/plant");
		trees = Resources.LoadAll<GameObject>("Envitoment/threes");

		foreach(natureType t in EnumCheck<natureType>.GetEnumValues())
			nature.Add(t, new List<Sprite>());


        Debug.Log("done");
		foreach (Sprite s in Resources.LoadAll<Sprite>("Envitoment/natureT"))
        {
            if (s.name.Contains("plant"))
                nature[natureType.plant].Add(s);
            else if (s.name.Contains("leaves"))
                nature[natureType.leaves].Add(s);
            else if (s.name.Contains("bush"))
                nature[natureType.bush].Add(s);
        } 

	}


	public IEnumerator GenerateTrees(int d)
	{
		int i = 0;
		foreach(TDTile tile in map.Read_Tiles)
		{
			if(tile.CanPlantsGrow)
			{
				i++;
				float temp = Random.Range(0f,100f);
                if (Random.Range(0, 100) > ChanceVegetation)
                    continue;

				if(temp <= ChanceTree)
				{
					GenerateTree(tile);	
					for(int x = -1; x <= 1; x++)
						for(int y = -1; y < 1; y++)
						{
							TDTile t = tile.GetNeigbour(x,y);
							if(y == -1 && x == 0 || t == tile || t.IsWater)
								continue;
							
							if(Random.Range(0,10) < 3)
								GenerateNature(natureType.leaves, tile.GetNeigbour(x,y));
						}
				}
				else
				{
					for(int k = 0; k < Random.Range(0, 5); k++)
					{
						if(Random.Range(0,3) == 0)
							GenerateNature(natureType.bush, tile);
						else
							GenerateNature(natureType.plant, tile);
					}
				}
				
				

				if(i > d && Application.isPlaying)
				{
					i = 0;
					yield return null;
				}
			}
		}
		yield return null;
	}

	public void GenerateNature(natureType type, TDTile Tile)
	{
		Transform child = Object.Instantiate(plant_prefab).transform;
		child.SetParent(MapTransform);
		SpriteRenderer sr = child.GetComponent<SpriteRenderer>();

        if (nature[type].Count == 0)
            return;
		sr.sprite = nature[type][Random.Range(0, nature[type].Count)];

		child.localPosition = new Vector3((-size_x/2 + Tile.x) *tile_size + tile_size*0.5f,
							 (-size_y/2 + Tile.y) *tile_size + tile_size*0.5f, 0);

        if (type != natureType.leaves)
        {
            child.GetComponent<LayerSorter>().SortDown = 30;
            float scale = Random.Range(0.25f, 0.75f);
            child.localScale = Vector3.one * scale;

            Vector3 p = Vector2.zero;
            p.x = Random.Range(-0.4f, 0.4f);
            p.y = Random.Range(-0.4f, 0.4f);

            child.localPosition += p * tile_size;
        }
        else
        {
            child.GetComponent<LayerSorter>().SortDown = 30;
            child.localScale = Vector3.one * 0.7f;
        }
	}
	public void GenerateTree(TDTile Tile)
	{
		Transform child = Object.Instantiate(trees[Random.Range(0, trees.Length)]).transform;
		child.SetParent(MapTransform);

		child.localPosition = new Vector3((-size_x/2 + Tile.x) *tile_size,
							 (-size_y/2 + Tile.y) *tile_size + (tile_size * 1.5f), 0);
		//child.localScale = new Vector3(0.5f, 0.5f, 1);
		if(Grid_creator.instance != null && Grid_creator.instance.Loaded)
			Grid_creator.instance.GetNode(Tile.x, Tile.y).walkable = false;

		return;
	}
	public void DeleteOld(Transform t)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            t.GetChild(i).gameObject.AddComponent<DestroySelf>();
        }
    }

	public enum natureType
	{
		bush,
		plant,
		leaves,
	}
}
