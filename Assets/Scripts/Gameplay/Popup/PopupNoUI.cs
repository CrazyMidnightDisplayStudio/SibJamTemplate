using R3;
using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Popup
{
    public class PopupNoUI : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _tmp;
        private IDisposable _timerDisposable;

        private void Awake()
        {
            if (_tmp == null) _tmp = GetComponent<TextMeshPro>();
        }

        public void Construct(string text, Color colorText, int fontSize = -1, TMPro.FontStyles fontStyle = TMPro.FontStyles.Normal)
        {
            _tmp.text = text;
            _tmp.color = colorText;
            _tmp.fontSize = fontSize;
            _tmp.fontStyle = fontStyle;
        }

        public PopupNoUI SetText(string text)
        {
            _tmp.text = text;
            return this;
        }
        public PopupNoUI SetColor(Color color)
        {
            _tmp.color = color;
            return this;
        }
        public void SetActive(bool active) => gameObject.SetActive(active);
        public void SetParent(Transform parent)
        {
            gameObject.transform.SetParent(parent, false);
        }
        public void SetTimeLife(double second)
        {
            if (second > 0)
            {
                _timerDisposable = Observable.Timer(TimeSpan.FromSeconds(second))
                 .Subscribe(_ => SetActive(false))
                 .AddTo(this);
            }
        }
        public void SetPosition(Vector3 position) => gameObject.transform.position = position;

        private void OnDisable()
        {
            DisposeTimer();
        }
        private void DisposeTimer()
        {
            if (_timerDisposable != null)
            {
                _timerDisposable.Dispose();
                _timerDisposable = null;
            }
        }
    }
}
