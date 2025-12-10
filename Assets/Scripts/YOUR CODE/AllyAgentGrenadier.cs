using UnityEngine;
using System.Collections.Generic;

public class AllyAgentGrenadier : SteeringAgent
{
	private SteeringBehaviour sbAlignment;
	private SteeringBehaviour sbSeparation;
	private SteeringBehaviour sbCohesion;
	private SteeringBehaviour sbEvade;
	private SteeringBehaviour sbPathFollow;
	private SteeringBehaviour sbWander;

	private SteeringAgent closestEnemy;

	private float distanceToEnemy; //the distance between the agent and closest enemy
	private float sightRadius = 30f; //the line of sight that the agent can see and shoot from
	private bool isBattleshocked = true; //Couldnt think of a better name. Has a chance to toggle false when the agent is low health. When off, evade behaviour is switched off. based on the warhammer 40k battleshock mechanic.

	private bool keyBehavioursActive = true; //some behaviours that will only switch off in specific conditions (simplifies decision tree)

	/// stores all the steering behaviours
	private List<SteeringBehaviour> steeringBehvaiours = new List<SteeringBehaviour>();

	protected override void InitialiseFromAwake()
	{
		sbAlignment = gameObject.AddComponent<Alignment>();
		sbSeparation = gameObject.AddComponent<Separation>();
		sbCohesion = gameObject.AddComponent<Cohesion>();
		sbEvade = gameObject.AddComponent<Evade>();
		sbPathFollow = gameObject.AddComponent<PathFollow>();
		sbWander = gameObject.AddComponent<Wander>();

		sbWander.enabled = false; //initially wont be wandering
		sbEvade.enabled = false; //initially wont be evasive
	}

	protected override void CooperativeArbitration()
	{
		base.CooperativeArbitration();

		if (keyBehavioursActive == true)
		{
			sbAlignment.enabled = true;
			sbSeparation.enabled = true;
			sbCohesion.enabled = true;
			sbPathFollow.enabled = true;
		}
		else
		{
			sbAlignment.enabled = false;
			sbSeparation.enabled = false;
			sbCohesion.enabled = false;
			sbPathFollow.enabled = false;
		}

		closestEnemy = GetNearestAgent(transform.position, GameData.Instance.enemies);
		if (closestEnemy != null) //if closestEnemy returns a position
			distanceToEnemy = (closestEnemy.transform.position - transform.position).magnitude;
		else //if closestEnemy does not return a position (if there are no more enemies in the stage)
		{
			distanceToEnemy = sightRadius + 1; //set the distance to enemy *just* out of sight. (so the agent has no reason to fire.)
			sbWander.enabled = true;
			keyBehavioursActive = false;
		}

		///Health and Self Preservation
		if (Health < 0.25f && isBattleshocked == true && Random.value <= 0.01f) //if at 1/4 health & hasnt recovered from battleshock & they passed a check
		{
			if (Random.value <= 0.01f) //rare chance to overcome fear behaviour
			{
				sbEvade.enabled = false; //no longer evading fights
				sbWander.enabled = false; //no longer aimlessly wandering

				keyBehavioursActive = true; //agent falls back into coherency

				isBattleshocked = false; //no longer battleshocked
			}
			sbEvade.enabled = true; //the agent flees
			sbWander.enabled = true; //in a random direction
			keyBehavioursActive = false; //and falls out of unit coherency
		}

		///Aggression
		if (TimeToNextAttack <= 0 && closestEnemy != null && distanceToEnemy < sightRadius) //if enemy is close enough to fire at and an enemy exists
		{
			if (GameData.Instance.EnemyRocketsAvailable > 0 && Random.value <= 0.001f)
			{
				AttackWith(Attack.AttackType.Rocket);
			}
			else
				AttackWith(Attack.AttackType.AllyGun);
		}

		SteeringVelocity = Vector3.zero;

		//the weights of each steering behaviour is determined in their scripts, at the end of each one their steering behaviour is multiplied by a specific weight.
		//after that they are just added together as normal.
		GetComponents<SteeringBehaviour>(steeringBehvaiours);
		foreach (SteeringBehaviour currentBehaviour in steeringBehvaiours) //for each steering behaviour
		{
			if (currentBehaviour.enabled) //take the behaviours enabled
			{
				SteeringVelocity += currentBehaviour.UpdateBehaviour(this); //add each velocity
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
