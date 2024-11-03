using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private CharacterController2D characterController;
    [SerializeField] float speed = 2f;
    [SerializeField] float maxSpeed = 5f;

    private Vector2 moveDirection;
    private float turnInput;

    void Start()
    {
        characterController = GetComponent<CharacterController2D>();
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
