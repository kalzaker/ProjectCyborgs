using UnityEngine;

public class DoorController : MonoBehaviour, IHitable
{
    [SerializeField] Transform openPosition; // Точка, куда переместится дверь при открытии
    [SerializeField] Transform closedPosition; // Точка, куда вернется дверь при закрытии
    [SerializeField] float speed = 10f; // Скорость движения двери
    [SerializeField]private ProximityDetector proximityDetector; // Скрипт для детектирования объектов рядом

    [SerializeField] AudioClip doorClosing;
    [SerializeField] AudioClip doorOpening;

    [SerializeField]AudioSource doorSound;

    private void Start()
    {
        // Найти объект, который содержит ProximityDetector
    }

    private void Update()
    {
        if (proximityDetector.AreObjectsNearby())
        {
            // Открыть дверь (переместить к openPosition)
            MoveDoor(openPosition.position);
        }
        else
        {
            // Закрыть дверь (переместить к closedPosition)
            MoveDoor(closedPosition.position);
        }
    }

    private void MoveDoor(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public void Hit()
    {

    }
}