using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class CreateWorld : MonoBehaviour
{
    public static bool Fully_loaded = false;

    public bool CreatedEveryTHing = false;
    public Sprite World_grass;

    public Transform plants_t;
    public Transform ground_t;
    public Transform trees_t;
    public Sprite[] blocks;
    public Sprite[] grass_Special;
    public Sprite[] wood;
    public Sprite[] sand;
    //public int ChanceGrass = 75;
    public int ChanceBlocks = 15;
    public int ChanceWood = 10;
    public int ChanceTree = 33;
    public int ChanceSand = 33;
    public GameObject[] trees;

    private List<Sprite> plants = new List<Sprite>();
    //private List<Sprite> threes = new List<Sprite>();

    GameObject game_Object;
    GameObject plant_prefab;
    

    Enviroment world;

    public int plant_count = 2;
    public float plant_diff = 2;

    public float scale = 1;
    public bool ReDoWorld = false;
    public int Border_X = 200;
    public int Border_Y = 200;
    private const int pixels = 16;

    // Use this for initialization
    void Start()
    {
        game_Object = (GameObject)Resources.Load("Envitoment/Ground");
        plant_prefab = (GameObject)Resources.Load("Prefabs/plant");
        foreach (Sprite s in Resources.LoadAll<Sprite>("Envitoment/sprites/plants"))
        {
            if (s.name.Contains("plants"))
                plants.Add(s);
            //else if (s.name.Contains("three"))
              //  threes.Add(s);
        }        
        


        trees = Resources.LoadAll<GameObject>("Envitoment/threes");

        world = new Enviroment(Border_X, Border_Y, pixels, ground_t, plants_t, trees_t, this);

        //Enviroment.blocks = blocks;
        //Enviroment.wood = wood;
        //Enviroment.World_grass = World_grass;
        //Enviroment.plants = plants;
        //Enviroment.grass_Special = grass_Special;

        StartCoroutine(world.GenerateWorld());
        //GenerateWorld();
    }

    public void CreateTexture(Vector2 pos, Sprite sprite, ground_type gt)
    {
        Transform child = Instantiate(game_Object).transform;
        SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
        //child.gameObject.AddComponent<Unselectable>();
        if(gt == ground_type.world_grass)
            sr.sortingLayerName = "wgrass";
        else
            sr.sortingLayerName = "ground";

        sr.sprite = sprite;
        child.SetParent(ground_t);
        child.localPosition = pos;
        child.localScale = new Vector3(scale, scale, 1);

        //print(gt);
    }


    public Ground_tile ChanceChoose(int _x,int _y, Vector2 worldPos)
    {
        int chance = 80;
        Ground_tile ret = new Ground_tile(worldPos, _x, _y);
        
        for(int x = -2; x < 1; x ++)
            for(int y = -2; y < 1; y++)
            {
                if(x == 0 && y == 0) 
                    continue;
                int xt = _x + x;
                int yt = _y + y;
                xt = Mathf.Clamp(xt, 0, world.Border_X - 1);
                yt = Mathf.Clamp(yt, 0, world.Border_Y - 1);

                ground_type temp = (world.map[xt, yt] != null) ? world.map[xt, yt].type: ground_type.world_grass;
                if(temp == ground_type.world_grass)
                    continue;

                int chance_t = random_int(0, 100);
                if(chance_t > chance)
                {
                    chance = chance_t;
                    ret.type = temp;             
                }
            }

        if(ret.type != ground_type.world_grass)
            return ret;

        chance = random_int(0, 100);
        if (chance < ChanceWood)
        {
            ret.type = ground_type.wood;
            //ret.sprite = wood[random_int(0, wood.Length)];
            //ret.type = ground_type.wood;
        }
        else if (chance < ChanceBlocks + ChanceWood)
        {
            ret.type = ground_type.blocks;
            //ret.type = ground_type.blocks;
            //ret.sprite = blocks[random_int(0, blocks.Length)];
        }
        else if (chance < ChanceBlocks + ChanceWood + ChanceSand)
        {
            ret.type = ground_type.sand;
            //ret.type = ground_type.grass_s;
            //ret.sprite = grass_Special[random_int(0, grass_Special.Length)];
        }
        return ret;
    }

    int random_int(int min, int max)
    {
        return Mathf.RoundToInt(Random.Range(min, max));
    }

    public IEnumerator GenerateTiles()
    {

        int i = 0;
        for(int x = 0; x < world.Border_X; x++)
            for(int y = 0; y < world.Border_Y; y++)
            {
                Ground_tile gt = world.map[x, y];
                if(gt.type == ground_type.world_grass)
                    continue;

                tile_typ2e type = GetType(x, y, gt.type);
                SelectTileSprite(type, gt.type, gt.pos);
                i++;
                if(i % 10 == 0)
                    yield return new WaitForSeconds(Time.deltaTime);
            }

        yield return null;
    }

    public void SelectTileSprite(tile_typ2e type, ground_type gt, Vector2 pos)
    {
        if(gt == ground_type.sand)
        {
            foreach(Sprite s in sand)
            {
                if(s.name.Contains(type.ToString()))
                {
                    CreateTexture(pos, s, gt);
                }
            }
        }
    }

    public tile_typ2e GetType(int _x, int _y, ground_type type)
    {
        Dictionary<tile_typ2e, bool> CanBe = new Dictionary<tile_typ2e, bool>();
        foreach(tile_typ2e t in EnumCheck<tile_typ2e>.GetEnumValues())
        {
            CanBe.Add(t,false);
            if(t == tile_typ2e.mid)
                CanBe[t] = true;
        }
        

        for(int x = -1; x < 2; x ++)
            for(int y = -1; y < 2; y++)
            {
                if(x == 0 && y == 0)
                    continue;

                int xt = _x + x;
                int yt = _y + y;
                xt = Mathf.Clamp(xt, 0, world.Border_X - 1);
                yt = Mathf.Clamp(yt, 0, world.Border_Y - 1);
                if(xt == _x && yt == _y)
                    continue;
                
                ground_type t = world.map[xt, yt].type;
                if(t != type)
                {
                    CanBe[tile_typ2e.mid] = false;
                    continue;
                }
                    
                if(x == -1)
                {
                    if(y == -1)
                    {
                        
                    }

                }

            }

        return tile_typ2e.eev;
        
    }

    public void GeneratePlants(int x, int y)
    {
        int pc = random_int(0, plant_count);
        for (int i = 0; i < pc; i++)
        {
            bool tree = random_int(0, 100) <= ChanceTree;
            if (!tree)
            {
                int px = random_int(-pixels / 2, pixels / 2) + x;
                int py = random_int(-pixels / 2, pixels / 2) + y;

                Transform child = Instantiate(plant_prefab).transform;
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();

                sr.sprite = plants[random_int(0, plants.Count)];

                child.SetParent(plants_t);

                child.localPosition = new Vector3(px, py, 0);



                child.localScale = new Vector3(scale, scale, 1);
            }
            else
            {
                int px = random_int(-pixels / 2, pixels / 2) + x;
                int py = random_int(-pixels / 2, pixels / 2) + y;

                Transform child = Instantiate(trees[random_int(0, trees.Length)]).transform;
                child.SetParent(trees_t);

                child.localPosition = new Vector3(px, py, 0);
                child.localScale = new Vector3(scale, scale, 1);
            }
        }
    }



    // Update is called once per frame
    void Update()
    {

        if (ReDoWorld)
        {
            world = new Enviroment(Border_X, Border_Y, pixels, ground_t, plants_t, trees_t, this);
            StartCoroutine(world.GenerateWorld());
            ReDoWorld = false;
            Fully_loaded = false;
        }
    }
}

