using System.Collections.Generic;
using UnityEngine;
using System;

public static class AStar_1
{
    public static List<Vector2> AStarFindWay(Vector2 start, Vector2 target, Func<Vector2, bool> Walkable)
    {
        Node currentNode = new Node(start, target, 0, Walkable);
        List<Node> toCheck = new List<Node>() { currentNode };
        List<Vector2> Checked = new List<Vector2> { };
        Dictionary<Vector2, Vector2> pathDic = new Dictionary<Vector2, Vector2>() { };

        while (toCheck.Count > 0)
        {
            List<Node> neighbors = currentNode.GetNeigbors(Checked);
            toCheck.Remove(currentNode);
            Checked.Add(currentNode.pos);

            // check neigbors of current node
            if (neighbors.Count > 0)
            {
                foreach (var neighbor in neighbors)
                {
                    pathDic[neighbor.pos] = currentNode.pos;
                    toCheck.Add(neighbor);
                    if (neighbor.pos == target)
                    {
                        //return neighbor.path;
                        return GetPath(pathDic, neighbor.pos);
                    }
                }
            }
            foreach (var node in toCheck)
            {
                if (node.Cost < currentNode.Cost || node.Cost == currentNode.Cost && node.hCost < currentNode.hCost)
                {
                    currentNode = node;
                }
            }
        }
        return null;
    }

    public static List<Vector2> GetPath(Dictionary<Vector2, Vector2> pathDic, Vector2 endPos)
    {
        List<Vector2> pathList = new List<Vector2>() { endPos };

        while (pathDic.ContainsKey(endPos))
        {
            endPos = pathDic[endPos];
            pathList.Add(endPos);
        }
        pathList.Reverse();
        return pathList;
    }
}
public class Node
{
    public Vector2 pos;
    public Vector2 target;

    public float gCost;
    public float hCost;
    public float Cost;

    public bool walkable;

    public Func<Vector2, bool> CheckWalkable;

    public Node(Vector2 pos, Vector2 target, float gCost, Func<Vector2, bool> CheckWalkable)
    {
        this.pos = pos;
        this.target = target;
        this.gCost = gCost;
        this.hCost = GetHCost(this.pos, this.target);
        this.Cost = this.hCost;
        this.CheckWalkable = CheckWalkable;
        this.walkable = CheckWalkable(this.pos);
    }

    float GetHCost(Vector2 v1, Vector2 v2)
    {
        return Mathf.Pow(v1.x - v2.x, 2) + Mathf.Pow(v1.y - v2.y, 2);
    }

    public List<Node> GetNeigbors(List<Vector2> Checked)
    {
        List<Node> nodeList = new List<Node>()
            {
                new Node(pos + Vector2.up, target,gCost + 1, CheckWalkable),
                new Node(pos + Vector2.down, target,gCost + 1, CheckWalkable),
                new Node(pos + Vector2.left, target,gCost + 1, CheckWalkable),
                new Node(pos + Vector2.right, target,gCost + 1, CheckWalkable),
                new Node(pos + new Vector2(1,1), target,gCost + 1.4f, CheckWalkable),
                new Node(pos + new Vector2(1,-1), target,gCost + 1.4f, CheckWalkable),
                new Node(pos + new Vector2(-1,1), target,gCost + 1.4f, CheckWalkable),
                new Node(pos + new Vector2(-1,-1), target,gCost + 1.4f, CheckWalkable),

            };

        List<Node> neigbors = new List<Node>();
        foreach (var node in nodeList)
        {
            if (node.walkable && !Checked.Contains(node.pos))
            {
                neigbors.Add(node);
            }
        }
        return neigbors;
    }
}
