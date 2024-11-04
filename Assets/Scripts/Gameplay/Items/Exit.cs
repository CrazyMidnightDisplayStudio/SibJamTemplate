using System;
using System.Collections.Generic;
using Assets.Scripts.Gameplay.ActionMenu;
using Core.Services.EventBus;
using Core.Services.SceneManagement;
using Game.Services.Events;
using UnityEngine;
using Zenject;

namespace Gameplay.Items
{
    public class Exit : MonoBehaviour, IInteractAction
    {
        [SerializeField] private CapsuleCollider2D _triggerCollider;
        
        private Dictionary<string, Action> actions = new Dictionary<string, Action>();
        
        private int _keyNumber;
        private bool _isExitPrepared;
        
        private SceneTransitionService _sceneTransitionService;

        [Inject]
        public void Construct(SceneTransitionService sceneTransitionService)
        {
            _sceneTransitionService = sceneTransitionService;
        }
        
        private void Start()
        {
            _triggerCollider.isTrigger = true;
            _triggerCollider.size = Vector2.one * 1.2f;
            _triggerCollider.enabled = false;
            actions.Add("Rescue capsule", EndGame);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Human"))
            {
                EndGame();
            }
        }
        
        public void AddExitWay()
        {
            Debug.Log("Add Exit Way");
            _isExitPrepared = true;
            _triggerCollider.enabled = true;
        }
        
        private void EndGame()
        {
            Debug.Log("end game");
            if (_keyNumber == 2)
            {
                _sceneTransitionService.LoadScene("FinalCutscene");
            }
        }

        public Dictionary<string, Action> GetActions() => actions;
    }
}