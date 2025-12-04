using UnityEngine;

public class Alignment : SteeringBehaviour
{
    private int neighbourDistance = 4; //line of sight for neighbour detection.
    private int count = 0; //amount of neighbouring agents.
    private Vector3 totalHeading = Vector3.zero; //the average coordinates of all the agents to align towards.

    public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
    {
        count = 0;
        totalHeading = Vector3.zero;

        for (int i = 0; i < GameData.Instance.allies.Count; i++) //for each friendly agent in the scene
        {
            var a = GameData.Instance.allies[i]; //each individual ally agent
            Vector3 pushForce = transform.position - a.transform.position;
            float distance = pushForce.magnitude;

            Debug.Log("Distance " + distance); 
            
            ///FOR TOMORROW
            ///find a way to make these three scripts only read the current velocity of other agents.
            ///currently it takes from all ally agents in the scene INCLUDING the agent the script is attached to.

            if (distance > 0 && distance < neighbourDistance)//if a neighbour is currently close to the agent
            {
                count++;
                totalHeading += a.CurrentVelocity.normalized;
            }
        }

        if (count > 0) //if there are neighbours
        {
            Vector3 avgHeading = (totalHeading / count);
            desiredVelocity = Vector3.Normalize(avgHeading - transform.position) * SteeringAgent.MaxCurrentSpeed;
            steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;

            return steeringVelocity;
        }
        return Vector3.zero;
    }
}
