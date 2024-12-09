using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

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

    float crouchHeight = 1f;
    float standingHeight;
    float currentHeight;

    float crouchTransitionSpeed = 10f;

    Vector3 initialCameraPosition;
    public Transform cameraTransform;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;
    public bool isRunning;
    public bool isCrouching;


    public float height
    {
        get => characterController.height;
        set => characterController.height = value;
    }
    #endregion
    CharacterController characterController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        standingHeight = height;
        initialCameraPosition = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        handleMovement();
        handleRotation();
        handleCrouching();
    }

    void handleMovement()
    {
        #region Handles Movement
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        isRunning = Input.GetKey(KeyCode.LeftShift);

        // Determine movement speed based on crouching or running
        float curSpeedX = canMove ? (isRunning && !isCrouching ? runSpeed : (isCrouching ? crouchSpeed : walkSpeed)) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning && !isCrouching ? runSpeed : (isCrouching ? crouchSpeed : walkSpeed)) * Input.GetAxis("Horizontal") : 0;

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
    }

    void handleCrouching()
    {
        // Target height based on crouch state
        var targetHeight = isCrouching ? crouchHeight : standingHeight;

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching; // Toggle crouch state
        }

        /*
        if (isCrouching && Input.GetKeyDown(KeyCode.LeftControl))
        {
            var castOrigin = transform.position + new Vector3(0, currentHeight / 2, 0);
            if (Physics.Raycast(castOrigin, Vector3.up, out RaycastHit hit, 0.2f))
            {
                var distToCeiling = hit.point.y - castOrigin.y;
                targetHeight = Mathf.Max
                (
                    currentHeight + distToCeiling - 0.1f,
                    crouchHeight
                );
            }
        }
        */

        // Smoothly transition the height
        currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * crouchTransitionSpeed);

        // Update character controller's height
        height = currentHeight;

        // Adjust camera position based on crouch state
        var halfHeightDiff = new Vector3(0, (standingHeight - currentHeight) / 2, 0);

        if (isCrouching)
        {
            halfHeightDiff.y -= 0.3f;
        }

        Vector3 targetCamPos = initialCameraPosition - halfHeightDiff;
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, targetCamPos, Time.deltaTime * crouchTransitionSpeed);
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
