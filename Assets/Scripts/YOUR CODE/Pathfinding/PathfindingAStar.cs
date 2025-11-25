using System.Collections.Generic;
using System;

public class PathfindingAStar
{
    public static PathfindingAStar Instance { get; private set; }

    protected int movementXCost = 10;
    protected int movementYCost = 10;
    protected int movementDiagonalCost = 14;
    protected int movementDiagonalMinusXY;

    public PathfindingAStar()
    {
        Instance = this;
        movementDiagonalMinusXY = movementDiagonalCost - (movementXCost + movementYCost);
    }

    public List<Node> AStarSearch(Node startNode, Node endNode)
    {
        if (startNode == endNode)
        {
            startNode.onClosedList = true;
            return new List<Node>() { startNode };
        }

        List<Node> openList = new List<Node>();

        openList.Add(startNode);
        Node currentNode;

        while (openList.Count > 0)
        {
            openList.Sort();
            currentNode = openList[0];
            openList.RemoveAt(0);

            currentNode.onClosedList = true;

            if (currentNode == endNode)
            {
                return GetFoundPath(endNode);
            }

            Node[] neighbours = currentNode.neighbours;
            int neighboursCount = neighbours.Length;
            for (int connectedNodesIndex = 0; connectedNodesIndex < neighboursCount; ++connectedNodesIndex)
            {
                Node currentNeighbour = neighbours[connectedNodesIndex];

                if (currentNeighbour.onClosedList)
                    continue;

                int gCost = currentNode.g + currentNode.neighbourCosts[connectedNodesIndex];
                int hCost = ChebyshevDistanceHeuristic(currentNeighbour.x, currentNeighbour.y, endNode.x, endNode.y);
                int fCost = gCost + hCost;

                if (fCost < currentNeighbour.f || !currentNeighbour.onOpenList)
                {
                    currentNeighbour.g = gCost;
                    currentNeighbour.h = gCost;
                    currentNeighbour.f = fCost;
                    currentNeighbour.parent = currentNode;
                }

                if(!currentNeighbour.onOpenList)
                {
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

            foundPath.Reverse();
        }
        return foundPath;
    }
}
