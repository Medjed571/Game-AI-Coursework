using UnityEngine;
using System.Collections.Generic;

public class PathFollow : SteeringBehaviour
{
	private SteeringAgent target;

	private Pathfinding pathfinding;

	private List<Node> targetNodeList;
	private int pathListNumber;

    protected override void Start()
    {
        base.Start();
		pathfinding = gameObject.AddComponent<Pathfinding>();
		targetNodeList = GetNodeList();
	}

	public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
	{
		if(targetNodeList.Count <= 0)
        {
			return steeringVelocity;
        }

		Node nearestNode = targetNodeList[0];

		Vector3 targetPosition = new Vector3(targetNodeList[pathListNumber].x, targetNodeList[pathListNumber].y, 0f);
		if((transform.position - targetPosition).magnitude < 0.2f)
        {
			pathListNumber++;
			if(pathListNumber == targetNodeList.Count)
            {
				pathListNumber = targetNodeList.Count - 1;
			}
		}

		// Get the desired velocity for seek and limit to maxSpeed
		desiredVelocity = Vector3.Normalize(targetPosition - transform.position) * SteeringAgent.MaxCurrentSpeed;

		// Calculate steering velocity
		steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;
		return steeringVelocity;
	}

	private List<Node> GetNodeList()
    {
		target = SteeringAgent.GetNearestAgent(transform.position, GameData.Instance.enemies);
		Vector3 targetCoord = target.transform.position;

		List<Node> path = pathfinding.ExecuteAlgorithm(targetCoord);

		return path;
    }
}
