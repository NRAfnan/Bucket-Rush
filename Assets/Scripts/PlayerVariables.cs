using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int savedScore = 0;
    public int savedHighScore = 0;
    public float playerXPosition = 0f;
}

public class PlayerVariables : MonoBehaviour
{
    // Holds the live tracking structural profile data
    public PlayerData data = new PlayerData();

    // Injects structural changes back into live elements on load
    public void ApplyLoadedData()
    {
        transform.position = new Vector3(data.playerXPosition, transform.position.y, transform.position.z);
    }

    // Captures the updated placement coordinates right before saving
    public void UpdatePositionData()
    {
        data.playerXPosition = transform.position.x;
    }
}