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
        [SerializeField] private CapsuleCollider2D _getKeyCollider;
        private Dictionary<string, Action> actions = new();
        public Dictionary<string, Action> GetActions() => actions;

        private void Start()
        {
            _getKeyCollider.enabled = false;
            _getKeyCollider.isTrigger = true;
            actions.Add("TakeTheKeyCard2", TakeTheKey);
        }

        private void TakeTheKey()
        {
            CMDEventBus.Publish(new CurrentEvent("HumanToTheShelf"));
        }

        private void Brain(CurrentEvent currentEvent)
        {
            _getKeyCollider.enabled = currentEvent.EventName == "HumanToTheShelf";
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Human"))
            {
                Debug.Log($"Collidion with human");
                if (other.gameObject.TryGetComponent<HumanController>(out var humanController))
                {
                    humanController.HaveKeyCardNumber = 2;
                    //TODO ссылка на дверь, у которой меняем метод для открытия
                }
            }
        }
    }
}