using UnityEngine;

public class Door : MonoBehaviour
{
    private Collider2D doorCollider;
    private Animator animator;

    void Start()
    {
        doorCollider = transform.parent.GetComponent<Collider2D>();
        animator = transform.parent.GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Human"))
        {
            doorCollider.enabled = false;
            animator.SetTrigger("Open");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Human"))
        {
            doorCollider.enabled = true;
            animator.SetTrigger("Close");
        }
    }
}