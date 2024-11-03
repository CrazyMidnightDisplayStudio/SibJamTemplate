using System;
using System.Collections.Generic;
using UnityEngine;
using static HumanController;

namespace Assets.Scripts.Gameplay.ActionMenu
{
    public class ActionHumanMove: IInteractAcnion
    {
        private Dictionary<string, Action> _actions = new();
        private HumanController _humanController;
        private Transform _pointMove;

        public ActionHumanMove(HumanController humanController, Transform pointMove) 
        {
            _humanController = humanController;
            _pointMove = pointMove;
            InitAction();
        }

        private void InitAction() 
        {
            _actions.Add("Stealth", () => Stealth());
            _actions.Add("Walk", () => Walk());
            _actions.Add("Run", () => Run());
        }
        private void Stealth()
        {
            Move(MovementStyle.Stealth);
        }
        private void Walk()
        {
            Move(MovementStyle.Walk);
        }
        private void Run()
        {
            Debug.Log("Move");
            Move(MovementStyle.Run);
        }
        private void Move(MovementStyle movementStyle)
        {
            _humanController.CurrentMovementStyle = movementStyle;
            _humanController.SetTarget(_pointMove);
        }

        public Dictionary<string, Action> GetActions()
        {
            return _actions;
        }
    }
}
