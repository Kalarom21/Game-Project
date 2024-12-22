using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ToolSwitching : MonoBehaviour {
    
    public GameObject rightHand;
    private Transform newTarget;
    private TwoBoneIKConstraint twoBoneIK;
    public int selectedTool = 0;

    void Start() {

        SelectTool();
    }

    // Changes selected tool when mouse scrolled and calls SelectTool.
    void Update() {

        int previousSelectedTool = selectedTool;

        if(Input.GetAxis("Mouse ScrollWheel") > 0f) {
            if(selectedTool >= transform.childCount - 1) {
                selectedTool = 0;
            }
            else {
                selectedTool++;
            }
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0f) {
            if(selectedTool <= 0) {
                selectedTool = transform.childCount - 1;
            }
            else {
                selectedTool--;
            }
        }

        if(previousSelectedTool != selectedTool) {
            SelectTool();
        }
    }

    // Activates the currently selected tool and deactivates all others.
    // Changes RightHand target to the grip of the selected tool.
    void SelectTool() {

        int i = 0;
        foreach(Transform tool in transform) {
            if(i == selectedTool) {
                twoBoneIK = rightHand.GetComponent<TwoBoneIKConstraint>();
                newTarget = tool.gameObject.transform.GetChild(0);
                twoBoneIK.data.target = newTarget;
                tool.gameObject.SetActive(true);

                // Force RigBuilder to update rigs
                RigBuilder rigBuilder = rightHand.GetComponentInParent<RigBuilder>();
                rigBuilder.Build();
            }
            else {
                tool.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
