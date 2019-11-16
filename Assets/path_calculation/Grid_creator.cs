using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_creator : MonoBehaviour {

    public static Grid_creator instance;
    public Vector2 GridWorldSize;
    public float node_radius = 2;

    node[,] Grid;

    public bool Loaded = false;


    float NodeDiamiter;
    int GridSizeX, GridSizeY;

    int penaltyMin = int.MaxValue;
    int penaltyMax = int.MinValue;
    int MaxLeap = 2;
    public int MaxSize
    {
        get
        {
            return GridSizeX * GridSizeY;
        }
    }

    // Use this for initialization
    IEnumerator Start()
    {
        NodeDiamiter = node_radius * 2;
        GridSizeX = Mathf.RoundToInt(GridWorldSize.x / NodeDiamiter);
        GridSizeY = Mathf.RoundToInt(GridWorldSize.y / NodeDiamiter);

        while (!TGMap.instance.MapCreated)
        {
            yield return new WaitForSeconds(0.1f);
        }

        print("calculating giz");

        CreateGrid();

        MaxLeap = TDMap.GlobalMap.MaxHeightLeap;

        print("GridDone");
        Loaded = true;

        instance = this;
    }


    void CreateGrid()
    {
        Grid = new node[GridSizeX, GridSizeY];
        Vector2 CornerLeftDown = transform.position - (Vector3.right * GridWorldSize.x/2) - Vector3.up * GridWorldSize.y/2;
        for (int x = 0; x < GridSizeX; x++)
            for (int y = 0; y < GridSizeY; y++)
            {

                Vector2 p = CornerLeftDown + Vector2.right * (x * NodeDiamiter + node_radius)
                    + Vector2.up * (y * NodeDiamiter + node_radius);
                bool walkable = rayCast(p, node_radius);
                bool diagonal_move = rayCast(p , node_radius * 2);

                float penalty = 0;

                /* 
                if (walkable)
                {
                    for (float k = 4, b = 0.95f ; k > 1.2f; k *= b, b -= 0.03f)
                    {
                        if (!rayCast(p, node_radius * k))
                        penalty += 3 - b;
                    }
                }
                else
                    penalty = 30;
                    */
                //print(penalty);
                Grid[x, y] = new node(p, walkable, diagonal_move, x, y, Mathf.RoundToInt(penalty));

                if (penalty > penaltyMax)
                {
                    penaltyMax = Mathf.RoundToInt(penalty);
                }
                if (penalty < penaltyMin)
                {
                    penaltyMin = Mathf.RoundToInt(penalty);
                }
            }
        //BlurPenaltyMap(3, true);
    }

    void BlurPenaltyMap(int blurSize,bool t)
    {
        int kernelSize = blurSize * 2 + 1;
        int kernelExtents = (kernelSize - 1) / 2;

        int[,] penaltiesHorizontalPass = new int[GridSizeX, GridSizeY];
        int[,] penaltiesVerticalPass = new int[GridSizeX, GridSizeY];

        for (int y = 0; y < GridSizeY; y++)
        {
            for (int x = -kernelExtents; x <= kernelExtents; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                penaltiesHorizontalPass[0, y] += Grid[sampleX, y].MovementPenalty;
            }

            for (int x = 1; x < GridSizeX; x++)
            {
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, GridSizeX);
                int addIndex = Mathf.Clamp(x + kernelExtents, 0, GridSizeX - 1);

                penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - Grid[removeIndex, y].MovementPenalty + Grid[addIndex, y].MovementPenalty;
            }
        }

        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = -kernelExtents; y <= kernelExtents; y++)
            {
                int sampleY = Mathf.Clamp(y, 0, kernelExtents);
                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
            }

            int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize));
            Grid[x, 0].MovementPenalty = blurredPenalty;

            for (int y = 1; y < GridSizeY; y++)
            {
                int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, GridSizeY);
                int addIndex = Mathf.Clamp(y + kernelExtents, 0, GridSizeY - 1);

                penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] - penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];
                blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));
                if (t)
                    Grid[x, y].MovementPenalty = blurredPenalty;
                else
                {
                    Grid[x, y].MovementPenalty += blurredPenalty;
                    Grid[x, y].MovementPenalty = Mathf.RoundToInt((float)Grid[x, y].MovementPenalty * 0.3f);
                }
                if (blurredPenalty > penaltyMax)
                {
                    penaltyMax = blurredPenalty;
                }
                if (blurredPenalty < penaltyMin)
                {
                    penaltyMin = blurredPenalty;
                }
            }
        }
    }

    bool rayCast(Vector2 pos, float radius)
    {
        Collider2D coll = Physics2D.OverlapCircle(pos, radius);

        if (coll != null)
        {
            if (coll.transform.tag == "character_box")
                return GetIfWalkableTerrain(pos, radius);
            return false;
        }
        return GetIfWalkableTerrain(pos, radius);
    }

    bool GetIfWalkableTerrain(Vector2 p, float radius)
    {
        int r = Mathf.RoundToInt(radius / node_radius) - 1;

        TDTile t = TGMap.instance.TileFromWorldPoint(p);
        if(!t.IsWalkable)
            return false;
        for(int x = -r; x <= r; x++)
            for(int y = -r ; y <= r; y++)
            {
                if(!t.GetNeigbour( x, y).IsWalkable)
                    return false;
            }
        return true;
    }

    public bool draw_giz = false;
    // Update is called once per frame

    public List<node> GetNeigbours(node Node)
    {
        List<node> ret = new List<node>();
        
        if(Node.diagonal_move)
        {
            for(int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = Node.GridX + x;
                    if (checkX < 0 || checkX >= GridSizeX)
                        continue;
                    int checkY = Node.GridY + y;
                    if (checkY < 0 || checkY >= GridSizeY)
                        continue;
                    node r = Grid[checkX, checkY];
                    //if(Mathf.Abs(Node.tile.Height - r.tile.Height) > MaxLeap)
                      //  continue;
                    ret.Add(r);
                }
        }
        else
        {
            for(int x = -1; x <= 1; x++)
            {
                if(x == 0)
                    continue;
                int checkX = Node.GridX + x;
                    if (checkX < 0 || checkX >= GridSizeX)
                        continue;
                ret.Add(Grid[checkX, Node.GridY]);
            }
            for(int y = -1; y <= 1; y++)
            {
                if(y == 0)
                    continue;
                int checkY = Node.GridY + y;
                    if (checkY < 0 || checkY >= GridSizeY)
                        continue;
                ret.Add(Grid[Node.GridX, checkY]);
            }
        }
        return ret;
    }

    public node NodeFromWorldPoint(Vector2 pos)
    {
        pos -= (Vector2)transform.position;
        float precentX = (pos.x + GridWorldSize.x / 2) / GridWorldSize.x;
        float precentY = (pos.y + GridWorldSize.y / 2) / GridWorldSize.y;
        precentX = Mathf.Clamp01(precentX);
        precentY = Mathf.Clamp01(precentY);

        int x = Mathf.RoundToInt((GridSizeX - 1) * precentX);
        int y = Mathf.RoundToInt((GridSizeY - 1) * precentY);

        return Grid[x, y];
    }
    public MovePos WalkableNearMeNodePos(Vector2 pos)
    {
        MovePos ret = new MovePos(false, Vector2.zero);
        node n = NodeFromWorldPoint(pos);

        if(n.walkable)
            return ret;
            
        
        node retn = GetWalkableNodeFromRadius(n, 1, true, null);
        int i = 2;
        while (retn == n)
        {
            if (i > 50)
                return ret;
            retn = GetWalkableNodeFromRadius(n, i, true, null);
            i++;        
        }
        ret.pos = retn.pos;
        ret.Move = true;
        return ret;
    }

    public node FindClosestWalkableNode(Vector2 pos, node StartPoint)
    {
        node n = NodeFromWorldPoint(pos);
        node ret = n;
        int i = 1;
        while(!ret.walkable)
        {
            if (i > 3)
                return n;
            ret = GetWalkableNodeFromRadius(n, i, true, StartPoint);
            i++;        
        }
        return ret;
    }

    public node GetWalkableNodeFromRadius(node n,int radius, bool skip, node StartPoint)
    {
        node ret = null;
        node nr = n;
        float dist = 5000;
        for(int x = -radius; x <= radius; x++)
            for(int y = -radius; y <= radius; y++)
            {
                if(skip && Mathf.Abs(x) + Mathf.Abs(y) != (radius))
                    continue;
                int currentX = Mathf.Clamp(x + n.GridX,0, GridSizeX);
                int currentY = Mathf.Clamp(y + n.GridY,0, GridSizeY);

                ret = Grid[currentX, currentY];
                if (ret.walkable)
                {
                    if (StartPoint == null)
                        return ret;
                    float distn = Vector2.Distance(ret.pos, StartPoint.pos);
                    if (dist > distn)
                    {
                        nr = ret;
                        dist = distn;
                    }
                
                }

            }
        return nr;
    }
    public node GetNode( int x, int y)
	{
		if(x < 0 || x >= GridSizeX || y < 0 || y >= GridSizeY)
		{
			//Debug.Log(x + " ," + y + " are not existing!");
			return null;
		}
		return Grid[ x, y ];
	}
    void OnDrawGizmos()
    {
        if (!draw_giz || Grid == null)
            return;

        foreach(node n in Grid)
        {
            //Gizmos.color = Color.Lerp(new Color(1,1,1,0.1f), Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.MovementPenalty));
            Gizmos.color = (n.walkable) ? (n.diagonal_move) ? new Color(1,1,1, 0.2f) 
                            : new Color(0f, 0f, 1f, 0.5f) : new Color(1,0,0, 0.5f);
            
            Gizmos.DrawCube(n.pos, Vector3.one * (NodeDiamiter));
        }
    }
}

public class node : IHeapItem<node>
{
    public Vector2 pos;
    public int GridX, GridY;
    public bool walkable;
    public bool diagonal_move;
    public int MovementPenalty;
    public TDTile tile;
    public node(Vector2 _pos, bool _walkable, bool _diagonal_move, int _GridX, int _GridY, int _penalty)
    {
        GridX = _GridX;
        GridY = _GridY;
        pos = _pos;
        walkable = _walkable;
        diagonal_move = _diagonal_move;
        MovementPenalty = _penalty;
        tile = TDMap.GlobalMap.GetTile( GridX, GridY );
    }

    public node parent;

    public int gCost;
    public int hCost;

    public int fCost
    {
        get { return gCost + hCost; }
    }

    int index;
    public int HeapIndex
    {
        get
        {
            return index;
        }
        set
        {
            index = value;
        }
    }

    public int CompareTo(node n)
    {
        int compare = fCost.CompareTo(n.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(n.hCost);
        }
        return -compare;
    }
}
