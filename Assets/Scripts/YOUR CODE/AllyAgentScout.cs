using UnityEngine;
using System.Collections.Generic;

public class AllyAgentScout : SteeringAgent
{
	private Attack.AttackType attackType = Attack.AttackType.AllyGun;

	private float shootRange = 40f;

	SteeringBehaviour sbAlignment;
	SteeringBehaviour sbSeparation;
	SteeringBehaviour sbCohesion;
	SteeringBehaviour sbPursue;
	SteeringBehaviour sbEvade;

	SteeringAgent closestEnemy;

	private float timer = 1f;

	/// stores all the steering behaviours
	private List<SteeringBehaviour> steeringBehvaiours = new List<SteeringBehaviour>();

	protected override void InitialiseFromAwake()
	{
		//sbAlignment = gameObject.AddComponent<Alignment>();
		sbSeparation = gameObject.AddComponent<Separation>();
		//sbCohesion = gameObject.AddComponent<Cohesion>();
		sbPursue = gameObject.AddComponent<Pursue>();
		sbEvade = gameObject.AddComponent<Evade>();

		sbSeparation.enabled = true; 
		sbPursue.enabled = false;
		sbEvade.enabled = false;
	}

    protected override void CooperativeArbitration()
    {
		//base.CooperativeArbitration();

		var activeBehaviours = new List<SteeringBehaviour>();
		if (closestEnemy == null) //if no enemy has been selected
		{
			LocateNearestEnemy();
		}
	
		float closestEnemyDistance = (transform.position - closestEnemy.transform.position).sqrMagnitude;

		Debug.Log(closestEnemyDistance);

		if (closestEnemyDistance < shootRange) //if enemy is close enough to fire at		
		{
			//if enemy is visible
			if (Health < 0.25f) //frail agents, will always flee if health is below threshhold
			{
				sbEvade.enabled = true;
			}
			else
			{
				AttackWith(attackType);
			}
		}
		else
		{
			sbPursue.enabled = true;
		}

		SteeringVelocity = Vector3.zero;

		GetComponents<SteeringBehaviour>(steeringBehvaiours);
		foreach (SteeringBehaviour currentBehaviour in steeringBehvaiours)
		{
			if (currentBehaviour.enabled)
			{
				SteeringVelocity += currentBehaviour.UpdateBehaviour(this);
			}
		}
	}

	private void LocateNearestEnemy()
	{
		SteeringAgent enemySearch = GetNearestAgent(transform.position, GameData.Instance.enemies); //find the nearest enemy
		closestEnemy = enemySearch;
	}

	private void UpdatePosition()
	{
		// Limit steering velocity to supplied maximum so it can be used to adjust current velocity. Ensure to subtract this limnited
		// amount from the current value of the steering velocity so that it decreases as over multiple game frames to reach the target
		SteeringVelocity = LimitVector(SteeringVelocity, MaxSteeringSpeed * Time.deltaTime);

		// Set final velocity
		CurrentVelocity += SteeringVelocity;
		CurrentVelocity = LimitVector(CurrentVelocity, GetMaxSpeedAllowed(this));

		// Apply current velocity amount for this frame
		transform.position += CurrentVelocity * Time.deltaTime;
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
