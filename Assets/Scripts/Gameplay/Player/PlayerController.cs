using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController2D characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController2D>();
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        float moveInput = -Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        Vector2 moveDirection = transform.up * moveInput;

        characterController.Move(moveDirection);
        characterController.Rotate(turnInput);
    }
}
