using System;
using System.Collections.Generic;

namespace Assets.Scripts.Gameplay.ActionMenu
{
    public interface IInteractAction
    {
        public Dictionary<string, Action> GetActions();
    }
}
