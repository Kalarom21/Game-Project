using UnityEngine;

interface Interactable
{
    void Interact();
}

public class InteractionController : MonoBehaviour
{
    public Transform interactSource;
    public float interactRange;
    public float holdDuration = 1.0f; // Time required to hold the key

    private float holdTimer = 0f; // Tracks how long the key has been held
    private bool isHolding = false; // Tracks if the key is being held

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(new Ray(interactSource.position, interactSource.forward), out RaycastHit hitInfo, interactRange))
        {
            GameObject targetObject = hitInfo.collider.gameObject;

            if (targetObject.CompareTag("Hold"))
            {
                HandleHoldInteraction(targetObject);
            }
            else if (targetObject.CompareTag("Click"))
            {
                HandleClickInteraction(targetObject);
            }
        }
    }

    void HandleHoldInteraction(GameObject targetObject)
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (!isHolding)
            {
                isHolding = true; // Start holding
                holdTimer = 0f; // Reset the timer
            }

            holdTimer += Time.deltaTime; // Increment timer

            if (holdTimer >= holdDuration)
            {
                TryInteract(targetObject); // Trigger interaction
                holdTimer = 0f; // Reset timer to avoid repeated triggers
            }
        }
        else
        {
            isHolding = false; // Reset holding state
            holdTimer = 0f; // Reset timer
        }
    }

    void HandleClickInteraction(GameObject targetObject)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract(targetObject); // Trigger interaction on key press
        }
    }

    void TryInteract(GameObject targetObject)
    {
        if (targetObject.TryGetComponent(out Interactable interactObj))
        {
            interactObj.Interact();
        }
    }
}
