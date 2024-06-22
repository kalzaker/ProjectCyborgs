using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerParameters : MonoBehaviour
{
    [SerializeField]
    public Transform enemySpawnerPosition;

    [SerializeField]
    public Transform pathParentObject;

    [SerializeField]
    public int enemiesCount;
}
