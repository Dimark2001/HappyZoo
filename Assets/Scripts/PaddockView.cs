using TMPro;
using UnityEngine;

[RequireComponent(typeof(Paddock))]
public class PaddockView : MonoBehaviour
{
    [SerializeField] private GameObject _indicator;
    [SerializeField] private TextMeshPro _textMesh;
    
    private Paddock _paddock;
    void Awake()
    {
        _paddock = GetComponent<Paddock>();
    }

    private void OnEnable()
    {
        _paddock.CountOfResourceChanged += DrawIndicator;
        _paddock.OnAte += HideIndicator;
    }

    private void OnDisable()
    {
        _paddock.CountOfResourceChanged -= DrawIndicator;
        _paddock.OnAte -= HideIndicator;
    }

    private void DrawIndicator()
    {
        _indicator.SetActive(true);
        _textMesh.text = _paddock.CountOfResource.ToString();
    }

    private void HideIndicator()
    {
        _indicator.SetActive(false);
    }
}
