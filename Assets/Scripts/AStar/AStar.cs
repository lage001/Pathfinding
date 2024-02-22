using System.Collections.Generic;
using UnityEngine;
using System;

namespace Astar
{
    public static class AStar
    {
        //static List<Vector2> check = new List<Vector2>();
        #region Astar Function
        public static List<Vector2> AStarFindWay(Vector2 start, Vector2 target, Func<Vector2, bool> Walkable)
        {
            if (!Walkable(target))
                return null;
           
            Node currentNode = new Node(start, target, 0, Walkable);
            List<Node> toCheck = new List<Node>() { currentNode };
            List<Vector2> Checked = new List<Vector2> {};
            Dictionary<Vector2, Vector2> pathDic = new Dictionary<Vector2, Vector2>() { };
            
            while (toCheck.Count > 0)
            {
                List<Node> neighbors = currentNode.GetNeigbors(Checked);
                toCheck.Remove(currentNode);
                Checked.Add(currentNode.pos);

                // check neigbors of current node
                if (neighbors.Count > 0)
                {
                    Node node = neighbors[0];
                    foreach (var neighbor in neighbors)
                    {
                        //neighbor.Cost < node.Cost
                        //    || neighbor.Cost == node.Cost && neighbor.hCost < node.hCost
                        //    || neighbor.Cost == node.Cost && neighbor.hCost == node.hCost && GetVCost(neighbor.pos - currentNode.pos, target - currentNode.pos) > GetVCost(node.pos - currentNode.pos, target - currentNode.pos)
                        if (neighbor.Cost < node.Cost
                            || neighbor.Cost == node.Cost && neighbor.hCost < node.hCost
                            || neighbor.Cost == node.Cost && neighbor.hCost == node.hCost && GetVCost(neighbor.pos - currentNode.pos, target - currentNode.pos) > GetVCost(node.pos - currentNode.pos, target - currentNode.pos))
                        {
                            node = neighbor;
                        }
                        pathDic[neighbor.pos] = currentNode.pos; 
                        toCheck.Add(neighbor);
                        if (neighbor.pos == target)
                        {
                            //return neighbor.path;
                            return GetPath(pathDic, neighbor.pos);
                        }
                    }
                    currentNode = node;
                }
                else
                {
                    currentNode = toCheck[0];
                    foreach (var node in toCheck)
                    {
                        if (node.Cost < currentNode.Cost || node.Cost == currentNode.Cost && node.hCost < currentNode.hCost)
                        {
                            currentNode = node;
                        }
                    }
                }
            }
            
            return null;
        }

        public static List<Vector2> GetPath(Dictionary<Vector2,Vector2> pathDic,Vector2 endPos)
        {
            List<Vector2> pathList = new List<Vector2>() { endPos};
            
            while (pathDic.ContainsKey(endPos))
            {
                endPos = pathDic[endPos];
                pathList.Add(endPos);
            }
            pathList.Reverse();
            return pathList;
        }
        static float GetVCost(Vector2 v1, Vector2 v2)
        {
            return Vector2.Dot(v1.normalized, v2.normalized);
        }
        #endregion
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
            this.Cost = this.gCost + this.hCost;
            this.CheckWalkable = CheckWalkable;
            this.walkable = CheckWalkable(this.pos);
        }

        float GetHCost(Vector2 v1, Vector2 v2)
        {
            return Mathf.Pow(v1.x - v2.x,2) + Mathf.Pow(v1.y - v2.y,2);
        }

        
        public List<Node> GetNeigbors(List<Vector2> Checked)
        {
            List<Node> neigbors = new List<Node>();
            List<Vector2> posList = new List<Vector2>()
            {
                Vector2.up, Vector2.right, Vector2.down, Vector2.left, 
            };
            Dictionary<Vector2, bool> dic = new Dictionary<Vector2, bool>()
            {
                { new Vector2(1,1),true},
                { new Vector2(-1,1),true},
                { new Vector2(-1,-1),true},
                { new Vector2(1,-1),true},
            };
            for (int i = 0; i < posList.Count; i++)
            {
                Node node = new Node(pos + posList[i], target, gCost + 1, CheckWalkable);
                if (!node.walkable)
                {
                    dic[posList[i] + posList[(i + 1) % 2]] = false; 
                    dic[posList[i] - posList[(i + 1) % 2]] = false;
                }
                else if (!Checked.Contains(node.pos))
                {
                    neigbors.Add(node);
                }
            }
            foreach (var key in dic.Keys)
            {
                Node node = new Node(pos + key, target, gCost + 1.4f, CheckWalkable);
                if(dic[key] && node.walkable && !Checked.Contains(node.pos))
                {
                    neigbors.Add(node);
                }
            }
            Debug.Log("ÁÚ¾ÓµÄ¸öÊý£º" + neigbors.Count);
            return neigbors;
        }
    }
}