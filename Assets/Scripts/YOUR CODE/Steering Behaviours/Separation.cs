using UnityEngine;

public class Separation : SteeringBehaviour
{
    private float neighbourDistance = 2f; //line of sight for neighbour detection.
    private float separationDistance = 3f;
    private int count = 0; //amount of neighbouring agents.
    private Vector3 totalForce; //force to move away from neighbouring agents 

    public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
    {
        for (int i = 0; i < GameData.Instance.allies.Count; i++) //for each friendly agent in the scene
        {
            var a = GameData.Instance.allies[i]; //each individual ally agent
            if (a != gameObject) //if the agent isnt *this* agent
            {
                if ((transform.position - a.transform.position).magnitude < neighbourDistance && (transform.position - a.transform.position).magnitude > 0) //if the agent is in the cone of vision
                {
                    var pushForce = transform.position - a.transform.position;

                    totalForce += (pushForce / neighbourDistance) * separationDistance;
                    count++;
                }
            }
        }

        desiredVelocity = Vector3.Normalize(transform.position - totalForce) * SteeringAgent.MaxCurrentSpeed;
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
