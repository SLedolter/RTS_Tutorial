using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomEventData {
  public UnitData unitData;
  public Unit unit;

  public CustomEventData(UnitData buildingData) {
    this.unitData = buildingData;
    this.unit = null;
  }

  public CustomEventData(Unit unit) {
    this.unitData = null; 
    this.unit = unit;
  }
}
