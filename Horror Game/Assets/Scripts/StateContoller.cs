using UnityEditor.Build;
using UnityEngine;

public class StateContoller : MonoBehaviour
{
    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool("isWalking");
        bool wPressed = Input.GetKey("w");
        if (!isWalking && wPressed)
        {
            animator.SetBool("isWalking", true);
        }
        if (isWalking && !wPressed)
        {
            animator.SetBool("isWalking", false);
        }
    }
}
