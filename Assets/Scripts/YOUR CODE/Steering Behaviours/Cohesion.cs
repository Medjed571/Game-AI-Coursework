using UnityEngine;

public class Cohesion : SteeringBehaviour
{
	int count = 1; //neighbours count
	float maxCohesion = 5f; //range where agents will start to group up

	public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
	{
		var centreOfMass = transform.position; //agent position

		for(var i = 0; i < GameData.Instance.allies.Count; i++)
        {
			var a = GameData.Instance.allies[i];
			if (a != gameObject)
            {
				var distance = Vector3.SqrMagnitude(a.transform.position);
				if (distance < maxCohesion)
                {
					centreOfMass += a.transform.position;
					count++;
                }
            }
        }

		desiredVelocity = Vector3.Normalize(centreOfMass - transform.position) * SteeringAgent.MaxCurrentSpeed;
		steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;

		if (count == 1)
        {
			steeringVelocity = Vector3.zero;
			return steeringVelocity;
		}

		centreOfMass /= count;
		steeringVelocity = centreOfMass;

		return steeringVelocity;
	}
}
