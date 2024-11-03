using System;


namespace Assets.Scripts.Gameplay.ActionMenu
{
    public interface IInteract
    {
        public string Name { get; }
        public Action Interact { get; set; }
    }
}
