﻿using System;
using System.Collections.Generic;
using Assets.Scripts.Gameplay.ActionMenu;
using Assets.Scripts.Gameplay.Popup;
using Assets.Scripts.Gameplay.Services;
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
        [SerializeField] private CapsuleCollider2D _keyTriggerZone;
        [SerializeField] private Patrol _patrol;
        
        private Dictionary<string, Action> actions = new Dictionary<string, Action>();
        private int _state = 0;
        
        private AudioService _audioService;
        private DoorsService _doorsService;
        private PopupService _popupService;

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
        public void Construct(AudioService audioService, DoorsService doorsService, PopupService popupService)
        {
            _audioService = audioService;
            _doorsService = doorsService;
            _popupService = popupService;
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
            _keyTriggerZone.enabled = false;
            _keyTriggerZone.isTrigger = true;
            _keyTriggerZone.size = Vector2.one * 1.8f;
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
                if (eventName != "InteractWithTerminal")
                {
                    IsPreparedCardAction = false;
                }

                if (eventName == "HumanToTheTerminal")
                {
                    IsPreparedCardAction = true;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Human"))
            {
                Debug.Log($"Collidion with human");
                if (other.gameObject.TryGetComponent<HumanController>(out var humanController))
                {
                    Debug.Log($"Unlock doors");
                    humanController.HaveKeyCardNumber = 0;
                    UnlockDoorsWithDelay().Forget();
                }
                else
                {
                    Debug.LogError($"humanController is null");
                }
            }
        }

        private async UniTask UnlockDoorsWithDelay()
        {
            var audioSource = GetComponent<AudioSource>();
            PopupNoUI popup;
            
            // text
            _audioService.PlaySfx("Pip", default, audioSource);
            popup = _popupService.GetPopup("before the doors open...3", transform.position);
            await UniTask.WaitForSeconds(1);
            
            _audioService.PlaySfx("Pip", default, audioSource);
            popup.SetText("before the doors open...2");
            await UniTask.WaitForSeconds(1);
            
            _audioService.PlaySfx("Pip", default, audioSource);
            popup.SetText("before the doors open...1");
            await UniTask.WaitForSeconds(1);
            
            // open doors
            _doorsService.UnlockDoors(new []{1, 2, 3, 4});
            _doorsService.OpenDoors(new []{1, 2, 3, 4});
            _doorsService.StayOpenDoors(new []{1, 2, 3, 4});
            _patrol.addAllPoints();
            _popupService.DisablePopup(popup);
            Destroy(this);
        }

        private void InteractWithTerminal()
        {
            switch (_state)
            {
                case 0:
                    // popup "need a key card!"
                    _popupService.GetPopup("Need a key card", transform.position, 3f);
                    break;
                case 1:
                    // "I don't have the hands to use the key card!"
                    _popupService.GetPopup("I don't have the hands to use the kay card", transform.position, 3f);
                    break;
                case 2:
                    //_audioService.PlaySfx("I'm going", 1f);
                    CMDEventBus.Publish(new CurrentEvent("HumanToTheTerminal"));
                    break;
            }
        }
    }
}