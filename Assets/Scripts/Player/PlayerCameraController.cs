using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    const float Y_ANGLE_MIN = -90f;
    const float Y_ANGLE_MAX = 90f;

    const float OFFSET_MIN = 0f;
    const float OFFSET_MAX = 40f;

    const float e = 0.0001f; //Using e as in epsilon to mark a small positive number

    [SerializeField]
    private Transform pivotPoint = default;

    [SerializeField,
        Tooltip("A point on where the camera uses as a focus point, this object will be in the middle of the screen")]
    private Transform playerFocusPoint = default;


    [Header("Camera attributes")]

    [SerializeField,
        Tooltip("Choose what layers should not be breaking the line of sight")]
    LayerMask obstructionMask = -1;

    [SerializeField, Range(OFFSET_MIN, OFFSET_MAX),
        Tooltip("The minimum distance you can have between the camera and the player")]
    float minOffsetDistance = 0f; //Min distance between player and camera

    [SerializeField, Range(OFFSET_MIN, OFFSET_MAX),
        Tooltip("The distance between the player and the camera")]
    private float offsetDistance = 20f; //Distance between player and camera

    [SerializeField, Range(OFFSET_MIN, OFFSET_MAX),
        Tooltip("How fast the camera should snap to target")]
    private float responsivness = 8;


    [Header("Camera Controls")]

    [SerializeField,
        Tooltip("If you want to invert the horizontal controls")]
    bool invertHorizontalCtrls = false;

    [SerializeField,
        Tooltip("If you want to invert the horizontal controls")]
    bool invertVerticalCtrls = false;

    [Header("Looking")]
    [SerializeField, Range(1f, 360f), 
        Tooltip("Mouse sensitivity for X-axis")]
    float XMouseSensitivity = 90f;

    [SerializeField, Range(1f, 360f), 
        Tooltip("Mouse sensitivity for Y-axis")]
    float YMouseSensitivity = 90f;

    [SerializeField, Range(Y_ANGLE_MIN, Y_ANGLE_MAX), 
        Tooltip("Capping the max and min vertical angle of the player controller")]
    float minVerticalAngle = -30f, maxVerticalAngle = 60f;

    PlayerCameraController mainPlayerCameraController;
    public PlayerCameraController MainPlayerCameraController => mainPlayerCameraController;

    Camera MainCamera;

    private Vector3 focusPoint; //What we want to focus on, in this case the players position

    private Vector3 lookPosition;

    private Vector2 orbitAngles = Vector2.zero; //We use Vector2D for there is no need for the Z axis

    private Quaternion lookRotation;

    private Vector2 input;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        MainCamera = GetComponent<Camera>();
        focusPoint = playerFocusPoint.position;
        transform.localRotation = Quaternion.Euler(playerFocusPoint.eulerAngles);

        mainPlayerCameraController = this;
    }

    private void Start()
    {
        SetCameraPositionBehindPlayer();
    }

    private void OnValidate()
    {
        if (maxVerticalAngle < minVerticalAngle) //Basically clamping the the max and min values of the vertical angle
        {
            maxVerticalAngle = minVerticalAngle;
        }

        if (offsetDistance < minOffsetDistance)
        {
            offsetDistance = minOffsetDistance;
        }
    }

    void LateUpdate()
    {
        UpdateFocusPoint();

        ReadMouseInputForCameraRotation(); //We only need to constraint the angles if they been changed, so we check for that

        ConstraintAngles();

        lookRotation = Quaternion.Euler(orbitAngles); //WE only need to recalculate the angles if they been changed, otherwise (else stat.) we retrieve the existing one

        //Set the position and rotation for the camera
        Vector3 lookDirection = lookRotation * Vector3.forward;

        lookPosition = focusPoint - lookDirection * offsetDistance;

        Vector3 rectOffset = lookDirection * MainCamera.nearClipPlane;
        Vector3 rectPosition = lookPosition + rectOffset;
        Vector3 castFrom = pivotPoint.position;
        Vector3 castLine = rectPosition - castFrom;
        float castDistance = castLine.magnitude;
        Vector3 castDirection = castLine / castDistance;

        if (Physics.BoxCast(castFrom, CameraHalfExtends, castDirection, out RaycastHit hit, lookRotation, castDistance - MainCamera.nearClipPlane, obstructionMask))
        {
            rectPosition = castFrom + castDirection * hit.distance;
            lookPosition = rectPosition - rectOffset;
        }

        transform.SetPositionAndRotation(lookPosition, lookRotation);
    }

    void UpdateFocusPoint()
    {
        Vector3 targetPoint = pivotPoint.position;
        
        focusPoint = Vector3.Lerp(focusPoint, targetPoint, Time.deltaTime * responsivness);
    }

    void ReadMouseInputForCameraRotation()
    {
        input.x = invertVerticalCtrls ? -Input.GetAxis("Mouse Y") * XMouseSensitivity : Input.GetAxis("Mouse Y") * XMouseSensitivity;
        input.y = invertHorizontalCtrls ? -Input.GetAxis("Mouse X") * YMouseSensitivity : Input.GetAxis("Mouse X") * YMouseSensitivity;

        if (input.x < -e || input.x > e || input.y < -e || input.y > e) //here we check for movement from the mouse
        {
            orbitAngles += Time.unscaledDeltaTime * input; //Time.unscaledDeltaTime is independent from the in-game time, so if the timescale is tempered the orbitAngles are unaffected 
        }
    }

    void ConstraintAngles()
    {
        orbitAngles.x = Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);

        if (orbitAngles.y < 0f) //Making sure the vertical angles stays within the 0-360 range and not exceeding that
        {
            orbitAngles.y += 360f;
        }
        else if (orbitAngles.y > 360f)
        {
            orbitAngles.y -= 360f;
        }
    }

    Vector3 CameraHalfExtends
    {
        get
        {
            Vector3 halfExtends;
            halfExtends.y = MainCamera.nearClipPlane * Mathf.Tan(0.5f * Mathf.Deg2Rad * MainCamera.fieldOfView);
            halfExtends.x = halfExtends.y * MainCamera.aspect;
            halfExtends.z = 0f;
            return halfExtends;
        }
    }

    public void SetCameraPositionBehindPlayer()
    {
        orbitAngles = new Vector2(15f, playerFocusPoint.eulerAngles.y);
    }
}