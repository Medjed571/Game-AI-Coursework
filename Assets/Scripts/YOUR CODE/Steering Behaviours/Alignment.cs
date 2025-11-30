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
            float distance = Vector3.Distance(transform.position, a.transform.position); //determine the distance between them
            if (distance < neighbourDistance)// && a.CurrentVelocity.magnitude > 0) //if a neighbour is currently close to the agent AND is moving
            {
                sumTotal = sumTotal + Vector3.Normalize(a.CurrentVelocity);
                count++;
            }
        }

        if (count == 0) //if there are no neighbors nearby
        {
            steeringVelocity = Vector3.zero;
            return steeringVelocity;
        }

        sumTotal = sumTotal / count;

        Vector3 targetPosition = sumTotal;

        desiredVelocity = targetPosition * SteeringAgent.MaxCurrentSpeed;//(targetPosition - transform.position) * SteeringAgent.MaxCurrentSpeed;
        targetPosition.z = 0.0f;

        steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;

        Debug.Log(sumTotal);
        Debug.Log(count);
        Debug.Log(GameData.Instance.allies.Count);

        count = 0;

        return steeringVelocity;
    }
}
