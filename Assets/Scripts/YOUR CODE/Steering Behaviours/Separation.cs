using UnityEngine;

public class Separation : SteeringBehaviour
{
    private float neighbourDistance = 4f; //line of sight for neighbour detection.
    private float separationDistance = 1f;
    private int count = 0; //amount of neighbouring agents.
    private Vector3 totalForce; //force to move away from neighbouring agents 

    public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
    {
        for (int i = 0; i < GameData.Instance.allies.Count; i++) //for each friendly agent in the scene
        {
            var a = GameData.Instance.allies[i]; //each individual ally agent
            if (a != gameObject) //if the agent isnt *this* agent
            {
                float distance = Vector3.Distance(transform.position, a.transform.position); //determine the distance between them
                if (distance < separationDistance && distance > 0)
                {
                    var pushForce = transform.position - a.transform.position;

                    totalForce += (pushForce / neighbourDistance);
                    count++;
                }
            }
        }

        desiredVelocity = Vector3.Normalize(totalForce - transform.position) * SteeringAgent.MaxCurrentSpeed;
        steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;

        if (count == 0) //if there are no neighbors nearby
        {
            steeringVelocity = Vector3.zero;
            return steeringVelocity;
        }

        steeringVelocity /= count;

        count = 0;

        return steeringVelocity;
    }
}
