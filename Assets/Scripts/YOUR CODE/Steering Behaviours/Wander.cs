using UnityEngine;

/// <summary>
/// used primarily by the captain in order to wander around the scene when not in direct combat or on the way to combat.
/// </summary>

public class Wander : SteeringBehaviour
{
    //controls the size of the small circle 
    protected float circleRadius = 150.0f;
    //controls how far away from the agent the circle is
    protected float circleDistance = 250.0f;
    //how large the displacement can be from frame to frame (bigger number = wider turns)
    protected float maxRandomDisplacement = 25.0f;
    //keeps track of the last position to help make the wandering look more natural
    private Vector3 previousTargetPosition;

    protected override void Start()
    {
        base.Start();
        //set a random starting orientation
        transform.up = RandomPointOnUnitCircleCircumference();
        //set the previous target opposite the starting orientation
        previousTargetPosition = transform.up * (circleDistance + circleRadius);
    }

    public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
    {
        //gets a random point on a circle circumference
        Vector3 targetPosition = RandomPointOnUnitCircleCircumference();

        //scale the direction by the max amount of displacement to get the smaller circle.
        targetPosition *= maxRandomDisplacement;

        //add the previous target location to the target position to stop really janky movements
        targetPosition += previousTargetPosition;

        //get the centre position of the smaller circle and then locks the target position inside of that circle.
        Vector3 circlePosition = transform.position + (transform.up * circleDistance);
        targetPosition = circlePosition + (Vector3.Normalize(targetPosition - circlePosition) * circleRadius);

        //updates previous target position to the new position
        previousTargetPosition = targetPosition;

        //get the desired velocity and limit it to max speed
        desiredVelocity = Vector3.Normalize(targetPosition - transform.position) * SteeringAgent.MaxCurrentSpeed;

        //calculate steering velocity
        steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;

        return steeringVelocity;
    }

    protected static Vector3 RandomPointOnUnitCircleCircumference() //returns a random point on a circle circumference
    {
        //gets a random angle
        float randomAngle = Random.value * (2f * Mathf.PI);
        //gets a point on the circle based on the random angle
        return new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0f);
    }

    public override void DebugDraw(SteeringAgent steeringAgent)
    {
        Vector3 circlePosition = transform.position + (transform.up * circleDistance);
        DebugDrawCircle("DebugCircle " + GetType().Name, circlePosition, circleRadius, Color.magenta);

        base.DebugDraw(steeringAgent);
    }
}
