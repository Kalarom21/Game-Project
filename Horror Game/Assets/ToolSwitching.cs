using UnityEngine;

public class ToolSwitching : MonoBehaviour {

    public int selectedTool = 0;

    void Start() {
        SelectTool();
    }

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

    void SelectTool() {
        int i = 0;
        foreach(Transform tool in transform) {
            if(i == selectedTool) {
                tool.gameObject.SetActive(true);
            }
            else {
                tool.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
