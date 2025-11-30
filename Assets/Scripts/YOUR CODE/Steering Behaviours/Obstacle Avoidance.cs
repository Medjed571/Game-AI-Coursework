using UnityEngine;

public class ObstacleAvoidance : SteeringBehaviour
{
	private float detectionRadius = 4f;
	private Vector3 distanceAhead;
	private Vector3 distanceAhead2;

	public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
	{
		var nearestEnemy = SteeringAgent.GetNearestAgent(transform.position, GameData.Instance.enemies);
		distanceAhead = transform.position + Vector3.Normalize(steeringAgent.CurrentVelocity) * detectionRadius;
		distanceAhead2 = transform.position + Vector3.Normalize(steeringAgent.CurrentVelocity) * detectionRadius * 0.5f;
		//var avoidanceForce = distanceAhead - 

		Vector3 targetPosition = nearestEnemy.transform.position + nearestEnemy.CurrentVelocity;
		targetPosition.z = 0.0f;

		// Get the desired velocity for seek and limit to maxSpeed
		desiredVelocity = Vector3.Normalize(targetPosition - transform.position) * SteeringAgent.MaxCurrentSpeed;

		// Calculate steering velocity
		steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;
		return steeringVelocity;
	}
	float distance(Vector3 mapNode, Vector3 detectionPoint)
	{
		return Mathf.Sqrt((mapNode.x - detectionPoint.x) * (mapNode.x - detectionPoint.x) + (mapNode.y - detectionPoint.y) * (mapNode.y - detectionPoint.y));
	}

	bool lineIntersectsNode(Vector3 pointA, Vector3 pointB, Vector3 mapNode)
    {
		return distance(mapNode, distanceAhead) <= 0.5f || distance(mapNode, distanceAhead2) <= 0.5f;
    }
}
