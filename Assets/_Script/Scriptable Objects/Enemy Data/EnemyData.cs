using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy/Enemy Data")]

public class EnemyData : ScriptableObject
{

    [Header("Movement")]
    public float moveSpeed;
    public bool isFlying;
    public bool isClimber;
    public float retreatDistance;

    [Header("Attacks")]
    public float attackRange;
    public float damage;

}
