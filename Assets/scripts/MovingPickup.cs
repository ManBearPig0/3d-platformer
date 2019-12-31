using UnityEngine;

public class MovingPickup : MonoBehaviour
{
    private Vector3 StartingPosition;
    private float LastHeight;

    void Start()
    {
        StartingPosition = transform.position;
    }

    void Update()
    {
        Vector3 NewPosition = StartingPosition + Vector3.up * Mathf.Sin(Time.time) / 3; // Pickups will move in a wave pattern.
        transform.position = NewPosition;
    }
}
