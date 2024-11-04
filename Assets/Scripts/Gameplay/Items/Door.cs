using Core.Audio;
using UnityEngine;
using Zenject;

namespace Gameplay.Items
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private int _doorID;
        private Collider2D doorCollider;
        private Animator animator;

        private SpriteRenderer lockedSign;

        private AudioService _audioService;
        public bool IsOpen { get; private set; }
        
        public bool IsStayOpen { get; set; }
        public bool IsLocked { get; private set; }
        public int DoorID => _doorID;

        private void Awake()
        {
            doorCollider = transform.parent.GetComponent<Collider2D>();
            animator = transform.parent.GetComponent<Animator>();
            lockedSign = GetComponentInChildren<SpriteRenderer>();
        }
        [Inject]
        public void Construct(AudioService audioService)
        {
            _audioService = audioService;
        }
        public void Lock() => IsLocked = true;
        public void Unlock() => IsLocked = false;

        public void OpenDoor()
        {
            if (IsLocked)
            {
                lockedSign.enabled = true;
                return;
            }
            _audioService.PlaySfx("Door_Open");
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
            if (IsStayOpen) return;
            if (other.CompareTag("Player") || other.CompareTag("Human"))
            {
                OpenDoor();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (IsStayOpen) return;
            if (other.CompareTag("Player") || other.CompareTag("Human"))
            {
                lockedSign.enabled = false;
                CloseDoor();
            }
        }
    }
}