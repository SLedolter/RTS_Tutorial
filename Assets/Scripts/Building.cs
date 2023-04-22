using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingPlacement {
  VALID,
  INVALID,
  FIXED
};

public class Building {
  private BuildingData _data;
  private Transform _transform;
  private int _currentHealth;
  private BuildingPlacement _placement;
  private List<Material> _materials;
  private BuildingManager _buildingManager;

  public Building(BuildingData data) {
    _data = data;
    _currentHealth = data.HP;

    GameObject g = GameObject.Instantiate(
      Resources.Load($"Prefabs/Buildings/{_data.Code}")
      ) as GameObject;
    _transform = g.transform;
    _placement = BuildingPlacement.VALID;
    _materials = new List<Material>();
    Material[] materials = _transform.Find("Mesh").GetComponent<Renderer>().materials;
    foreach (Material material in materials) {    
      _materials.Add(new Material(material));
    }

    _buildingManager = g.GetComponent<BuildingManager>();
    _placement = BuildingPlacement.VALID;
    SetMaterials();
  }

  public void SetMaterials() { SetMaterials(_placement); }
  public void SetMaterials(BuildingPlacement placement) {
    List<Material> materials;
    if (placement == BuildingPlacement.VALID) {
      Material refMaterial = Resources.Load("Materials/Valid") as Material;
      materials = new List<Material>();
      for (int i = 0; i < _materials.Count; i++) {
        materials.Add(refMaterial);
      }
    } else if (placement == BuildingPlacement.INVALID) {
      Material refMaterial = Resources.Load("Materials/Invalid") as Material;
      materials = new List<Material>();
      for(int i = 0; i < _materials.Count; i++) {
        materials.Add(refMaterial);
      }
    } else if(placement == BuildingPlacement.FIXED) {
      materials = _materials;
    } else {
      return;
    }
    _transform.Find("Mesh").GetComponent<Renderer>().materials = materials.ToArray();
  }

  public void SetPosition(Vector3 position) {
    _transform.position = position;
  }

  public string Code { get => _data.Code; }
  public Transform Transform { get { return _transform; } }
  public int HP { get => _currentHealth; set => _currentHealth = value; }
  public int MaxHP { get => _data.HP; }
  public int DataIndex {
    get {
      for (int i = 0; i < Globals.BUILDING_DATA.Length; i++) {
        if (Globals.BUILDING_DATA[i].Code == _data.Code) return i;
      }
      return -1;
    }
  }

  public void Place() {
    // set placerment state
    _placement = BuildingPlacement.FIXED;
    // change building materials
    SetMaterials();
    // remove "is trigger" flag from box collider to allow
    // for collisions with units
    _transform.GetComponent<BoxCollider>().isTrigger = false;
  }

  public void CheckValidPlacement() {
    if(_placement== BuildingPlacement.FIXED) { return; }
    _placement = _buildingManager.CheckPlacement()
    ? BuildingPlacement.VALID
      : BuildingPlacement.INVALID;
  }

  public bool IsFixed { get => _placement == BuildingPlacement.FIXED; }
  public bool HasValidPlacement { get => _placement == BuildingPlacement.VALID; }
}
