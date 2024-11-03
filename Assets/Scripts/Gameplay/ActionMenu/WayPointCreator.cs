using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.ActionMenu
{
    public class WayPointCreator : MonoBehaviour
    {
        [SerializeField] private GameObject pointPrefab;

        HumanController humanController;

        void Awake()
        {
            humanController = GetComponent<HumanController>();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                CreatePoint();
            }
        }

        private Transform CreatePoint()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            GameObject point = Instantiate(pointPrefab, mousePosition, Quaternion.identity);

            return point.transform;
        }
    }
}
