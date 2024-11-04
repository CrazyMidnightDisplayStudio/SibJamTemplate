using Core.Audio;
using UnityEngine;
using Zenject;

public class CharacterController2D : MonoBehaviour
{
    public float rotationSpeed = 350f;

    private Rigidbody2D rb;
    private AudioService _audioService;

    [SerializeField] private float VolumeSFXMove = 0.2f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    [Inject]
    public void Construct(AudioService audioService)
    {
        _audioService = audioService;
    }
    public void Move(Vector2 direction, float moveSpeed, float maxSpeed)
    {
        rb.velocity = direction * moveSpeed * Time.deltaTime;

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed * Time.deltaTime;
        }
        if (rb.velocity != Vector2.zero) 
        {
            _audioService.PlaySfx("robo_ezda_SHORT", VolumeSFXMove);
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