using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHandler {
  public static void LoadGameData() {
    Debug.Log("LoadGameData");
    Globals.BUILDING_DATA = Resources.LoadAll<BuildingData>("ScriptableObjects/Units/Buildings") as BuildingData[];
  }
}
