using UnityEngine;

public class AllyAgentScout : SteeringAgent
{
	protected override void InitialiseFromAwake()
	{
		gameObject.AddComponent<Pursue>();
	}
}
