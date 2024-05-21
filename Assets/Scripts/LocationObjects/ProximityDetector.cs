using UnityEngine;
using System.Collections.Generic;

public class ProximityDetector : MonoBehaviour
{
    public List<Transform> nearbyObjects = new List<Transform>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            if (!nearbyObjects.Contains(other.transform))
            {
                nearbyObjects.Add(other.transform);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            if (nearbyObjects.Contains(other.transform))
            {
                nearbyObjects.Remove(other.transform);
            }
        }
    }

    public bool AreObjectsNearby()
    {
        return nearbyObjects.Count > 0;
    }
}