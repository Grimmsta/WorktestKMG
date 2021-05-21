using UnityEngine;

public class Idle : EnemyAIBase
{
    [SerializeField,
        Tooltip("The minimum distance the rat can have to the player, " +
        "according to government recommendations")] 
        private float minDistanceToPlayer = 2f;

    [SerializeField,
        Tooltip("A range between floats on when the entity should wander, " +
        "to get a more random feeling")] 
    private Vector2 timeUntilWander;

    private float timer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = Random.Range(timeUntilWander.x, timeUntilWander.y);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            animator.SetInteger(Transitions.stateName, (int)Transitions.MovementStates.WANDERAROUND);
        }
    }
}
