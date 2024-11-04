using Core.Services.EventBus;
using Game.Services.Events;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController2D characterController;
    [SerializeField] float speed = 2f;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] private GameObject _keyCard1;

    private Vector2 moveDirection;
    private float turnInput;
    
    private CapsuleCollider2D _passKeyCollider;

    void Start()
    {
        characterController = GetComponent<CharacterController2D>();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
    }

    private void PreparePassKey()
    {
        _passKeyCollider = gameObject.AddComponent<CapsuleCollider2D>();
        _passKeyCollider.isTrigger = true;
        _passKeyCollider.size = Vector2.one * 0.5f;
    }

    private void PassTheKeyCard1()
    {
        CMDEventBus.Publish(new CurrentEvent("PassTheKeyCard1"));
        Destroy(_keyCard1);
        Destroy(_passKeyCollider);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Human")
        {
            PassTheKeyCard1();
        }
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        characterController.Move(moveDirection, speed, maxSpeed);
        characterController.Rotate(turnInput);

    }

    private void HandleInput()
    {
        float moveInput = -Input.GetAxis("Vertical");

        turnInput = Input.GetAxis("Horizontal");
        moveDirection = transform.up * moveInput;
    }
}
