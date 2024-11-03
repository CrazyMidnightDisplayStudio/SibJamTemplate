using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Services.Debugging.Gameplay.Radar
{
    public class RadarTarget : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _targetRenderer;
        [SerializeField] private bool _isStatic = true;
        
        private SpriteRenderer _echoRenderer;
        
        private float _disappearTimer;
        private float _disappearTimerMax;
        private Color _color;
        private Color _startColor;

        private bool _isEcho;

        private void Start()
        {
            _echoRenderer = gameObject.AddComponent<SpriteRenderer>();
            _echoRenderer.sprite = _targetRenderer.sprite;
            _echoRenderer.color = _targetRenderer.color;
            _disappearTimerMax = 1f;
            _color = _targetRenderer.material.color;
            _startColor = _color;
        }

        private void Update()
        {
            if (_isStatic || _isEcho) return;
            
            transform.position = _targetRenderer.transform.position;
        }

        public void Ping()
        {
            Debug.Log("Ping");
            transform.position = _targetRenderer.transform.position;
            transform.rotation = _targetRenderer.transform.rotation;
            _disappearTimer = 0f;
            _echoRenderer.material.color = _startColor;
            StartDisappear().Forget();
        }

        private async UniTask StartDisappear()
        {
            _isEcho = true;
            while (_disappearTimer < _disappearTimerMax)
            {
                _disappearTimer += Time.deltaTime;
                _color.a = Mathf.Lerp(_disappearTimerMax, 0f, _disappearTimer / _disappearTimerMax);
                _echoRenderer.material.color = _color;
                _echoRenderer.sprite = _targetRenderer.sprite;
                await UniTask.NextFrame();
            }

            _color.a = 0f;
            _echoRenderer.material.color = _color;
            _isEcho = false;
        }
    }
}