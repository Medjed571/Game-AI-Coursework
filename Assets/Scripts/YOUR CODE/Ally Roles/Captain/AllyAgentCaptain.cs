using UnityEngine;

public class AllyAgentCaptain : SteeringAgent
{
	//private Attack.AttackType attackType = Attack.AttackType.AllyGun;
	protected override void InitialiseFromAwake()
	{
		gameObject.AddComponent<Wander>();
	}

	protected override void CooperativeArbitration()
	{
		base.CooperativeArbitration();


	}
}
