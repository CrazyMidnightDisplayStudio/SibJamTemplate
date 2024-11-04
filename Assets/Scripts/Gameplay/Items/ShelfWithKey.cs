using System;
using System.Collections.Generic;
using Assets.Scripts.Gameplay.ActionMenu;
using Core.Services.EventBus;
using Game.Services.Events;
using UnityEngine;

namespace Gameplay.Items
{
    public class ShelfWithKey : MonoBehaviour, IInteractAction
    {
        [SerializeField] private Exit _exit;
        [SerializeField] private CapsuleCollider2D _getKeyCollider;
        private Dictionary<string, Action> actions = new();
        public Dictionary<string, Action> GetActions() => actions;

        private void Start()
        {
            _getKeyCollider.enabled = false;
            _getKeyCollider.isTrigger = true;
            actions.Add("TakeTheKeyCard2", TakeTheKey);
        }

        private void OnEnable() => CMDEventBus.Subscribe<CurrentEvent>(Brain);
        private void OnDisable() => CMDEventBus.Unsubscribe<CurrentEvent>(Brain);

        private void TakeTheKey()
        {
            CMDEventBus.Publish(new CurrentEvent("HumanToTheShelf"));
            _getKeyCollider.enabled = true;
        }

        private void Brain(CurrentEvent currentEvent)
        {
            if (currentEvent.EventName != "HumanToTheShelf")
            {
            //    _getKeyCollider.enabled = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Human"))
            {
                Debug.Log($"Collision with human");
                if (other.gameObject.TryGetComponent<HumanController>(out var humanController))
                {
                    humanController.HaveKeyCardNumber = 2;
                    _exit.AddExitWay();
                }
            }
        }
    }
}