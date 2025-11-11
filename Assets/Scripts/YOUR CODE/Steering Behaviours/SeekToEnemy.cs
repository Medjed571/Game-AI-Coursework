using UnityEngine;

/// <summary>
/// when active on an agent, said agent will move towards the closest enemy.
/// </summary>

public class SeekToEnemy : SteeringBehaviour
{
    public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
    {
        Vector3 targetPosition = ; //gets the target position from the closest enemy.

        desiredVelocity = Vector3.Normalize(targetPosition - transform.position) * SteeringAgent.MaxCurrentSpeed; //gets the velocity and caps it to the max current speed.

        steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity; //calculates the steering velocity.
        return steeringVelocity;
    }
}
