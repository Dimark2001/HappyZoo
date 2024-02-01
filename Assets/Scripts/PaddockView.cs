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
        if(_paddock.NeededFood <= 0)
            return;
        _indicator.SetActive(true);
        var a = _paddock.NeededFood - _paddock.PaddockHasFood;
        _textMesh.text = a.ToString();
    }

    private void HideIndicator()
    {
        _indicator.SetActive(false);
    }
}
