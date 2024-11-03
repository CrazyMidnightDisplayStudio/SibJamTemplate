/* 
    ------------------- Code Monkey -------------------
    
    Thank you for downloading the Code Monkey Utilities
    I hope you find them useful in your projects
    If you have any questions use the contact form
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Game.Services.Debugging.Gameplay.Radar;

public class Radar : MonoBehaviour {

    [SerializeField] private LayerMask radarLayerMask;
    [SerializeField] private Transform playerTransform;

    private Transform sweepTransform;
    private float rotationSpeed;
    private float radarDistance;
    private List<Collider2D> colliderList;

    private void Awake() {
        sweepTransform = transform.Find("Sweep");
        rotationSpeed = 180f;
        radarDistance = 150f;
        colliderList = new List<Collider2D>();
    }

    private void Update() {
        transform.position = playerTransform.position;
        float previousRotation = (sweepTransform.eulerAngles.z % 360) - 180;
        sweepTransform.eulerAngles -= new Vector3(0, 0, rotationSpeed * Time.deltaTime);
        float currentRotation = (sweepTransform.eulerAngles.z % 360) - 180;

        if (previousRotation < 0 && currentRotation >= 0) {
            // Half rotation
            colliderList.Clear();
        }

        RaycastHit2D[] raycastHit2DArray = Physics2D.RaycastAll(transform.position, UtilsClass.GetVectorFromAngle(sweepTransform.eulerAngles.z), radarDistance, radarLayerMask);
        foreach (RaycastHit2D raycastHit2D in raycastHit2DArray) {
            if (raycastHit2D.collider != null) {
                // Hit something
                if (!colliderList.Contains(raycastHit2D.collider)) {
                    // Hit this one for the first time
                    colliderList.Add(raycastHit2D.collider);
                    //CMDebug.TextPopup("Ping!", raycastHit2D.point);
                    //radarPing.transform.SetParent(transform);
                    raycastHit2D.collider.TryGetComponent(typeof(RadarTarget), out var target);
                    if (target is RadarTarget radarTarget)
                    {
                        radarTarget.Ping();
                    }
                }
            }
        }
    }

}