public enum tile_typ2e
{
    mid = 0,
    oelt = 1,
    oell = 2,
    oelb = 3,
    oebb = 4,
    oerb = 5,
    oerr = 6,
    oert = 7,
    oett = 8,   
    iert = 9,
    ierb = 10,
    ielb = 11,
    ielt = 12,
    eev = 13,
}

public enum ground_type
{
    world_grass,
    grass_s,
    wood,
    blocks,
    plants,
    sand,
}

public class Ground_tile
{
    public Ground_tile(Vector2 _pos,int x, int y)
    {
        pos = _pos;
        xa = x;
        ya = y;
        type = ground_type.world_grass;
        tile = tile_typ2e.eev;
    }
    public ground_type type;
    public Vector2 pos;
    public int xa,ya;
    public tile_typ2e tile;
}

public class Enviroment
{
    public Ground_tile[,] map;
    public int Border_Y;
    public int Border_X;
    int x_zero;
    int y_zero;
    int pixels;
    int scale = 1;

    CreateWorld self;
    Transform ground;
    Transform plants_t;
    Transform trees_t;

    public Enviroment(int x, int y, int _pixels, Transform _ground, Transform _plants_t, Transform _trees_t, CreateWorld script)
    {
        Border_X = (x * 2) / (_pixels);
        Border_Y = (y * 2) / (_pixels);
        map = new Ground_tile[Border_X, Border_Y];
        x_zero = x;
        y_zero = y;
        pixels = _pixels;
        ground = _ground;
        plants_t = _plants_t;
        self = script;
        trees_t = _trees_t;
        Debug.Log(Border_Y);
        self.CreatedEveryTHing = false;
    }

