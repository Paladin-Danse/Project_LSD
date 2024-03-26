using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Characters/Enemy")]
public class EnemySO : ScriptableObject
{
    [field: SerializeField] public float PlayerChasingRange { get; private set; } = 20f;
    [field: SerializeField] public float AttackRange { get; private set; } = 1.5f;
    [field: SerializeField] public float AttackRate { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField][field: Range(0f, 1f)] public float Dealing_Start_TransitionTime { get; private set; }
    [field: SerializeField][field: Range(0f, 1f)] public float Dealing_End_TransitionTime { get; private set; }

    [field: SerializeField] public EnemyGroundData GroundedData { get; private set; }

}
