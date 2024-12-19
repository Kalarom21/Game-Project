using UnityEngine;

public class ToolChecker : MonoBehaviour
{
    public Transform toolHolder; // Reference to the parent object holding the tools
    private GameObject currentTool;
    public bool wrench;
    public bool screwdriver;

    void Update()
    {
        DetectActiveTool();
    }

    private void DetectActiveTool()
    {
        if (toolHolder == null)
        {
            return;
        }

        // Iterate through each child of the toolHolder
        foreach (Transform tool in toolHolder)
        {
            if (tool.gameObject.activeSelf) // Check if the tool is enabled
            {
                if (currentTool != tool.gameObject)
                {
                    currentTool = tool.gameObject;
                    
                    if (currentTool.name == "AdjustableWrench")
                    {
                        wrench = true;
                        screwdriver = false;
                    }
                    if (currentTool.name == "Screwdriver")
                    {
                        wrench = false;
                        screwdriver = true;
                    }
                }
               
            }
        }

        // If no tool is active, clear the current tool
        if (currentTool != null)
        {
            
            currentTool = null;
        }
    }
}
