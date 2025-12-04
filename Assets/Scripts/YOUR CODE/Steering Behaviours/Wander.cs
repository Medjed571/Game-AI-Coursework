using UnityEngine;

public class Wander : SteeringBehaviour
{
	protected float sideViewAngle = 30f;

	protected float circleRadius = 5.0f;

	protected float circleDistance = 10.0f;

	protected float maxRandomDisplacement = 0.2f;

	private Vector3 previousTargetPosition;

	protected override void Start()
	{
		base.Start();
	}

	public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
	{
		Vector3 targetPosition = RandomPointOnUnitCircleCircumference();

		targetPosition *= maxRandomDisplacement;
		targetPosition += previousTargetPosition;

		Vector3 circlePosition = transform.position + (transform.up * circleDistance);
		targetPosition = circlePosition + (Vector3.Normalize(targetPosition - circlePosition) * circleRadius);

		previousTargetPosition = targetPosition;

		desiredVelocity = Vector3.Normalize(targetPosition - transform.position) * SteeringAgent.MaxCurrentSpeed;

		steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;
		return steeringVelocity;
	}

	protected static Vector3 RandomPointOnUnitCircleCircumference()
	{
		float randomAngle = Random.value * (2.0f * Mathf.PI);
		return new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0.0f);
	}

	public override void DebugDraw(SteeringAgent steeringAgent)
	{
		Vector3 circlePosition = transform.position + (transform.up * circleDistance);
		DebugDrawCircle("DebugCircle " + GetType().Name, circlePosition, circleRadius, Color.magenta);

		base.DebugDraw(steeringAgent);
	}
}
