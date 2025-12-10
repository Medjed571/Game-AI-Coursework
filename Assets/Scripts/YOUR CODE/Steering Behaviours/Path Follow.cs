using UnityEngine;
using System.Collections.Generic;

public class PathFollow : SteeringBehaviour
{
	private SteeringAgent target;

	private Pathfinding pathfinding;

	private List<Node> targetNodeList;
	private int pathListNumber;

	private float weight = 1.25f;

	protected override void Start()
    {
        base.Start();
		pathfinding = gameObject.AddComponent<Pathfinding>();
		targetNodeList = GetNodeList();
	}

	public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
	{
		if (target == null) //if there are no enemies left in the scene just return it
        {
			return steeringVelocity;
		}

		Vector3 targetPosition = new Vector3(targetNodeList[pathListNumber].x, targetNodeList[pathListNumber].y, 0f);
		if((targetPosition - transform.position).magnitude < 0.5f) //if the agent is close to the closest node
        {
			pathListNumber++; //begin moving towards the next
			if(pathListNumber == targetNodeList.Count) //if the agent is at the target
            {
				pathListNumber = 0; //reset the list to emulate an evasive movement

				if (target.Health <= 0) //if the target is dead
				{
					targetNodeList = GetNodeList(); //select a new target
				}
			}
		}

        // Get the desired velocity for seek and limit to maxSpeed
        desiredVelocity = Vector3.Normalize(targetPosition - transform.position) * SteeringAgent.MaxCurrentSpeed;

		// Calculate steering velocity
		steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;
		return steeringVelocity * weight;
	}

	private List<Node> GetNodeList()
	{
		target = SteeringAgent.GetNearestAgent(transform.position, GameData.Instance.enemies); //finds the nearest target

		if (target != null)
		{
			Vector3 targetCoord = target.transform.position; //find the target position
			List<Node> path = pathfinding.ExecuteAlgorithm(targetCoord); //find a path towards it
			return path;
		}
		return null;
    }
}
