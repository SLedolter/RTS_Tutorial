using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviour
{
  private BuildingPlacer _buildingPlacer;

  public Transform buildingMenu;
  public GameObject buildingButtonPrefab;
  public Transform resourcesUIParent;
  public GameObject gameResourceDisplayPrefab;

  private Dictionary<string, TMP_Text> _resourceTexts;

  private void Awake() {
    // create texts for each in-game resource (gold, wood, stone, ...)
    _resourceTexts = new Dictionary<string, TMP_Text>();
    foreach(KeyValuePair<string, GameResource> pair in Globals.GAME_RESOURCES) {
      GameObject display = Instantiate(gameResourceDisplayPrefab, resourcesUIParent);
      display.name = pair.Key;
      _resourceTexts[pair.Key] = display.transform.Find("Text").GetComponent<TMP_Text>();
      _SetResourceText(pair.Key, pair.Value.Amount);
      Debug.Log(pair.Key);
    }
    _buildingPlacer = GetComponent<BuildingPlacer>();

    // create buttons for each building type
    for(int i = 0; i < Globals.BUILDING_DATA.Length; i++) {
      GameObject button = GameObject.Instantiate(
        buildingButtonPrefab,
        buildingMenu
        );
      string code = Globals.BUILDING_DATA[i].Code;
      button.name = code;
      button.transform.Find("Text").GetComponent<TMP_Text>().text = code;
      Button b = button.GetComponent<Button>();
      _AddBuildingButtonListener(b, i);
    }
  }

  private void _SetResourceText(string resource, int value) {
    _resourceTexts[resource].text = value.ToString();
  }

  public void UpdateResourceTexts() {
    foreach(KeyValuePair<string, GameResource> pair in Globals.GAME_RESOURCES) {
      _SetResourceText(pair.Key, pair.Value.Amount);
    }
  }

  private void _AddBuildingButtonListener(Button b, int i) {
    b.onClick.AddListener(() => _buildingPlacer.SelectPlacedBuilding(i));
  }
}
