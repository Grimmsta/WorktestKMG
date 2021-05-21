using UnityEngine;
using UnityEngine.AI;

public class MoveAway : EnemyAIBase
{
    [SerializeField] private float unitsToMoveBack = 2f;

    private Vector3 moveToLocation;
    private NavMeshAgent navMesh;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navMesh = animator.GetComponent<Rat>().NavMesh;
        moveToLocation = (animator.transform.position - GetPlayer(animator).transform.position).normalized * unitsToMoveBack;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navMesh.SetDestination(animator.transform.position + moveToLocation);

        ReturnToIdle(animator);
    }
}
