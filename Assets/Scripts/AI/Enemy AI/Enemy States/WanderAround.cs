using UnityEngine;
using UnityEngine.AI;

public class WanderAround : EnemyAIBase
{
    [SerializeField] float maxWanderDistance = 5;

    private Vector2 posInCircle;
    private Vector3 startPosition;
    private Vector3 moveToLocation;
    private NavMeshAgent navMesh;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navMesh = animator.GetComponent<Rat>().NavMesh;

        posInCircle = Random.insideUnitCircle * maxWanderDistance;
        
        startPosition = animator.transform.position;

        moveToLocation = new Vector3(startPosition.x + posInCircle.x, startPosition.y, startPosition.z + posInCircle.y);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        navMesh.SetDestination(moveToLocation);

        ReturnToIdle(animator);
    }
}
