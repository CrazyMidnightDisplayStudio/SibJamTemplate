using System.Collections.Generic;
using R3;
using R3.Triggers;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Popup
{
    public class PopupNoUIManager
    {
        private Stack<PopupNoUI> _popupStack = new Stack<PopupNoUI>();
        public int Count => _popupStack.Count;
        private PopupNoUI _pfPopup;

        public PopupNoUIManager(PopupNoUI pfPopup, int count = 0)
        {
            _pfPopup = pfPopup;
            if (count > 0)
            {
                for (int i = 0; i < count; i++) CreatePopup();
            }
        }

        public PopupNoUI GetPopup(string text = "")
        {
            PopupNoUI popup = GetAvailablePopup() ?? CreateNewPopup();
            ConfigurePopup(popup, text);
            return popup;
        }

        private PopupNoUI GetAvailablePopup()
        {
            return _popupStack.Count > 0 ? _popupStack.Pop() : null;
        }

        private PopupNoUI CreateNewPopup()
        {
            PopupNoUI popup = CreatePopup();
            ObserveState(popup);
            return popup;
        }

        private void ConfigurePopup(PopupNoUI popup, string text)
        {
            popup.SetText(text);
            popup.SetActive(true);
        }

        private PopupNoUI CreatePopup()
        {
            return GameObject.Instantiate(_pfPopup);
        }

        private void ObserveState(PopupNoUI popup)
        {
           popup.OnDisableAsObservable().Subscribe(
               _ => { 
                   ReleasePopup(popup); 
               }
               ).AddTo(popup);
        }

        public void ReleasePopup(PopupNoUI popup)
        {
            popup.SetActive(false);
            if (!_popupStack.Contains(popup))
            {
                _popupStack.Push(popup);
            }
        }
    }
}
