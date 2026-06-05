using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// CRITICAL: This namespace is required to access Light2D properties
using UnityEngine.Rendering.Universal;


public class DiamondSpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    public GameObject diamondPrefab;
    public float spawnInterval = 1.5f;
    public float xSpawnBoundary = 2f;
    public float ySpawnHeight = 6.0f;

    void Start()
    {
        InvokeRepeating("SpawnDiamond", 0.5f, spawnInterval);
    }

    void SpawnDiamond()
    {
        // Don't spawn diamonds if the game hasn't started, is paused, or is over
        if (GameManager.Instance != null && (!GameManager.Instance.isGameStarted || GameManager.Instance.isGamePaused || GameManager.Instance.isGameOver))
        {
            return;
        }

        float randomX = Random.Range(-xSpawnBoundary, xSpawnBoundary);
        Vector3 spawnPosition = new Vector3(randomX, ySpawnHeight, 0f);

        GameObject newDiamond = Instantiate(diamondPrefab, spawnPosition, Quaternion.identity);

        float randomHue = Random.Range(0f, 1f);
        float fixedSat = 90f / 100f;
        float fixedVal = 30f / 100f;
        float fixedAlpha = 60f / 255f;

        Color randomColor = Color.HSVToRGB(randomHue, fixedSat, fixedVal);
        randomColor.a = fixedAlpha;

        Light2D childLight = newDiamond.GetComponentInChildren<Light2D>();

        if (childLight != null)
        {
            childLight.color = randomColor;
        }
        else
        {
            Debug.LogWarning("Spawned a diamond, but couldn't find a Light2D component attached to it or its children!");
        }
    }
}