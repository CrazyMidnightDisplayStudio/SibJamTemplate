using Core.Services.EventBus;
using Game.Services.Events;
using UnityEngine;

namespace Gameplay.Items
{
    public class Body2 : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                CMDEventBus.Publish(new CurrentEvent("PickupKeyCard1"));
                Destroy(GetComponent<CapsuleCollider2D>());
                Destroy(this);
            }
        }
    }
}