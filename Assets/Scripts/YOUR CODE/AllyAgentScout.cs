using UnityEngine;

public class AllyAgentScout : SteeringAgent
{
	protected override void InitialiseFromAwake()
	{
		gameObject.AddComponent<Alignment>();
		gameObject.AddComponent<Separation>();
		gameObject.AddComponent<Cohesion>();
		gameObject.AddComponent<Pursue>();
	}
}
