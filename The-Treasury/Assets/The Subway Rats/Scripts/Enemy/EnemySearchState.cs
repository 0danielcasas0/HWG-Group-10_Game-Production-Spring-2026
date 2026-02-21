using UnityEngine;

public class EnemySearchState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Entered Search State");
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
    }

    public override void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("Exiting Search State");
    }
}
