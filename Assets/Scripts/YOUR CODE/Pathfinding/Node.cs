using System;

public class Node : IComparable<Node>
{
    public int x;                       //stores the logical x coordinate of the node (not the game space coordinate)
    public int y;                       //stores the logical y coordinate of the node (not the game space coordinate)

    public Node parent;                 //stores the parent of this node. used to create a path later in the pathfinding calculation
    public Node[] neighbours;           //stores all neighbouring nodes
    public int[] neighbourCosts;        //stores the cost to all neighbouring nodes

    public int f;                       //final cost
    public int g;                       //goal cost
    public int h;                       //heuristic cost

    public bool onOpenList = false;     //keeps track if this node is a part of the open list or not
    public bool onClosedList = false;   //keeps track if this node is a part of the closed list or not

    public int CompareTo(Node othernode)//utilised to help with sorting and creating a priority queue.
    {
        if (f < othernode.f)
        {
            return -1;
        }
        else if (f > othernode.f)
        {
            return 1;
        }
        return 0;
    }

    public void Reset()
    {
        parent = null;
        f = 0;
        g = 0;
        h = 0;
    }
}
