using UnityEngine;

public class ArrowBox : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerControls player))
        {
            arrow.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerControls player))
        {
            arrow.SetActive(false);
        }
    }
}
