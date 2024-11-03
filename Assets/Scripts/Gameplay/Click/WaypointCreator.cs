using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaypointCreator : MonoBehaviour
{
    [SerializeField] private GameObject pointPrefab;
    GameObject _lastClick;
	HumanController humanController;

	void Awake()
	{
		humanController = GetComponent<HumanController>();
	}

    public HumanController GetHumanController() => humanController;
    public Transform CreatePoint(Vector3 mousePosition)
    {
        mousePosition.z = 0;

        GameObject point = Instantiate(pointPrefab, mousePosition, Quaternion.identity);
        DeletePrevPoint(point);
        return point.transform;
    }
    private void DeletePrevPoint(GameObject newPoint)
    {
       if(_lastClick != null) 
        { 
            Destroy(_lastClick); _lastClick = null; 
        } 
        _lastClick = newPoint;
    }
}