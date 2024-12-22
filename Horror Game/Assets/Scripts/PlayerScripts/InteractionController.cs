using UnityEngine;

interface interactable
{
    public void interact();
}

public class InteractionController : MonoBehaviour
{
    public Transform interactSource;
    public float interactRange;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(interactSource.position, interactSource.forward);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, interactRange))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out interactable interactObj))
                {
                    interactObj.interact();
                }
            }
        }
    }
}
