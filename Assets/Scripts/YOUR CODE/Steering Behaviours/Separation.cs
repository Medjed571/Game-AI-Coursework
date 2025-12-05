using UnityEngine;

public class Separation : SteeringBehaviour
{
    private int neighbourDistance = 2; //line of sight for neighbour detection.
    private int count = 0; //amount of neighbouring agents.
    private Vector3 totalForce = Vector3.zero; //force to move away from neighbouring agents 

    private float weight = 0.25f;

    public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
    {
        count = 0;
        totalForce = Vector3.zero;

        for(int i = 0; i < GameData.Instance.allies.Count; i++) //for each friendly agent in the scene
        {
            var a = GameData.Instance.allies[i]; //each individual ally agent
            Vector3 pushForce = transform.position - a.transform.position;
            float distance = pushForce.magnitude;

            if (distance > 0 && distance < neighbourDistance) //if the agent is in the cone of vision
            {
                pushForce.Normalize();
                pushForce /= distance;
                totalForce += pushForce;
                count++;
            }

        }

        if (count > 0) //if there are neighbours
        {
            Vector3 avgCoord = totalForce / count;
            desiredVelocity = Vector3.Normalize(avgCoord - transform.position) * SteeringAgent.MaxCurrentSpeed;
            steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;

            return steeringVelocity * weight;
        }
        return Vector3.zero;
    }
}
