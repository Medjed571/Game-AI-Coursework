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
	SteeringBehaviour sbPathFollow;

	SteeringAgent closestEnemy;

	/// stores all the steering behaviours
	private List<SteeringBehaviour> steeringBehvaiours = new List<SteeringBehaviour>();

	protected override void InitialiseFromAwake()
	{
		sbAlignment = gameObject.AddComponent<Alignment>();
		sbSeparation = gameObject.AddComponent<Separation>();
		sbCohesion = gameObject.AddComponent<Cohesion>();
		//sbPursue = gameObject.AddComponent<Pursue>();
		//sbEvade = gameObject.AddComponent<Evade>();
		sbPathFollow = gameObject.AddComponent<PathFollow>();

		//sbPursue.enabled = false;
		//sbEvade.enabled = false;
		sbPathFollow.enabled = false;
	}

    protected override void CooperativeArbitration()
    {
        base.CooperativeArbitration();

        var activeBehaviours = new List<SteeringBehaviour>();
        if (closestEnemy == null) //if no enemy has been selected
        {
            LocateNearestEnemy();
        }

        float closestEnemyDistance = (transform.position - closestEnemy.transform.position).sqrMagnitude;

        if (closestEnemyDistance < shootRange) //if enemy is close enough to fire at		
        {
            //if enemy is visible
            if (Health < 0.25f) //frail agents, will always flee if health is below threshhold
            {
                //sbEvade.enabled = true;
            }
            else
            {
                AttackWith(attackType);
            }
        }
        else
        {
            sbPathFollow.enabled = true;
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
