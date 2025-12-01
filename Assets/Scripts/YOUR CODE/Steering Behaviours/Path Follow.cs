using UnityEngine;
using System.Collections.Generic;

public class PathFollow : SteeringBehaviour
{
	private SteeringAgent target;

	private Pathfinding pathfinding;
	private PathfindingBase pathfindingBase;

    protected override void Start()
    {
        base.Start();
		pathfinding = gameObject.AddComponent<Pathfinding>();
		pathfindingBase = gameObject.AddComponent<PathfindingBase>();
	}

	public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
	{
		var targetNodeList = GetNodeList();

		Debug.Log(targetNodeList);

		Node nearestNode = targetNodeList[0];

		Vector3 targetPosition = new Vector3(nearestNode.x, nearestNode.y);
		targetPosition.z = 0.0f;

		// Get the desired velocity for seek and limit to maxSpeed
		desiredVelocity = Vector3.Normalize(targetPosition - transform.position) * SteeringAgent.MaxCurrentSpeed;

		// Calculate steering velocity
		steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;
		Debug.Log(steeringVelocity);
		return steeringVelocity;
	}

	private List<Node> GetNodeList()
    {
		target = SteeringAgent.GetNearestAgent(transform.position, GameData.Instance.enemies);
		Vector3 targetCoord = target.transform.position;

		List<Node> path = pathfinding.ExecuteAlgorithm(targetCoord);

		Debug.Log(path);

		return path;
    }
}
