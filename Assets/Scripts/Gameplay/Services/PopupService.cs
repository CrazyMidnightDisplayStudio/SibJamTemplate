using Assets.Scripts.Gameplay.Popup;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Services
{
    public class PopupService
    {
        private PopupNoUIManager _popupNoUIManager;
        public PopupService(PopupNoUI prefPopup)
        {
            _popupNoUIManager = new PopupNoUIManager(prefPopup, 1);
        }

        public PopupNoUI GetPopup(string text, Vector3 position)
        {
            var popup = _popupNoUIManager.GetPopup(text);
            popup.SetPosition(position);
            return popup;
        }
        public PopupNoUI GetPopup(string text, Vector3 position, float timeLife)
        {
            var popup = GetPopup(text, position);
            popup.SetTimeLife(timeLife);
            return popup;
        }
        public void DisablePopup(PopupNoUI popupNoUI)
        {
            _popupNoUIManager.ReleasePopup(popupNoUI);
        }
    }
}
