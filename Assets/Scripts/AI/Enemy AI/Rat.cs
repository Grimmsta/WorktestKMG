using UnityEngine;
using UnityEngine.AI;

public class Rat : MonoBehaviour
{
    private PlayerMovementController player;

    public PlayerMovementController Player => player;

    private NavMeshAgent navMesh;
    public NavMeshAgent NavMesh => navMesh;

    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        player = PlayerMovementController.Player;
    }
}
