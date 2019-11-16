using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathGenerator : MonoBehaviour {

    Grid_creator grid;
    PathRequestManager requestManager;

	// Use this for initialization
	void Start () {
        grid = GetComponent<Grid_creator>();
        requestManager = GetComponent<PathRequestManager>();
	}

    public void FindPath(PathRequest request, Action<PathResult> callback)
    {
        Vector2[] waypoints = new Vector2[0];
        bool pathSuccess = false;

        node startNode = grid.NodeFromWorldPoint(request.pathStart);
        node targetNode = grid.FindClosestWalkableNode(request.pathEnd, startNode);
        //node targetNode = grid.NodeFromWorldPoint(request.pathEnd);
        


        startNode.parent = startNode;


        if (startNode.walkable && targetNode.walkable)
        {
            Heap<node> openSet = new Heap<node>(grid.MaxSize);
            HashSet<node> closedSet = new HashSet<node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                node currentNode = openSet.RemoveFirst();
                
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (node neighbour in grid.GetNeigbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + 
                        GetDistance(currentNode, neighbour) + neighbour.MovementPenalty;
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }
        }
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
            
            pathSuccess = waypoints.Length > 0;
        }
        callback(new PathResult(waypoints, pathSuccess, request.callback));

    }

    Vector2[] RetracePath(node startNode, node endNode)
    {
        List<node> path = new List<node>();
        node currentNode = endNode;
        
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector2[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector2[] SimplifyPath(List<node> path)
    {
        List<Vector2> waypoints = new List<Vector2>();

        if(path.Count > 2)
        {
            Vector2 directionOld = Vector2.zero;
            Vector2 directionOld2 = Vector2.zero;

            waypoints.Add(path[0].pos);

            for (int i = 1; i < path.Count - 1; i++)
            {
                Vector2 directionNew = path[i - 1].pos - path[i + 1].pos;
                Vector2 directionNew2 = path[i - 1].pos - path[i].pos;
                if (directionNew != directionOld || directionOld2 != directionNew2) 
                {
                    waypoints.Add(path[i].pos);
                }

                directionOld = directionNew;
                directionOld2 = directionNew2;
            }
            
            waypoints.Add(path[path.Count - 1].pos);
        }
        else
        {
            foreach(node n in path)
                waypoints.Add(n.pos);
        }

        return waypoints.ToArray();
    }

    int GetDistance(node nodeA, node nodeB)
    {
        int distX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int distY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }
}
