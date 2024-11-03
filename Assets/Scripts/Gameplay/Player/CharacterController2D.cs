using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public float rotationSpeed = 350f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 direction, float moveSpeed, float maxSpeed)
    {
        rb.velocity = direction * moveSpeed * Time.deltaTime;

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed * Time.deltaTime;
        }
    }

    public void Rotate(float turnInput)
    {
        if (turnInput != 0)
        {
            float turnAmount = turnInput * rotationSpeed * Time.deltaTime;
            rb.MoveRotation(rb.rotation - turnAmount);
        }
    }
}