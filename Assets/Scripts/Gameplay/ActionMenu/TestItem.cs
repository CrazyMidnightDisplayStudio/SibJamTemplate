using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.ActionMenu
{
    public class TestItem : MonoBehaviour, IInteractAcnion
    {

        private Dictionary<string, Action> _actions = new Dictionary<string, Action>();
        private WaypointCreator _waypointCreator;

        private void Start()
        {
            InitActionas();
            _waypointCreator = FindAnyObjectByType<WaypointCreator>();
        }
        public Dictionary<string, Action> GetActions()
        {

            return _actions;
        }
        private void InitActionas()
        {
            _actions.Add("Сome up", () => Move());
            _actions.Add("Say", () => Say());
        }

        private void Move()
        {
            var target = _waypointCreator.CreatePoint(transform.position);
            _waypointCreator.GetHumanController().SetTarget(target);
        }
        private void Say()
        {
            Debug.Log("Говорим");
        }
    }
}
