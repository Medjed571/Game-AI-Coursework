using UnityEngine;

public class AgentGuard : SteeringAgent
{
    private Attack.AttackType attackType = Attack.AttackType.AllyGun;

    protected override void InitialiseFromAwake()
    {
        
    }

    protected override void CooperativeArbitration()
    {
        base.CooperativeArbitration();
    }
}
