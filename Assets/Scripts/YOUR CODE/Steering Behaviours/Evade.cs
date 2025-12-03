using UnityEngine;

public class Evade : SteeringBehaviour
{
	public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
	{
		var nearestEnemy = SteeringAgent.GetNearestAgent(transform.position, GameData.Instance.enemies); ;

		if (nearestEnemy == null) //if there are no new enemies to find
        {
			return steeringVelocity; //just return the steering velocity
        }

		Vector3 targetPosition = nearestEnemy.transform.position + nearestEnemy.CurrentVelocity;
		targetPosition.z = 0.0f;

		// Get the desired velocity for seek and limit to maxSpeed
		desiredVelocity = Vector3.Normalize(transform.position - targetPosition) * SteeringAgent.MaxCurrentSpeed;

		// Calculate steering velocity
		steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;
		return steeringVelocity;
	}
}
