using UnityEngine;

public class Pursue : SteeringBehaviour
{
	private Map map;
	private Pathfinding pathfinding;

    public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
    {
		var target = SteeringAgent.GetNearestAgent(transform.position, GameData.Instance.enemies);
		int agentNode = map.MapIndex((int)transform.position.x, (int)transform.position.y);
		int targetNode = map.MapIndex((int)target.transform.position.x, (int)target.transform.position.y);

		pathfinding.AStarPathfind(agentNode, targetNode);

		var nearestEnemy = ;

		Vector3 targetPosition = nearestEnemy.transform.position + nearestEnemy.CurrentVelocity;
		targetPosition.z = 0.0f;

		// Get the desired velocity for seek and limit to maxSpeed
		desiredVelocity = Vector3.Normalize(targetPosition - transform.position) * SteeringAgent.MaxCurrentSpeed;

		// Calculate steering velocity
		steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;
		return steeringVelocity;
	}
}
