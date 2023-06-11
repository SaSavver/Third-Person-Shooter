using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SosokView : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private float _sense = 1f;

    [SerializeField]
    private float _yMaxDistance,
                  _xMaxDistance;

    [SerializeField]
    private RectTransform _innerCircle;

    private Vector3 _centerPos;

    public Vector2 Input { get; private set; }
    public Vector2 DisplayInput;

    private void Awake()
    {
        _centerPos = _innerCircle.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var offset = new Vector3(eventData.delta.x, eventData.delta.y, 0f);
        var nextPos = _innerCircle.localPosition + offset * _sense;
        var leftX = _centerPos.x - _xMaxDistance;
        var rightX = _centerPos.x + _xMaxDistance;
        var topY = _centerPos.y + _yMaxDistance;
        var bottomY = _centerPos.y - _yMaxDistance; 
        var x = Mathf.Clamp(nextPos.x, leftX, rightX);
        var y = Mathf.Clamp(nextPos.y, bottomY, topY);
        
        _innerCircle.localPosition = new Vector3(x, y, 0f);
        

        var xSign = x < _centerPos.x ? -1 : 1;
        var ySign = y < _centerPos.y ? -1 : 1;

        Input = new Vector2(
            Mathf.Abs(((x - _centerPos.x)) / _xMaxDistance) * xSign,
            Mathf.Abs(((y - _centerPos.x)) / _yMaxDistance) * ySign);

        DisplayInput = Input;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _innerCircle.localPosition = _centerPos;
        Input = Vector2.zero;
        DisplayInput = Input;
    }
}
