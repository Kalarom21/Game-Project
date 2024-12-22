using UnityEngine;

public class ColorController : MonoBehaviour, interactable
{
    public void interact()
    {
        if (gameObject.GetComponent<Renderer>().material.color == Color.red)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
        else gameObject.GetComponent<Renderer>().material.color = Color.red;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
