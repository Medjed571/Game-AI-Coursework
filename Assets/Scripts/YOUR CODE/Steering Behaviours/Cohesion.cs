using UnityEngine;

public class Cohesion : SteeringBehaviour
{
	private int neighbourDistance = 4; //line of sight for neighbour detection.
	private int count = 0; //amount of neighbouring agents.
	private Vector3 totalPositions = Vector3.zero;

	private float weight = 0.5f;

	public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
	{
		count = 0;
		totalPositions = Vector3.zero;

		for(var i = 0; i < GameData.Instance.allies.Count; i++)
        {
			var a = GameData.Instance.allies[i]; //each individual ally agent
			Vector3 pullForce = transform.position - a.transform.position;
			float distance = pullForce.magnitude;

			if (distance > 0 && distance < neighbourDistance) //if in line of sight
            {
				count++;
				totalPositions += a.CurrentVelocity.normalized;
            }
        }

		if (count > 0) //if there are neighbours
		{
			Vector3 avgPos = totalPositions / count;
			desiredVelocity = Vector3.Normalize(avgPos - transform.position) * SteeringAgent.MaxCurrentSpeed;
			steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;

			return steeringVelocity * weight;
		}
		return Vector3.zero;
	}
}
