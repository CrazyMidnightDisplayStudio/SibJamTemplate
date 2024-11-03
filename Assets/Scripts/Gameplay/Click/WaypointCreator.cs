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

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreatePoint();
        }
    }

    private void CreatePoint()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        GameObject point = Instantiate(pointPrefab, mousePosition, Quaternion.identity);

		humanController.SetTarget(point.transform);
    }
}