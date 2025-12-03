using UnityEngine;
using System.Collections.Generic;

public class AllyAgentScout : SteeringAgent
{
	SteeringBehaviour sbAlignment;
	SteeringBehaviour sbSeparation;
	SteeringBehaviour sbCohesion;
	SteeringBehaviour sbPursue;
	SteeringBehaviour sbEvade;
	SteeringBehaviour sbPathFollow;
	SteeringBehaviour sbWander;

	SteeringAgent closestEnemy;

	float distanceToEnemy;
	private bool isBattleshocked = true; //Couldnt think of a better name. Has a chance to toggle false when the agent is low health. When off, evade behaviour is switched off.

	/// stores all the steering behaviours
	private List<SteeringBehaviour> steeringBehvaiours = new List<SteeringBehaviour>();

	protected override void InitialiseFromAwake()
	{
		//sbAlignment = gameObject.AddComponent<Alignment>();
		//sbSeparation = gameObject.AddComponent<Separation>();
		//sbCohesion = gameObject.AddComponent<Cohesion>();
		//sbPursue = gameObject.AddComponent<Pursue>();
		sbEvade = gameObject.AddComponent<Evade>();
		sbPathFollow = gameObject.AddComponent<PathFollow>();
		sbWander = gameObject.AddComponent<Wander>();

		sbEvade.enabled = false;
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
				sbEvade.enabled = false; //no longer evading fights
				isBattleshocked = false; //no longer battleshocked
			}
			//sbEvade.enabled = true; //flee
		}

		///Aggression
		if (TimeToNextAttack <= 0 && closestEnemy != null) //if enemy is close enough to fire at and an enemy exists
        {
			if (distanceToEnemy < 15f) //if the enemy is in shooting range
			{
				sbWander.enabled = false;
				AttackWith(Attack.AttackType.AllyGun);
			}
        }

		///Roaming
		if (closestEnemy = null)
        {
			//sbWander.enabled = true;
        }

        SteeringVelocity = Vector3.zero;

		GetComponents<SteeringBehaviour>(steeringBehvaiours);
		foreach (SteeringBehaviour currentBehaviour in steeringBehvaiours) //for each steering behaviour
		{
			if (currentBehaviour.enabled) //take the behaviours enabled
			{
				SteeringVelocity += currentBehaviour.UpdateBehaviour(this); //add each velocity and divide by the amount of behaviours active.
				Debug.Log(SteeringVelocity);
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
