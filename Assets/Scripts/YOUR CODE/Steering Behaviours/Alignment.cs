using UnityEngine;

public class Alignment : SteeringBehaviour
{
    private float neighbourDistance = 3f; //line of sight for neighbour detection.
    private int count = 0; //amount of neighbouring agents.
    private Vector3 sumTotal = Vector3.zero; //the average coordinates of all the agents to align towards.

    public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
    {
        for (int i = 0; i < GameData.Instance.allies.Count; i++) //for each friendly agent in the scene
        {
            var a = GameData.Instance.allies[i]; //each individual ally agent
            if (a != gameObject) //if the ally agent is not *this* ally agent
            {
                if ((a.transform.position).magnitude < neighbourDistance)//if a neighbour is currently close to the agent
                {
                    sumTotal += Vector3.Normalize(a.CurrentVelocity);
                    count++;
                }
            }
        }

        if (count == 0) //if there are no neighbors nearby
        {
            steeringVelocity = Vector3.zero;
            return steeringVelocity;
        }

        sumTotal /= count;

        Vector3 targetPosition = sumTotal;
        
        desiredVelocity = Vector3.Normalize(targetPosition - transform.position) * SteeringAgent.MaxCurrentSpeed;

        steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;

        count = 0;

        return steeringVelocity;
    }
}
