using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Entered Patrol State");
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        
    }

    public override void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("Exiting Patrol State");
    }
}
