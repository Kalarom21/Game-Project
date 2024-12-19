using UnityEngine;

public class WaterPipeController : MonoBehaviour, Interactable
{
    ToolChecker tool;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            tool= player.GetComponentInChildren<ToolChecker>();
        }

        if (tool == null)
        {
            Debug.LogError("ToolChecker could not be found on the Player!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if (tool.wrench)
        {
            Debug.Log("Pipe is fixed");
        }
        else
        {
            Debug.Log("Incorrect tool");
        }
    }
}
