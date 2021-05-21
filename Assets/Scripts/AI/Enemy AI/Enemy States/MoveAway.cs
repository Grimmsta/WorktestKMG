using UnityEngine;

public class MoveAway : EnemyAIBase
{
    [SerializeField] private float unitsToMoveBack = 2f;
    [SerializeField] private float moveSpeed = 10f;

    private Vector3 moveToLocation;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        moveToLocation = (animator.transform.position - GetPlayer(animator).transform.position).normalized * unitsToMoveBack;
        Debug.Log((animator.transform.position - GetPlayer(animator).transform.position).normalized);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.position = Vector3.Lerp(animator.transform.position, animator.transform.position + moveToLocation, Time.deltaTime * moveSpeed);

        ReturnToIdle(animator);
    }

    private void ReturnToIdle(Animator animator)
    {
        animator.SetInteger(Transitions.stateName, (int)Transitions.TransitionToState.IDLE);
    }
}
