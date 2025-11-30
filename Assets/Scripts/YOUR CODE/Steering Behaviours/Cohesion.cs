using UnityEngine;

public class Cohesion : SteeringBehaviour
{
	int count = 1; //neighbours count
	float maxCohesion = 5f;

	public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
	{
		var centreOfMass = transform.position; //agent position

		for(var i = 0; i < GameData.Instance.allies.Count; i++)
        {
			var a = GameData.Instance.allies[i];
			if (a != this)
            {
				var distance = Vector3.SqrMagnitude(a.transform.position);
				if (distance < maxCohesion)
                {
					centreOfMass = centreOfMass + a.transform.position;
					count++;
                }
            }
        }

		if (count == 1)
        {
			steeringVelocity = Vector3.zero;
			return steeringVelocity;
		}

		centreOfMass = centreOfMass / count;
		steeringVelocity = centreOfMass;

		return steeringVelocity;
	}
}
