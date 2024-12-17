using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBobbing : MonoBehaviour
{
    public float walkIntensity = 0.02f;
    public float runIntensity = 0.06f;
    public float idleIntensity = 0.005f;
    public float effectIntensityX;
    public float effectSpeed = 4;
    public float runSpeed = 7;
    public float idleSpeed = 0.4f;
    public float smoothness = 10f; // Control how smooth the bobbing transition is

    private Vector3 originalLocalPosition;
    private Vector3 targetLocalPosition;
    private float sinTime;

    public FPSController player;

    void Start()
    {
        // Store the initial local position of the tool
        originalLocalPosition = transform.localPosition;
        targetLocalPosition = originalLocalPosition;
    }

    void Update()
    {
        Vector3 inputVector = new Vector3(Input.GetAxis("Vertical"), 0f, Input.GetAxis("Horizontal"));
        float currIntensity;
        if (inputVector.magnitude > 0f)
        {
            float curSpeed = player.isRunning ? runSpeed : effectSpeed;
            currIntensity = player.isRunning ? runIntensity : walkIntensity;
            sinTime += Time.deltaTime * curSpeed;
        }
        else
        {
            currIntensity = idleIntensity;
            sinTime += Time.deltaTime * idleSpeed;
        }

        // Calculate target bobbing offsets
        float sinAmountY = -Mathf.Abs(currIntensity * Mathf.Sin(sinTime));
        float sinAmountX = currIntensity * Mathf.Cos(sinTime) * effectIntensityX;

        targetLocalPosition = originalLocalPosition + new Vector3(sinAmountX, sinAmountY, 0f);

        // Smoothly interpolate towards the target position
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPosition, Time.deltaTime * smoothness);
    }
}
