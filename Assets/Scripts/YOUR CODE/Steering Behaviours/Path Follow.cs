using UnityEngine;
using System.Collections.Generic;

public class PathFollow : SteeringBehaviour
{
	private SteeringAgent target;

	private Pathfinding pathfinding;

	private List<Node> targetNodeList;
	private int pathListNumber;

    protected override void Start()
    {
        base.Start();
		pathfinding = gameObject.AddComponent<Pathfinding>();
		targetNodeList = GetNodeList();
	}

	public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
	{
		if (targetNodeList.Count <= 0) //if there are no enemies left in the scene
        {
			return steeringVelocity;
        }

		Vector3 targetPosition = new Vector3(targetNodeList[pathListNumber].x, targetNodeList[pathListNumber].y, 0f);
		if((transform.position - targetPosition).magnitude < 0.5f) //if the agent is close to the closest node
        {
			pathListNumber++;
			if(pathListNumber == targetNodeList.Count) //move up the list
            {
				pathListNumber = targetNodeList.Count - 1; //account for lists being 1-X
			}
		}

        if (target.Health <= 0)//|| (target.transform.position - transform.position).magnitude < 1f)//if the current enemy has died or if the agent is close enough to the current target.
        {
			target = null;
            pathListNumber = 0;
            targetNodeList = GetNodeList();//get a new enemy to follow
        }

        // Get the desired velocity for seek and limit to maxSpeed
        desiredVelocity = Vector3.Normalize(targetPosition - transform.position) * SteeringAgent.MaxCurrentSpeed;

		// Calculate steering velocity
		steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;
		return steeringVelocity;
	}

	private List<Node> GetNodeList()
    {
		target = SteeringAgent.GetNearestAgent(transform.position, GameData.Instance.enemies);

		Vector3 targetCoord = target.transform.position;

		List<Node> path = pathfinding.ExecuteAlgorithm(targetCoord);

		return path;
    }
}
