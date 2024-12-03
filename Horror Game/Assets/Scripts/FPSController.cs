using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] 
public class FPSController : MonoBehaviour
{
    #region Movement Variables
    public Camera playerCamera;
    public float crouchSpeed = 1.5f;
    public float walkSpeed = 3f;
    public float runSpeed = 5f;
    public float jumpPower = 7f;
    public float gravity = 10f;

    public float lookSpeed = 2f;
    public float lookLimit = 90f;

    public float crouchHeight = 0.5f; 
    public float standHeight = 2f;  
    public Vector3 crouchCenter = new Vector3(0, 0.5f, 0); 
    public Vector3 standCenter = new Vector3(0, 0, 0);

    private float originalCameraHeight;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;
    #endregion
    CharacterController characterController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originalCameraHeight = characterController.transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        #region Handles Movement
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isCrouching = Input.GetKey(KeyCode.LeftControl);
        float curSpeedX = canMove ? (isRunning ? runSpeed : (isCrouching ? crouchSpeed : walkSpeed)) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : (isCrouching ? crouchSpeed : walkSpeed)) * Input.GetAxis("Horizontal") : 0;

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        #endregion

        #region Handles Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        #endregion

        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            // Handle Vertical Rotation (Camera)
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

            rotationX -= mouseY; // Look up/down, camera rotation
            rotationX = Mathf.Clamp(rotationX, -lookLimit, lookLimit); // Prevent over-rotation

            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0); // Apply to camera

            // Handle Horizontal Rotation (Player Body)
            transform.rotation *= Quaternion.Euler(0, mouseX, 0); // Rotate player body horizontally

            if (isCrouching)
            {
                playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, crouchHeight, playerCamera.transform.localPosition.z);
                characterController.height = crouchHeight;
                characterController.center = crouchCenter;
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl) &&  isCrouching)
            {
                isCrouching = false;
                playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, originalCameraHeight, playerCamera.transform.localPosition.z);
                characterController.height = standHeight;
                characterController.center = standCenter;
            }
        }
        #endregion
    }

}