    public IEnumerator DeleteOld(Transform t)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            t.GetChild(i).gameObject.AddComponent<DestroySelf>();
            if (i > 50)
            {
                i = 0;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }

    public IEnumerator GenerateWorld()
    {
        int i = 0;
        yield return DeleteOld(trees_t);
        yield return DeleteOld(plants_t);
        yield return DeleteOld(ground);
        int xm = 0;
        int ym = 0;
        for (int x = -x_zero; xm < Border_Y; x += pixels * (int)scale, xm++)
        {
            for (int y = -y_zero; ym < Border_Y; y += pixels * (int)scale, ym++)
            {
                Ground_tile get = self.ChanceChoose(xm, ym, new Vector2(x, y));

                //if (get == ground_type.world_grass)
                //{
                  //  self.GeneratePlants(x, y);
                   // continue;
                //}          
                //self.CreateTexture(x, y, get.sprite, get.type);

                //Debug.Log(" / x / " + xm + " / y / " + ym + " // ; //  / x / " 
                       // + (x + x_zero)/pixels + " / y / " + (y + y_zero)/pixels);
                map[xm, ym] = get;

                i++;
                if (i > 500)
                {
                    yield return new WaitForSeconds(Time.deltaTime);
                    i = 0;
                }
            }
            ym = 0;
        }

        yield return self.GenerateTiles();

        self.CreatedEveryTHing = true;
        CreateWorld.Fully_loaded = true;
    }
    void CreateX8(float x, float y, Sprite sprite, int pixels, ground_type gt)
    {
        float skip = (pixels / 4 * scale);
        float move = (pixels / 2 * scale) - skip;
        CreateX4(x - skip, y - skip, sprite, pixels / 2, gt);
        CreateX4(x + move, y - skip, sprite, pixels / 2, gt);
        CreateX4(x + move, y + move, sprite, pixels / 2, gt);
        CreateX4(x - skip, y + move, sprite, pixels / 2, gt);
    }
    void CreateX4(float x, float y, Sprite sprite, int pixels, ground_type gt)
    {
        float skip = (pixels / 4 * scale);
        float move = (pixels / 2 * scale) - skip;
        //self.CreateTexture(x - skip, y - skip, sprite, gt);
        //self.CreateTexture(x + move, y - skip, sprite, gt);
        //self.CreateTexture(x + move, y + move, sprite, gt);
        //self.CreateTexture(x - skip, y + move, sprite, gt);
    }



    int random_int(int min, int max)
    {
        return Mathf.RoundToInt(Random.Range(min, max));
    }

}

public class ground_st
{
    public Sprite sprite;
    public ground_type type;
}
