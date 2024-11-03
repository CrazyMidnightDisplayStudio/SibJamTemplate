using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Services.Debugging.Gameplay.Lidar
{
    public class Raycaster
    {
        private int _rayCount;
        private float _scanRadius;
        private int _lidarHitLayerMask;
        private Transform _playerTransform;

        public event Action<Vector2> OnHitScanned;

        public Raycaster(int rayCount, float scanRadius, Transform playerTransform)
        {
            _rayCount = rayCount;
            _scanRadius = scanRadius;
            _playerTransform = playerTransform;
            _lidarHitLayerMask = 1 << LayerMask.NameToLayer("Walls");
        }

        public async UniTask CircleCastAsync(Vector2 position, float duration)
        {
            float timePerRay = duration / _rayCount;

            for (int i = 0; i < _rayCount; i++)
            {
                float angle = i * (360f / _rayCount) * Mathf.Deg2Rad;
                Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                RaycastHit2D hit = Physics2D.Raycast(position, direction, _scanRadius, _lidarHitLayerMask);
                
                // DrawLine
                Vector2 endPoint = position + direction * _scanRadius;
                Debug.DrawLine(position, endPoint, Color.green, timePerRay);
                
                if (hit.collider != null)
                {
                    Debug.DrawLine(hit.point,  hit.point + Vector2.up * 0.1f, Color.red, 0.5f);
                    OnHitScanned?.Invoke(hit.point);
                }
                await UniTask.WaitForSeconds(timePerRay);
            }
        }
    }
}