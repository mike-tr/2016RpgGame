using UnityEngine;
public class TDTile
{

	public class Properties
	{

			public enum Terain_type
			{
				water,
				normal,
				hill,
				mountain
			}
			public Terain_type tile_type;
			public bool IsWalkable;
			public bool CanPlantsGrow;

			public Properties(Terain_type tile_type, bool IsWalkable, bool CanPlantsGrow)
			{
				this.tile_type = tile_type;
				this.IsWalkable = IsWalkable;
				this.CanPlantsGrow = CanPlantsGrow;
			}
	}
	TDMap map;
	public int x,y;
	public Vector2 pos;
	public int Height = 0;

	public bool CanPlantsGrow { get { return (Height <= map.MaxHeightForPlants) && propreties.CanPlantsGrow; } } 
	private Properties propreties;
	public bool IsMountain { get { return propreties.tile_type == Properties.Terain_type.mountain; } }

	public bool IsHill { get { return propreties.tile_type == Properties.Terain_type.hill; } }
	public bool IsWater { get { return propreties.tile_type == Properties.Terain_type.water; } }
	public bool IsWalkable { get { return propreties.IsWalkable; } }
	public enum texType
	{
		normal,
		inBorder,
		Border,
	}

	private string ctype;
	public string type
	{
		get
		{
			return ctype;
		}
		set
		{
			Properties tryg;
			if(map.GetTileTypes.TryGetValue(value, out tryg))
			{
				ctype = value;
				propreties = tryg;
			}
		}
	}
	
	public TDTile(int x, int y, TDMap map)
	{
		this.x = x;
		this.y = y;
		this.map = map;
	}
	public TDTile(int x, int y, TDMap map, string type)
	{
		this.x = x;
		this.y = y;
		this.map = map;
		this.type = type;
		this.Height = 0;
	}

	public TDTile GetNeigbour(int x, int y)
	{
		TDTile r = map.GetTile(this.x + x, this.y + y);
		if(r == null)
		{
			return this;
		}
		return r;
	}
}

