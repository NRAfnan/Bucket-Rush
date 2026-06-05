using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    [Header("Falling Settings")]
    public float fallSpeed = 4f;
    public float bottomBoundary = -6f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.down * fallSpeed; // Use rb.velocity on older Unity versions
    }

    void FixedUpdate()
    {
        // If missed, report directly to the GameManager singleton
        if (transform.position.y < bottomBoundary)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TriggerGameOver();
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If caught by the bucket, tell the GameManager to increment score fields
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(1);
            }
            Destroy(gameObject);
        }
    }
}
