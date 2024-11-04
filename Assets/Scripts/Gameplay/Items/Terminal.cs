using System;
using System.Collections.Generic;
using Assets.Scripts.Gameplay.ActionMenu;
using Core.Audio;
using Core.Services.EventBus;
using Cysharp.Threading.Tasks;
using Game.Services.Debugging.Gameplay.Services;
using Game.Services.Events;
using UnityEngine;
using Zenject;

namespace Gameplay.Items
{
    public class Terminal : MonoBehaviour, IInteractAction
    {
        private Dictionary<string, Action> actions = new Dictionary<string, Action>();
        private int _state = 0;
        private AudioService _audioService;
        private DoorsService _doorsService;
        private CapsuleCollider2D _keyTriggerZone;

        private bool _isPreparedCardAction = false;

        public bool IsPreparedCardAction
        {
            get => _isPreparedCardAction;
            set
            {
                _isPreparedCardAction = value;
                _keyTriggerZone.enabled = value;
            }
        }

        [Inject]
        public void Construct(AudioService audioService, DoorsService doorsService)
        {
            _audioService = audioService;
            _doorsService = doorsService;
        }
        
        private void OnEnable()
        {
            CMDEventBus.Subscribe<CurrentEvent>(Brain);
        }

        private void OnDisable()
        {
            CMDEventBus.Unsubscribe<CurrentEvent>(Brain);
        }

        private void Start()
        {
            _doorsService.LockDoors(new []{1, 2, 3, 4});
            actions.Add("terminal", InteractWithTerminal);
            
            //add disabled KeyTriggerZone
            _keyTriggerZone = gameObject.AddComponent<CapsuleCollider2D>();
            _keyTriggerZone.enabled = false;
            _keyTriggerZone.isTrigger = true;
            _keyTriggerZone.size = Vector2.one;
        }
        
        public Dictionary<string, Action> GetActions()
        {
            return actions;
        }

        private void Brain(CurrentEvent currentEvent)
        {
            var eventName = currentEvent.EventName;

            if (_state == 0 && eventName == "PickupKeyCard1")
            {
                _state = 1;
                Debug.Log($"Terminal state: {_state}");
            }

            if (_state == 1 && eventName == "PassTheKeyCard1")
            {
                _state = 2;
                Debug.Log($"Terminal state: {_state}");
            }
            
            if (_state == 2 && eventName != "InteractWithTerminal")
            {
                IsPreparedCardAction = false;
                Debug.Log($"Terminal state: {_state}");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Human"))
            {
                if (other.GetComponent<HumanController>().HaveKeyCardNumber == 1)
                {
                    // TODO : Open Doors
                }
            }
        }

        private async UniTask UnlockDoorsWithDelay(float delay)
        {
            var audioSource = GetComponent<AudioSource>();
            
            // text
            _audioService.PlaySfx("Pip", default, audioSource);
            await UniTask.WaitForSeconds(1);
            _audioService.PlaySfx("Pip", default, audioSource);
            // text
            await UniTask.WaitForSeconds(1);
            // text
            _audioService.PlaySfx("Pip", default, audioSource);
            await UniTask.WaitForSeconds(1);
            
            // open doors
            _doorsService.UnlockDoors(new []{1, 2, 3, 4});
            Destroy(this);
        }

        private void InteractWithTerminal()
        {
            switch (_state)
            {
                case 0:
                    // popup "need a key card!"
                    Debug.Log("need a kay card");
                    break;
                case 1:
                    // "I don't have the hands to use the key card!"
                    Debug.Log("I don't have the hands to use the kay card");
                    break;
                case 2:
                    _audioService.PlaySfx("I'm going", 1f);
                    IsPreparedCardAction = true;
                    break;
            }
        }
    }
}