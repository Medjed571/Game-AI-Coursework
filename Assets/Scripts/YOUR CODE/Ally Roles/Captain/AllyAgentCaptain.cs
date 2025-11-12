using UnityEngine;

public class AllyAgentCaptain : SteeringAgent
{
	//private Attack.AttackType attackType = Attack.AttackType.AllyGun;
	protected override void InitialiseFromAwake()
	{
		gameObject.AddComponent<SeekToMouse>();
	}

	protected override void CooperativeArbitration()
	{
		base.CooperativeArbitration();


	}
}
