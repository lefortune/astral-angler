using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePointer : MonoBehaviour
{
    private float lerpTime = 0.2f; // Duration of the animation
    private float currentLerpTime = 0f; // Tracks the time elapsed for lerping
    private Vector3 initialScale; // The original scale of the prefab
    private Color initialColor; // The original color of the sprite
    private SpriteRenderer spriteRenderer; // The SpriteRenderer component of the marker

    // This will be called when the prefab is instantiated
    void Start()
    {
        // Initialize scale and color values
        initialScale = transform.localScale; // Get the initial scale of the prefab
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        initialColor = spriteRenderer.color; // Get the initial color of the sprite

        transform.localScale = initialScale * 0.5f;
        spriteRenderer.color = Color.black; // Start with black color
    }

    void Update()
    {
        if (currentLerpTime < lerpTime)
        {
            // Increment the lerp time based on deltaTime to smoothly transition over the duration
            currentLerpTime += Time.deltaTime;

            // Calculate lerp progress (a value between 0 and 1)
            float lerpProgress = currentLerpTime / lerpTime;

            // Lerp the scale (from small to large) over time
            transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, lerpProgress);

            // Lerp the color (from black to normal color) over time
            spriteRenderer.color = Color.Lerp(Color.black, initialColor, lerpProgress);
        }
    }

}
