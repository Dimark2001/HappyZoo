using System;
using DG.Tweening;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Transform arrow;
    [SerializeField] private AnimationCurve arrowCurve;
    [SerializeField] private AnimationCurve arrowCurve1;
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private float height = 20;
    private Sequence _moveSeq;

    private void OnEnable()
    {
        StartMoveAnim();
    }

    private void OnDisable()
    {
        arrow.localPosition = Vector3.zero;
        _moveSeq.Kill();
    }

    private void StartMoveAnim()
    {
        _moveSeq = DOTween.Sequence();
        _moveSeq.Append(arrow.DOLocalMove(new Vector3(0,height,0), duration)).SetEase(arrowCurve);
        _moveSeq.Append(arrow.DOLocalMove(Vector3.zero, duration)).SetEase(arrowCurve1);
        _moveSeq.AppendCallback(StartMoveAnim);
    }
}
