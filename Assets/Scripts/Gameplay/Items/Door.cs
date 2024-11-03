using UnityEngine;

namespace Gameplay.Items
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private int _doorID;
        private Collider2D doorCollider;
        private Animator animator;
        
        public bool IsOpen { get; private set; }
        public bool IsLocked { get; private set; }
        public int DoorID => _doorID;
    
        private void Awake()
        {
            doorCollider = transform.parent.GetComponent<Collider2D>();
            animator = transform.parent.GetComponent<Animator>();
        }
        
        public void Lock() => IsLocked = true;
        public void Unlock() => IsLocked = false;

        public void OpenDoor()
        {
            if (IsLocked) return;
            animator.SetTrigger("Open");
            doorCollider.enabled = false;
            IsOpen = true;
        }

        public void CloseDoor()
        {
            animator.SetTrigger("Close");
            doorCollider.enabled = true;
            IsOpen = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") || other.CompareTag("Human"))
            {
                OpenDoor();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player") || other.CompareTag("Human"))
            {
                CloseDoor();
            }
        }
    }
}