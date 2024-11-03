using UnityEngine;
using UnityEngine.Events;

public class WaypointCreator : MonoBehaviour
{
    [SerializeField] private GameObject pointPrefab;

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
        return point.transform;
    }
}