using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : PathfindingBase
{
    public int startNodeX;
    public int startNodeY;
    public int endNodeX;
    public int endNodeY;

    public List<Node> ExecuteAlgorithm(Vector3 target)
    {
        startNodeX = (int)transform.position.x;
        startNodeY = (int)transform.position.y;

        endNodeX = (int)target.x;
        endNodeY = (int)target.y;

        foreach (var node in nodes)
        {
            node.Reset(); //resets the data from previous runs of the algorithm
        }

        //translates coordinates into nodes
        Node startNode = nodes[startNodeX + (startNodeY * nodesWidth)];
        Node endNode = nodes[endNodeX + (endNodeY * nodesWidth)];

        return AStarPathfind(startNode, endNode);
    }

    public List<Node> AStarPathfind(Node startNode, Node endNode)
    {
        if (startNode == endNode)
        {
            startNode.onClosedList = true; //add the starting node onto the closed list
            return new List<Node>() { startNode };
        }

        List<Node> openList = new List<Node>(nodes.Length); //create a new list that can store all nodes created.

        openList.Add(startNode);

        Node currentNode;

        while (openList.Count > 0) //while openlist is not empty
        {
            openList.Sort(); //sorts list alphabetically(?) Im assuming its A-Z, 0-9.
            currentNode = openList[0]; //places the current node at the start of the list
            openList.RemoveAt(0); //and removes it

            currentNode.onClosedList = true; //places the just removed node into the closed list

            if (currentNode == endNode) //if the current node is the end node then just return it.
            {
                return GetFoundPath(endNode);
            }

            Node[] neighbours = currentNode.neighbours;
            int neighboursCount = neighbours.Length;

            for(int connectedNodesIndex = 0; connectedNodesIndex < neighboursCount; ++connectedNodesIndex)
            {
                Node currentNeighbour = neighbours[connectedNodesIndex];

                if(currentNeighbour.onClosedList) //if the current neighbour has already been sorted
                {
                    continue; //skip it
                }

                int gCost = currentNode.g + currentNode.neighbourCosts[connectedNodesIndex]; //calculated goal cost
                int hCost = ChebyshevDistanceHeuristic(currentNeighbour.x, currentNeighbour.y, endNode.x, endNode.y); //calculated heuristic
                int fCost = gCost + hCost; //calculated final cost

                if (fCost <= currentNeighbour.f || !currentNeighbour.onOpenList) //if the newly found final cost is lower than the previous lowest or if this is the first time checking the node
                {   //set the values of g,h and f to it.
                    currentNeighbour.g = gCost; 
                    currentNeighbour.h = hCost;
                    currentNeighbour.f = fCost;
                    currentNeighbour.parent = currentNode;
                }

                if (!currentNeighbour.onOpenList) //if the current neighbour isnt on the open list
                {   //put it in the open list
                    currentNeighbour.onOpenList = true;
                    openList.Add(currentNeighbour);
                }
            }
        }
        return GetFoundPath(null);
    }

    private int ChebyshevDistanceHeuristic(int currentX, int currentY, int targetX, int targetY)
    {
        int distanceX = Math.Abs(targetX - currentX);
        int distanceY = Math.Abs(targetY - currentY);
        return (distanceX * movementXCost) + (distanceY * movementYCost) + (movementDiagonalMinusXY * Math.Min(distanceX, distanceY));
    }

    private void Update()
    {
        
    }
}
