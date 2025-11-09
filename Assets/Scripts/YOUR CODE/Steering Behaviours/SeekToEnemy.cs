using UnityEngine;

/// <summary>
/// when active on an agent, said agent will move towards the closest enemy.
/// </summary>

public class SeekToEnemy : SteeringBehaviour
{
    private Targetting targetting; //references 

    public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
    {
        //gets the target position from the closest enemy.
        Vector3 targetPosition = targetting.ClosestEnemy;
        
        //gets the velocity and caps it to the max current speed.
        desiredVelocity = Vector3.Normalize(targetPosition - transform.position) * SteeringAgent.MaxCurrentSpeed;

        //calculates the steering velocity.
        steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;
        return steeringVelocity;
    }
}
