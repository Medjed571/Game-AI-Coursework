using UnityEngine;

public class AllyAgentScout : SteeringAgent
{
    //private Attack.AttackType attackType = Attack.AttackType.AllyGun;
	protected override void InitialiseFromAwake()
	{
		gameObject.AddComponent<SeekToEnemy>();
		gameObject.AddComponent<EnemySeekPriority>();
	}

	protected override void CooperativeArbitration()
	{
		base.CooperativeArbitration();


	}
}
