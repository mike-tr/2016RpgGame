using System.Collections.Generic;
using UnityEngine;
public class TDMap {
	private int MHP;
	public int MaxHeightForPlants{ get { return MHP; } }
	private int MWH;
	public int MaxHeightLeap{ get { return MWH; } }
	TDTile[,] tiles;
    Dictionary<string, TCSV> TileProp = new Dictionary<string, TCSV>();
	public TDTile[,] Read_Tiles
	{
		get
		{
			return tiles;
		}
	}
	public static TDMap GlobalMap;
	int size_y;
	int size_x;

	Dictionary< string, TDTile.Properties> Tile_Types = new Dictionary< string, TDTile.Properties>();
	public Dictionary< string, TDTile.Properties> GetTileTypes
	{
		get
		{
			return Tile_Types;
		}
	}
	Dictionary < string, float> TileChance = new Dictionary< string, float>();

	public void CreateTileTypes(TCSV[] TileChance)
	{
		this.TileChance = new Dictionary< string, float>();
		Tile_Types = new Dictionary< string, TDTile.Properties>();
		float inc = 0;
		foreach(TCSV t in TileChance)
		{
			try
			{
			    this.TileChance.Add( t.tile_name, t.Chance + 133f);
			    float temp = ( 50 / (float)this.TileChance[t.tile_name] );
			    if(temp > inc)
				    inc = temp;
			
			    TDTile.Properties np = new TDTile.Properties( t.tile_type, t.IsWakable, t.CanPlantsGrow);
			    Tile_Types.Add(t.tile_name, np);
                TileProp.Add(t.tile_name, t);
			}
			catch
			{

			}
		}

		foreach(TCSV t in TileChance)
		{
			this.TileChance[t.tile_name] *= inc;
		}

	}
	public enum MapType
	{
		loose_areas = - 1,
		normal_areas = 0,
		concentrated_areas = 1,
	}

	public TDMap( int size_x, int size_y, MapType type, TCSV[] TileChanceC, int MaxHeightForPlants, int MaxWalkableHeight )
	{
		CreateTileTypes(TileChanceC);
		this.size_x = size_x;
		this.size_y = size_y;
		MHP = MaxHeightForPlants;
		MWH = MaxWalkableHeight;
		tiles = new TDTile[ size_x, size_y];

		float chance = 0;
		TCSV tile_t = TileChanceC[0];
		for(int x  = 0; x <  size_x; x++)
			for(int y = 0; y < size_y; y++)
			{
				chance = 0;
				foreach(TCSV t in TileChanceC)
				{
					float temp = Random.Range( 0, (int)TileChance[t.tile_name]);
					if(temp > chance)
					{
						chance = temp;
						tile_t = t;
					}
				}
				tiles[x,y] = new TDTile( x, y, this, tile_t.tile_name );				
			}

		for(int x  = 0; x <  size_x; x++)
			for(int y = 0; y < size_y; y++)
			{
				int a = Mathf.Clamp((int)type, 0, 1);
				GetMoreRealisticTile( x, y, 2 + a );
			}		
		for(int x  = 0; x <  size_x; x++)
			for(int y = 0; y < size_y; y++)
			{
				int a = Mathf.Clamp(-(int)type, 0, 1);
				GetMoreRealisticTile( x, y, 2 - a );
			}	
		for(int x  = 0; x <  size_x; x++)
			for(int y = 0; y < size_y; y++)
			{
				GetMoreRealisticTile(x,y, 1);
			}	
		for(int x  = 0; x <  size_x; x++)
			for(int y = 0; y < size_y; y++)
			{
				GetMoreRealisticTile(x,y, 2);
			}	
		GlobalMap = this;
	}

	public void GetMoreRealisticTile(int _x, int _y, int range)
	{
		Dictionary< string, float> Chance = new Dictionary< string, float>();
		Chance.Add(tiles[_x,_y].type, 18);

		int r = 0;
		for(int y = -range; y <= range; y++)
		{
			r = Mathf.Abs(y);
			for(int x = -range + r; x <= range - r; x++)
			{
				
				int mx = Mathf.Clamp(_x + x, 0, size_x - 1);
				int my = Mathf.Clamp(_y + y, 0, size_y - 1);
				string t = tiles[ mx, my].type;
				float o;
				if(Chance.TryGetValue(t, out o))
				{
					Chance[t] += 33;
					Chance[t] *= 1.25f;
				}
				else
					Chance.Add(t, 33);
			}
		}
		string ret = "";
		float chance = 0;
		foreach(string vo in Chance.Keys)
		{
			if(Chance[vo] < 20)
				continue;
			float temp = Random.Range(0, Chance[vo]);
			if(chance < temp)
			{
				chance = temp;
				ret = vo;
			}
		}
		tiles[_x,_y].type = ret;

	}


	public float GetIfMountine( int x, int y)
	{
		if(!tiles[x,y].IsMountain)
			return 0;

		bool temp = GetContinuesly( x, y, 1, 0);
		int m = tiles[x,y].Height;
		while(temp && m < 50)
		{
			m++;
			temp = GetContinuesly( x, y, m + 1, m);
		}

		if(m > 3)
		{
			if(x + 1 < size_x)
			{
				tiles[x + 1,y].Height = m - 1;
				if(y + 1 < size_y)
				{
					tiles[x + 1,y + 1].Height = m - 1;
					tiles[x,y + 1].Height = m - 1;
				}
			}
			else if(y + 1 < size_y)
				tiles[x,y + 1].Height = m - 1;		
		}

		tiles[x,y].Height = m;
        TCSV t = TileProp[tiles[x, y].type];

        return (float)m * Random.Range(t.Height.x, t.Height.y) * t.HeightScale;
	}

	private bool GetContinuesly( int x, int y, int range, int dc)
	{
		if(x < 0 || y < 0 || x >= size_x - 1 || x >= size_y - 1)
			return false;
		for(int _x = -range; _x <= range; _x++)
		{
			if(_x >= -dc && _x < 0)
					_x = dc + 1;
			for(int _y = -range; _y <= range; _y++)
			{
				if(_y >= -dc && _y < 0)
					_y = dc + 1;
					
				int mx = Mathf.Clamp(_x + x, 0, size_x - 1);
				int my = Mathf.Clamp(_y + y, 0, size_y - 1);
				if(!tiles[mx, my].IsMountain)
					return false;
			}
		}
		return true;
	}

	public bool GetIfWater( int x, int y)
	{
		if(tiles[x,y].IsWater)
			return true;
		for(int _x = 0; _x < 2; _x++)
			for(int _y = 0; _y < 2; _y++)
			{
				int mx = Mathf.Clamp(_x + x, 0, size_x - 1);
				int my = Mathf.Clamp(_y + y, 0, size_y - 1);
				if(tiles[mx, my].IsWater)
					return true;
			}
		return false;
	}
	public TDTile GetTile( int x, int y)
	{
		if(x < 0 || x >= size_x || y < 0 || y >= size_y)
		{
			//Debug.Log(x + " ," + y + " are not existing!");
			return null;
		}
		return tiles[ x, y ];
	}
}
