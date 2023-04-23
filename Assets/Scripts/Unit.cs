using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Unit {
  protected UnitData _data;
  protected Transform _transform;
  protected int _currentHealth; 

  public Unit(UnitData data) {
    _data = data;
    _currentHealth = data.healthpoints;
    _transform = (GameObject.Instantiate(data.prefab) as GameObject).transform;
  }

  

  public void SetPosition(Vector3 position) {
    _transform.position = position;
  }

  public UnitData Data { get { return _data; } }
  public string Code { get => _data.code; }
  public Transform Transform { get { return _transform; } }
  public int HP { get => _currentHealth; set => _currentHealth = value; }
  public int MaxHP { get => _data.healthpoints; }

  public virtual void Place() {
    // remove "is trigger" flag from box collider to allow
    // for collisions with units
    _transform.GetComponent<BoxCollider>().isTrigger = false;
    // update game resources: remove the cost of the building
    // from each game resource
    foreach(ResourceValue resource in _data.cost) {
      Globals.GAME_RESOURCES[resource.code].AddAmount(-resource.amount);
    }
  }

  public bool CanBuy() {
    return _data.CanBuy();
  }
}
