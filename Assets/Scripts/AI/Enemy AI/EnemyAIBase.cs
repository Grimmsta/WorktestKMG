using UnityEngine;

public class EnemyAIBase : StateMachineBehaviour
{
    protected PlayerMovementController player;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CheckDistanceToPlayer(animator);
    }

    protected PlayerMovementController GetPlayer(Animator animator)
    {
        return animator.GetComponent<Rat>().Player;
    }

    protected void ReturnToIdle(Animator animator)
    {
        animator.SetInteger(Transitions.stateName, (int)Transitions.MovementStates.IDLE);
    }
    protected void CheckDistanceToPlayer(Animator animator)
    {
        if (Vector3.Distance(GetPlayer(animator).transform.position, animator.transform.position) <= 2)
        {
            animator.SetInteger(Transitions.stateName, (int)Transitions.MovementStates.MOVEAWAY);
        }
    }
}