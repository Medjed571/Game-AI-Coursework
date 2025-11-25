using UnityEngine;

public class AgentPathfinding : MonoBehaviour
{
    private void Start()
    {
        this.GetComponent<PathfindingAStar>().AStarSearch(this, target);
    }

    void Update()
    {
        
    }
}
