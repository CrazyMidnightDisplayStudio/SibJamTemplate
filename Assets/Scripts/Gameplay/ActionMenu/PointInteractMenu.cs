using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Gameplay.ActionMenu
{
    public class PointInteractMenu : MonoBehaviour
    {
        [SerializeField] private Transform _centerGO;
        [SerializeField] private float _distanceClick;
        private void Update()
        {
            // Проверяем, было ли нажатие левой кнопки мыши
            if (Input.GetMouseButtonDown(0))
            {
               Vector3 pointClick = Input.mousePosition;
                GetGameObjectsPoint(pointClick);
            }
        }

        //получаем список объектов на с которыми можно взаимодействовать
        public List<GameObject> GetGameObjectsPoint(Vector3 point)
        {
            List<GameObject> gameObjects = new List<GameObject>();

            Ray ray = Camera.main.ScreenPointToRay(point);
            RaycastHit[] hits;

            // Получаем все объекты, пересекаемые лучом
            hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                // Получаем объект, по которому был произведен клик
                GameObject clickedObject = hit.collider.gameObject;
                Debug.Log("Кликнули по объекту: " + clickedObject.name);
                gameObjects.Add(clickedObject);
            }

            return gameObjects;
        }
    }
}
