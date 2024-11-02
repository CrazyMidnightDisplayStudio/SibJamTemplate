using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 700f;
    public float maxSpeed = 10f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 direction)
    {
        rb.velocity = direction * moveSpeed;

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
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