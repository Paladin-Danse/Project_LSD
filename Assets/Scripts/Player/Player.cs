using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerInput input { get; private set; }
    [field: SerializeField] public PlayerData Data { get; private set; }
    public Rigidbody rigidbody { get; private set; }

<<<<<<< Updated upstream
    public DungeonInteract dungeonInteract;
=======
    DungeonInteract dungeonInteract;
>>>>>>> Stashed changes

    private void Awake()
    {
        stateMachine = new PlayerStateMachine(this);
        rigidbody = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
        dungeonInteract = GetComponent<DungeonInteract>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }    
}
