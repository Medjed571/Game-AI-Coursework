using UnityEngine;
using System.Collections.Generic;

public class PathfindingBase : MonoBehaviour
{
    protected const int nodesWidth = 100;       //width of the map
    protected const int nodesHeight = 100;      //height of the map

    protected int movementXCost = 10;           //movement cost along the X axis
    protected int movementYCost = 10;           //movement cost along the Y axis
    protected int movementDiagonalCost = 14;    //movement cost when moving diagonally
    protected int movementDiagonalMinusXY;      

    protected int mudCost = 1;                  //if moving through mud, add variable to the movement cost.
    protected int waterCost = 2;                //if moving through water, add variable to the movement cost.

    protected Node[] nodes;                     //holds all nodes

    protected Map map;                          //used to refer to each tile on the map.

    protected byte[] mapData;                   //used to refer to the map data.

    protected void Awake()
    {
        movementDiagonalMinusXY = movementDiagonalCost - (movementXCost + movementYCost);
        
        var gameData = GameData.Instance;
        map = gameData.Map;
        mapData = map.GetMapData();

        CreateNodes();
        CreateNodesConnections();
    }

    private void CreateNodes()
    {
        nodes = new Node[nodesWidth * nodesHeight]; //creates a node per tile on the map.

        for (int y = 0; y < nodesHeight; ++y) //sets the position of each node.
        {
            for (int x = 0; x < nodesWidth; ++x)
            {
                Node node = new Node();
                node.x = x;
                node.y = y;
                nodes[x + (nodesWidth * y)] = node;
            }
        }
    }

    private void CreateNodesConnections()
    {
        for (int nodeY = 0; nodeY < nodesHeight; ++nodeY) //for every node
        {
            for (int nodeX = 0; nodeX < nodesWidth; ++nodeX)
            {
                int nodeIndex = nodeX + (nodesWidth * nodeY); //create index of each node
                Node node = nodes[nodeIndex];
                if (mapData[nodeIndex] > 2) //if the node is on a tree tile
                {
                    node.neighbours = new Node[0]; //store which nodes are connected to one another
                    node.neighbourCosts = new int[0]; //same as before but in int form
                    continue;
                }

                int connectedNodesCount = 0; //tracks the connected nodes
                for (int neighbourY = nodeY - 1; neighbourY <= nodeY + 1; ++neighbourY) //for each neighbouring Y node 
                {
                    if (neighbourY < 0 || neighbourY >= nodesHeight) //if the neighbour is out of bounds
                    {
                        continue; //skip over it
                    }

                    for (int neighbourX = nodeX - 1; neighbourX <= nodeX + 1; ++neighbourX) //for each neighbouring X node
                    {
                        if (neighbourX < 0 || neighbourX >= nodesWidth || (neighbourX == nodeX && neighbourY == nodeY) || mapData[neighbourX + (neighbourY * nodesWidth)] > 2) //if the neighbour is out of bounds or already accounted for or a tree tile
                        {
                            continue; //skip over it
                        }

                        ++connectedNodesCount; //tracks the currently connected nodes
                    }
                }

                node.neighbours = new Node[connectedNodesCount]; //tracks nodes with connections
                node.neighbourCosts = new int[connectedNodesCount];

                int connectedNodesIndex = 0;
                for (int neighbourY = nodeY - 1; neighbourY <= nodeY + 1; ++neighbourY) //for each neighbouring Y node
                {
                    if (neighbourY < 0 || neighbourY >= nodesHeight) //if the neighbour is out of bounds
                    {
                        continue; //skip over it
                    }

                    for (int neighbourX = nodeX - 1; neighbourX <= nodeX + 1; ++neighbourX) //for each neighbouring X node
                    {
                        if (neighbourX < 0 || neighbourX >= nodesWidth || (neighbourX == nodeX && neighbourY == nodeY) || mapData[neighbourX + (neighbourY * nodesWidth)] > 2) //if the neighbour is out of bounds or already accounted for or a tree tile
                        {
                            continue; //skip over it
                        }

                        node.neighbours[connectedNodesIndex] = nodes[neighbourX + (neighbourY * nodesWidth)]; //stores node connections
                        node.neighbourCosts[connectedNodesIndex] = CalculateInitialCost(nodeX, nodeY, neighbourX, neighbourY); //stores how much it costs for one node to connect to another node
                        ++connectedNodesIndex;
                    }
                }
            }
        }
    }

    protected int CalculateInitialCost(int firstNodeX, int firstNodeY, int secondNodeX, int secondNodeY)
    {
        int xCost = Mathf.Abs(secondNodeX - firstNodeX); //returns positive X value
        int yCost = Mathf.Abs(secondNodeY - firstNodeY); //returns positive Y value

        //if the second node is on water
        if (map.GetTerrainAt(secondNodeX, secondNodeY) == Map.Terrain.Water)
        {
            if ((xCost + yCost) < 2)
            {
                if (xCost > 0)
                {
                    return movementXCost + waterCost;
                }
                return movementYCost + waterCost;
            }
            return movementDiagonalCost + waterCost;
        }

        //if the second node is on mud
        if (map.GetTerrainAt(secondNodeX, secondNodeY) == Map.Terrain.Mud)
        {
            if ((xCost + yCost) < 2)
            {
                if (xCost > 0)
                {
                    return movementXCost + mudCost;
                }
                return movementYCost + mudCost;
            }
            return movementDiagonalCost + mudCost;
        }

        //if the second node is on grass
        if ((xCost + yCost) < 2)
        {
            if (xCost > 0)
            {
                return movementXCost;
            }
            return movementYCost;
        }
        return movementDiagonalCost;
    }

    protected List<Node> GetFoundPath(Node endNode)
    {
        List<Node> foundPath = new List<Node>();
        if (endNode != null)
        {
            foundPath.Add(endNode);

            while (endNode.parent != null)
            {
                foundPath.Add(endNode.parent);
                endNode = endNode.parent;
            }

            foundPath.Reverse(); //reverse the path so the start node is at index 0
        }
        return foundPath;
    }
}
