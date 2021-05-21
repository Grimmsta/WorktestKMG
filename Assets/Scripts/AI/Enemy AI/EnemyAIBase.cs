using UnityEngine;

public class EnemyAIBase : StateMachineBehaviour
{
    protected PlayerMovementController player;

    protected PlayerMovementController GetPlayer(Animator animator)
    {
        return animator.GetComponent<Rat>().Player;
    }
}