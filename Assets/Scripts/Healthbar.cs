using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour {
  public RectTransform recTransform;

  private Transform _target;
  private Vector3 _lastTargetPosition;
  private Vector2 _pos;

  void Update() {
    if(!_target || _lastTargetPosition == _target.position) {  return; }
    SetPosition();
  }

  public void Initialize(Transform target) {
    _target = target;
  }

  public void SetPosition() {
    if(!_target) { return; }
    _pos = Camera.main.WorldToScreenPoint(_target.position);
    Debug.Log(_pos.ToString());
    recTransform.anchoredPosition = _pos;
    _lastTargetPosition = _target.position;
  }
}
