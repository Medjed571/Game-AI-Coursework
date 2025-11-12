using UnityEngine;

/// <summary>
/// for as long as the script is active, the agent will attempt to find and then move to the coordinates of the captain ally agent.
/// if there is no captain ally agent on the scene then another script deactivates this and activates berseker scripts.
/// </summary>

public class SeekCaptain : SteeringBehaviour
{
	private Vector3 captainLocation;

	private float timerInterval = 0.5f; //works in InvokeRepeating to cause a 0.5 second delay between each captain location get.

    protected override void Start()
    {
        base.Start();
		//repeatedly calls UpdateCaptainLocation every few moments based on the timer delay.
		InvokeRepeating(nameof(UpdateCaptainLocation), 0.5f, timerInterval);
	}

    public override Vector3 UpdateBehaviour(SteeringAgent steeringAgent)
	{
		// Get the target position from the mouse input
		Vector3 targetPos = captainLocation;

		// Get the desired velocity for seek and limit to maxSpeed
		desiredVelocity = Vector3.Normalize(targetPos - transform.position) * SteeringAgent.MaxCurrentSpeed;

		// Calculate steering velocity
		steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;
		return steeringVelocity;
	}

	void UpdateCaptainLocation()
    {
		captainLocation = GameObject.Find("Ally 0").transform.position;
		Debug.Log(captainLocation);
	}
}
