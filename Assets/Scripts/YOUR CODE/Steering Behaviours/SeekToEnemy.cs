using UnityEngine;

/// <summary>
/// when active on an agent, said agent will move towards the closest enemy.
/// </summary>

public class SeekToEnemy : SteeringBehaviour
{
    private EnemySeekPriority target; //references what to actually seek

    public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
    {
        target.ChosenEnemy();

        Vector3 targetPosition = target.chosenEnemy; //gets the target position from the closest enemy.

        desiredVelocity = Vector3.Normalize(targetPosition - transform.position) * SteeringAgent.MaxCurrentSpeed; //gets the velocity and caps it to the max current speed.

        //calculates the steering velocity.
        steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;
        return steeringVelocity;
    }
}
