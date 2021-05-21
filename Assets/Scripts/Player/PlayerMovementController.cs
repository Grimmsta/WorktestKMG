using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;

    private static PlayerMovementController player;
    public static PlayerMovementController Player => player;

    private void Awake()
    {
        player = this;
    }

    void Update()
    {
        RotateCharacter();
        MoveCharacter();
    }

    void RotateCharacter()
    {
        transform.eulerAngles = Vector3.up * Camera.main.transform.eulerAngles.y;
    }

    void MoveCharacter()
    {
        Vector3 moveTo = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        transform.Translate(moveTo * movementSpeed * Time.deltaTime);
    }
}