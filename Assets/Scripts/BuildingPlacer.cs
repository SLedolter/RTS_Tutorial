using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacer : MonoBehaviour {
  private Building _placedBuilding;
  private Ray _ray;
  private RaycastHit _raycastHit;
  private Vector3 _lastPlacementPosition;

  public void SelectPlacedBuilding(int buildingDataIndex) {
    _PreparePlacedBuilding(buildingDataIndex);
  }

  private void Awake() { }

  void Update() {
    if (_placedBuilding != null) {
      if (Input.GetKeyUp(KeyCode.Escape)) {
        _CancelPlacedBuilding();
        return;
      }

      _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(
        _ray,
        out _raycastHit,
        1000f,
        Globals.TERRAIN_LAYER_MASK
        )) {
        _placedBuilding.SetPosition(_raycastHit.point);
        if (_lastPlacementPosition != _raycastHit.point) {
          _placedBuilding.CheckValidPlacement();
        }
        _lastPlacementPosition = _raycastHit.point;
      }

      if (_placedBuilding.HasValidPlacement 
        && Input.GetMouseButtonUp(0)
        && !EventSystem.current.IsPointerOverGameObject()) {
        _PlaceBuilding();
      }
    }
  }

  void _PreparePlacedBuilding(int buildingDataIndex) {
    // destroy the previous "phantom" if ther is one
    if (_placedBuilding != null && !_placedBuilding.IsFixed) {
      Destroy(_placedBuilding.Transform.gameObject);
    }

    Building building = new Building(
      Globals.BUILDING_DATA[buildingDataIndex]
      );
    building.Transform.GetComponent<BuildingManager>().Initialize(building);
    _placedBuilding = building;
    _lastPlacementPosition = Vector3.zero;
  }

  private void _CancelPlacedBuilding() {
    // destroy the "phantom" building
    Destroy(_placedBuilding.Transform.gameObject);
    _placedBuilding = null;
  }

  private void _PlaceBuilding() {
    _placedBuilding.Place();
    if (_placedBuilding.CanBuy()) {
      _PreparePlacedBuilding(_placedBuilding.DataIndex);
    } else {
      _placedBuilding = null;
    }
    EventManager.TriggerEvent("UpdateResourceTexts");
    EventManager.TriggerEvent("CheckBuildingButtons");
  }
}
