using UnityEngine;
using System.Collections.Generic;

public class AllyAgentScout : SteeringAgent
{
	SteeringBehaviour sbAlignment;
	SteeringBehaviour sbSeparation;
	SteeringBehaviour sbCohesion;
	SteeringBehaviour sbEvade;
	SteeringBehaviour sbPathFollow;
	//SteeringBehaviour sbWander;

	SteeringAgent closestEnemy;

	float distanceToEnemy;
	private bool isBattleshocked = true; //Couldnt think of a better name. Has a chance to toggle false when the agent is low health. When off, evade behaviour is switched off.

	/// stores all the steering behaviours
	private List<SteeringBehaviour> steeringBehvaiours = new List<SteeringBehaviour>();

	protected override void InitialiseFromAwake()
	{
		sbAlignment = gameObject.AddComponent<Alignment>();
		sbSeparation = gameObject.AddComponent<Separation>();
		sbCohesion = gameObject.AddComponent<Cohesion>();
		//sbEvade = gameObject.AddComponent<Evade>();
		sbPathFollow = gameObject.AddComponent<PathFollow>();
		//sbWander = gameObject.AddComponent<Wander>();

		//sbWander.enabled = false;
		//sbEvade.enabled = false;
	}

    protected override void CooperativeArbitration()
    {
        base.CooperativeArbitration();

		closestEnemy = GetNearestAgent(transform.position, GameData.Instance.enemies);
		distanceToEnemy = (closestEnemy.transform.position - transform.position).magnitude;

		///Health and Self Preservation
		if (Health < 0.25f && isBattleshocked == true) //if at 1/4 health and the agent hasnt 
		{
			if (Random.value <= 0.0001f)
			{
				Debug.Log("Passed Battleshock");
				//sbEvade.enabled = false; //no longer evading fights
				//isBattleshocked = false; //no longer battleshocked
			}
			//sbEvade.enabled = true; //flee
		}

		///Aggression
		if (TimeToNextAttack <= 0 && closestEnemy != null && distanceToEnemy < 15f) //if enemy is close enough to fire at and an enemy exists
        {
			AttackWith(Attack.AttackType.AllyGun);
        }

		///Flocking (Weighted Blending)
		Vector3 alignmentVect = sbAlignment.transform.position;
		Vector3 cohesionVect = sbCohesion.transform.position;
		Vector3 separationVect = sbSeparation.transform.position;
		float flockingX = (alignmentVect.x * 0.25f) + (cohesionVect.x * 0.5f) + (separationVect.x * 0.25f);
		float flockingY = (alignmentVect.y * 0.25f) + (cohesionVect.y * 0.5f) + (separationVect.x * 0.25f);
		Vector3 flockingVect = new Vector3(flockingX, flockingY, 0f);
		//Debug.Log(flockingVect);
		SteeringVelocity = Vector3.zero;

		GetComponents<SteeringBehaviour>(steeringBehvaiours);
		foreach (SteeringBehaviour currentBehaviour in steeringBehvaiours) //for each steering behaviour
		{
			if (currentBehaviour.enabled) //take the behaviours enabled
			{
				SteeringVelocity += flockingVect.normalized; //add each velocity
			}
		}
	}

	protected override void UpdateDirection()
	{
		base.UpdateDirection();

		if (closestEnemy == null)
		{
			return;
		}

		var difference = closestEnemy.transform.position - transform.position;
		if (closestEnemy != null && (difference).magnitude < 30.0f)
		{
			transform.up = Vector3.Normalize(new Vector3(difference.x, difference.y, 0.0f));
		}
	}
}
