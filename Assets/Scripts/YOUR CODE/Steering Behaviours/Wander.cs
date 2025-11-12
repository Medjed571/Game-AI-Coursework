using UnityEngine;

/// <summary>
/// used primarily by the captain in order to wander around the scene when not in direct combat or on the way to combat.
/// </summary>

public class Wander : SteeringBehaviour
{
    public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
    {
        Vector3 targetPosition = RandomPointOnUnitCircleCircumference();

        return steeringVelocity;
    }

    protected static Vector3 RandomPointOnUnitCircleCircumference()
    {
        float randomAngle = Random.value * (2f * Mathf.PI);
        return new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0f);
    }
}
