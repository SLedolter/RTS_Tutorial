using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsSelection : MonoBehaviour {
  private bool _isDragginMouseBox = false;
  private Vector3 _dragStartPosition;

  void Update() {
    if(Input.GetMouseButtonDown(0)) {
      _isDragginMouseBox = true;
      _dragStartPosition = Input.mousePosition;
    }

    if(Input.GetMouseButtonUp(0)) {
      _isDragginMouseBox = false;
    }
  }

  private void OnGUI() {
    if(_isDragginMouseBox) {
      // Create a rect from both mouse positions
      var rect = Utils.GetScreenRect(_dragStartPosition, Input.mousePosition);
      Utils.DrawScreenRect(rect, new Color(0.5f, 1f, 0.4f, 0.2f));
      Utils.DrawScreenRectBorder(rect, 1, new Color(0.5f, 1f, 0.4f));
    }
  }
}
