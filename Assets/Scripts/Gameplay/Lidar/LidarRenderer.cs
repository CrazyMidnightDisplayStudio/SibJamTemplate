using UnityEngine;

namespace Game.Services.Debugging.Gameplay.Lidar
{
    public class LidarRenderer
    {
        private ParticleSystem _particleSystem;
        private ParticleSystem.EmitParams _emitParams;
        private float _particleSize;

        public LidarRenderer(ParticleSystem particleSystem, float particleSize)
        {
            _particleSystem = particleSystem;
            _particleSize = particleSize;
        }
        
        public void DrawParticle(Vector2 position)
        {
            _emitParams.position = new Vector3(position.x, position.y, 0);
            _emitParams.startSize = _particleSize;
            _particleSystem.Emit(_emitParams, 1);
        }
    }
}