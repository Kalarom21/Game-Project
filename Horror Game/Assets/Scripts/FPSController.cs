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

    float crouchHeight;
    float standingHeight;

    Vector3 initialCameraPosition;
    public Transform cameraTransform;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;
    public bool isRunning;
    public bool isCrouching;
    #endregion
    CharacterController characterController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        standingHeight = characterController.height;
        crouchHeight = standingHeight / 2;
        initialCameraPosition = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        handleMovement();
        handleRotation();
    }

    void handleMovement()
    {
        #region Handles Movement
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        isRunning = Input.GetKey(KeyCode.LeftShift);

        // Determine movement speed based on crouching or running
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

        #region Handles Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            // Toggle crouch state
            isCrouching = !isCrouching;

            // Set target values based on crouch state
            var heightTarget = isCrouching ? crouchHeight : standingHeight;

            var halfHeightDiff = new Vector3(0, (standingHeight - heightTarget) / 2, 0);
            var newCameraPos = initialCameraPosition - halfHeightDiff;

            playerCamera.transform.localPosition = newCameraPos;
            characterController.height = heightTarget;
        }

        
        #endregion

    }

    void handleRotation()
    {
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
        }
    }

}
