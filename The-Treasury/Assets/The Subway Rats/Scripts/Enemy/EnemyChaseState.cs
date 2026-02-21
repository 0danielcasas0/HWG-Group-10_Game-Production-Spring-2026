using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Entered Chase State");
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
    }

    public override void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("Exiting Chase State");
    }
}
