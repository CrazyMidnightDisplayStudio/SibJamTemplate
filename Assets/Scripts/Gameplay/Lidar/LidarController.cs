using Cysharp.Threading.Tasks;
using Debugging;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game.Services.Debugging.Gameplay.Lidar
{
    public class LidarController : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private float _radius = 4f;
        [SerializeField] private float _particleSize = 0.1f;
        
        private Raycaster _raycaster;
        private LidarRenderer _lidarRenderer;

        private void Awake()
        {
            _raycaster = new Raycaster(360, _radius, _playerTransform);
            _lidarRenderer = new LidarRenderer(_particleSystem, _particleSize);
        }

        private void OnEnable()
        {
            _raycaster.OnHitScanned += _lidarRenderer.DrawParticle;
        }

        private void OnDisable()
        {
            _raycaster.OnHitScanned -= _lidarRenderer.DrawParticle;
        }

        public void TestScan()
        {
            _raycaster.CircleCastAsync(_playerTransform.position, 0.5f).Forget();
        }
    }
}