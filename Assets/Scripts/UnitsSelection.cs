using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsSelection : MonoBehaviour {
  private bool _isDragginMouseBox = false;
  private Vector3 _dragStartPosition;
  private Ray _ray;
  private RaycastHit _raycastHit;
  private Dictionary<int, List<UnitManager>> _selectionGroups = new Dictionary<int, List<UnitManager>>();

  void Update() {
    if (Input.GetMouseButtonDown(0)) {
      _isDragginMouseBox = true;
      _dragStartPosition = Input.mousePosition;
    }

    if (Input.GetMouseButtonUp(0)) {
      _isDragginMouseBox = false;
    }

    if (_isDragginMouseBox && _dragStartPosition != Input.mousePosition) {
      _SelectUnitsInDraggingBox();
    }

    if(Globals.SELECTED_UNITS.Count > 0) {
      if(Input.GetKeyDown(KeyCode.Escape)) {
        _DeselectAllUnits();
      }
      if (Input.GetMouseButtonDown(0)) {
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(_ray, out _raycastHit, 1000f)) {
          if(_raycastHit.transform.tag == "Terrain") {
            _DeselectAllUnits();
          }
        }
      }
    }

    // manage selection groups with alphanumeric keys
    if (Input.anyKeyDown) {
      int alphaKey = Utils.GetAlphaKeyValue(Input.inputString);
      if (alphaKey != -1) {
        if(Input.GetKey(KeyCode.LeftControl)
          || Input.GetKey(KeyCode.RightControl)
          || Input.GetKey(KeyCode.LeftApple)
          || Input.GetKey(KeyCode.RightApple)){
          _CreateSelectionGroup(alphaKey);
        } else {
          _ReselectGroup(alphaKey);
        }
      }
    }
  }

  private void _SelectUnitsInDraggingBox() {
    Bounds selectionBounds = Utils.GetViewportBounds(
      Camera.main,
      _dragStartPosition,
      Input.mousePosition
      );
    GameObject[] selectableUnits = GameObject.FindGameObjectsWithTag("Unit");
    bool inBounds;
    foreach (GameObject unit in selectableUnits) {
      inBounds = selectionBounds.Contains(
        Camera.main.WorldToViewportPoint(unit.transform.position)
        );
      if(inBounds) {
        unit.GetComponent<UnitManager>().Select();
      } else {
        unit.GetComponent<UnitManager>().Deselect();
      }
    }
  }

  private void OnGUI() {
    if (_isDragginMouseBox) {
      // Create a rect from both mouse positions
      var rect = Utils.GetScreenRect(_dragStartPosition, Input.mousePosition);
      Utils.DrawScreenRect(rect, new Color(0.5f, 1f, 0.4f, 0.2f));
      Utils.DrawScreenRectBorder(rect, 1, new Color(0.5f, 1f, 0.4f));
    }
  }

  private void _DeselectAllUnits() {
    List<UnitManager> selectedUnits = new List<UnitManager>(Globals.SELECTED_UNITS);
    foreach(UnitManager um in selectedUnits) {
      um.Deselect();
    }
  }

  public void SelectUnitsInGroup(int groupIndex) {
    _ReselectGroup(groupIndex);
  }

  private void _CreateSelectionGroup(int groupIndex) {
    // check there are units currently selected
    if(Globals.SELECTED_UNITS.Count == 0) {
      if (_selectionGroups.ContainsKey(groupIndex)) {
        _RemoveSelectionGroup(groupIndex);
      }
      return;
    }

    List<UnitManager> groupUnits = new List<UnitManager>(Globals.SELECTED_UNITS);
    _selectionGroups[groupIndex] = groupUnits;
  }

  private void _RemoveSelectionGroup(int groupIndex) {
    _selectionGroups.Remove(groupIndex);
  }

  private void _ReselectGroup(int groupIndex) {
    // check the group actually is defined
    if (!_selectionGroups.ContainsKey(groupIndex)) { return; }
    _DeselectAllUnits();
    foreach(UnitManager um in _selectionGroups[groupIndex]) {
      um.Select();
    }
  }
}
