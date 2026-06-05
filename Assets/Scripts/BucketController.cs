using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;

    [Header("Screen Boundaries")]
    public float xBoundary = 2f;

    private float horizontalInput;

    void Update()
    {
        // Don't calculate movement if the game state tells us to freeze
        if (GameManager.Instance == null || !GameManager.Instance.isGameStarted || GameManager.Instance.isGamePaused || GameManager.Instance.isGameOver)
        {
            return;
        }

        // 1. Get Input: Automatically maps to Left/Right arrows and A/D keys
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 2. Calculate Movement: Speed * Input * Time.deltaTime
        float movement = horizontalInput * moveSpeed * Time.deltaTime;

        // 3. Calculate New Position
        float newXPosition = transform.position.x + movement;

        // 4. Clamp Position: Prevents the bucket from going off-screen
        newXPosition = Mathf.Clamp(newXPosition, -xBoundary, xBoundary);

        // 5. Apply Position back to the GameObject
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
    }
}
