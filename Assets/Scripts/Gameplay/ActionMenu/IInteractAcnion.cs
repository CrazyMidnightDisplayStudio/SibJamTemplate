using System;
using System.Collections.Generic;

namespace Assets.Scripts.Gameplay.ActionMenu
{
    public interface IInteractAcnion
    {
        public Dictionary<string, Action> GetActions();
    }
}
