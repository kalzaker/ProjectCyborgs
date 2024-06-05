using UnityEngine;

public class DoorController : MonoBehaviour, IHitable
{
    [SerializeField] Transform openPosition; // �����, ���� ������������ ����� ��� ��������
    [SerializeField] Transform closedPosition; // �����, ���� �������� ����� ��� ��������
    [SerializeField] float speed = 10f; // �������� �������� �����
    [SerializeField]private ProximityDetector proximityDetector; // ������ ��� �������������� �������� �����

    [SerializeField] AudioClip doorClosing;
    [SerializeField] AudioClip doorOpening;

    [SerializeField]AudioSource doorSound;

    private void Start()
    {
        // ����� ������, ������� �������� ProximityDetector
    }

    private void Update()
    {
        if (proximityDetector.AreObjectsNearby())
        {
            // ������� ����� (����������� � openPosition)
            MoveDoor(openPosition.position);
        }
        else
        {
            // ������� ����� (����������� � closedPosition)
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