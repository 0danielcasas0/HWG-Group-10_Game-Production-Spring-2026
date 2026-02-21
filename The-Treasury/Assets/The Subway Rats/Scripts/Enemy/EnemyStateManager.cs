using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    EnemyBaseState currentState;
    EnemyPatrolState patrolState = new EnemyPatrolState();
    EnemyChaseState chaseState = new EnemyChaseState();
    EnemySearchState searchState = new EnemySearchState();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set the initial state to Patrol
        currentState = patrolState;
        // Call the EnterState method of the initial state
        currentState.EnterState(this);  
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(EnemyBaseState State)
    {
        // Call the ExitState method of the current state
        currentState.ExitState(this);
        // Switch to the new state
        currentState = State;
        // Call the EnterState method of the new state
        currentState.EnterState(this);
    }
}
