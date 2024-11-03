using UnityEngine;

public class TerrorEffect : MonoBehaviour
{
    [SerializeField] float terrorFactor = 0.01f;
    [SerializeField] int effectFrequency = 2;

    private HumanController guest;
    private float timer;

    void Update()
    {
        if (!guest)
            return;

        timer += Time.deltaTime;
        if (timer >= 1f / effectFrequency)
        {
            guest.Scare(terrorFactor);
            timer = 0f;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Human"))
        {
            guest = other.gameObject.GetComponent<HumanController>();
            guest.ToggleSafeState();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Human"))
        {
            guest.ToggleSafeState();
            guest = null;
            timer = 0f;
        }
    }
}
