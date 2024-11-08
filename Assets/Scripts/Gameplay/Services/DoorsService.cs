﻿using System.Collections.Generic;
using Gameplay.Items;
using UnityEngine;

namespace Game.Services.Debugging.Gameplay.Services
{
    public class DoorsService
    {
        private Dictionary<int, Door> _doorsMap;

        public DoorsService()
        {
            _doorsMap = new Dictionary<int, Door>();
            
            GameObject doors = GameObject.Find("Doors");
            if (doors == null)
            {
                Debug.LogError("Doors not found on scene");
                return;
            }

            var doorsComponents = doors.GetComponentsInChildren<Door>();
            foreach (var door in doorsComponents)
            {
                _doorsMap.Add(door.DoorID, door);
            }

            if (_doorsMap.Count != doorsComponents.Length)
            {
                Debug.LogError("Non unique doors id");
            }
            _doorsMap[10].Lock();
        }

        public void StayOpenDoors(int[] doorIDs)
        {
            foreach (var id in doorIDs)
            {
                _doorsMap[id].IsStayOpen = true;
            }
        }

        public void LockDoors(int[] doorIDs)
        {
            foreach (var id in doorIDs)
            {
                _doorsMap[id].Lock();
            }
        }

        public void UnlockDoors(int[] doorIDs)
        {
            foreach (var id in doorIDs)
            {
                _doorsMap[id].Unlock();
            }
        }

        public void OpenDoors(int[] doorIDs)
        {
            foreach (var id in doorIDs)
            {
                _doorsMap[id].OpenDoor();
            }
        }

        public void CloseDoors(int[] doorIDs)
        {
            foreach (var id in doorIDs)
            {
                _doorsMap[id].CloseDoor();
            }
        }
    }
}