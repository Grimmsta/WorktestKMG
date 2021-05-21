using UnityEngine;

public class Idle : EnemyAIBase
{
    [SerializeField,
        Tooltip("The minimum distance the rat can have to the player, " +
        "according to government recommendations")] 
        private float minDistanceToPlayer = 2f;

    private PlayerMovementController m_Player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_Player = GetPlayer(animator);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector3.Distance(m_Player.transform.position, animator.transform.position) <= minDistanceToPlayer)
        {
            animator.SetInteger(Transitions.stateName, (int)Transitions.TransitionToState.MOVEAWAY);
        }
    }
}
