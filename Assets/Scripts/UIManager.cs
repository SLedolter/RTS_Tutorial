using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour {
  private BuildingPlacer _buildingPlacer;

  public Transform buildingMenu;
  public GameObject buildingButtonPrefab;
  public Transform resourcesUIParent;
  public GameObject gameResourceDisplayPrefab;
  public GameObject gameResourceCostPrefab;
  public GameObject infoPanel;
  public Color invalidTextColor;

  private Dictionary<string, TMP_Text> _resourceTexts;
  private Dictionary<string, Button> _buildingButtons;
  private TMP_Text _infoPanelTitleText;
  private TMP_Text _infoPanelDescriptionText;
  private Transform _infoPanelResourcesCostParent;

  private void Awake() {
    // create texts for each in-game resource (gold, wood, stone, ...)
    _resourceTexts = new Dictionary<string, TMP_Text>();
    foreach (KeyValuePair<string, GameResource> pair in Globals.GAME_RESOURCES) {
      GameObject display = Instantiate(gameResourceDisplayPrefab, resourcesUIParent);
      display.name = pair.Key;
      _resourceTexts[pair.Key] = display.transform.Find("Text").GetComponent<TMP_Text>();
      SetResourceText(pair.Key, pair.Value.Amount);
      Debug.Log(pair.Key);
    }
    _buildingPlacer = GetComponent<BuildingPlacer>();

    // create buttons for each building type
    _buildingButtons = new Dictionary<string, Button>();
    for (int i = 0; i < Globals.BUILDING_DATA.Length; i++) {
      BuildingData data = Globals.BUILDING_DATA[i];
      GameObject button = Instantiate(buildingButtonPrefab, buildingMenu);
      button.name = data.unitName;
      button.transform.Find("Text").GetComponent<TMP_Text>().text = data.unitName;
      Button b = button.GetComponent<Button>();
      _AddBuildingButtonListener(b, i);
      _buildingButtons[data.code] = b;
      if (!Globals.BUILDING_DATA[i].CanBuy()) {
        b.interactable = false;
      }
      button.GetComponent<BuildingButton>().Initialize(Globals.BUILDING_DATA[i]);
    }

    Transform infoPanelTransform = infoPanel.transform;
    _infoPanelTitleText = infoPanelTransform.Find("Content/Title").GetComponent<TMP_Text>();
    _infoPanelDescriptionText = infoPanelTransform.Find("Content/Description").GetComponent<TMP_Text>();
    _infoPanelResourcesCostParent = infoPanelTransform.Find("Content/ResourcesCost");
    ShowInfoPanel(false);
  }

  private void OnEnable() {
    EventManager.AddListener("UpdateResourceTexts", _OnUpdateResourceTexts);
    EventManager.AddListener("CheckBuildingButtons", _OnCheckBuildingButtons);
    EventManager.AddTypedListener("HoverBuildingButton", _OnHoverBuildingButton);
    EventManager.AddListener("UnhoverBuildingButton", _OnUnhoverBuildingButton);
  }

  private void OnDisable() {
    EventManager.RemoveListener("UpdateResourceTexts", _OnUpdateResourceTexts);
    EventManager.RemoveListener("CheckBuildingButtons", _OnCheckBuildingButtons);
    EventManager.RemoveTypeListener("HoverBuildingButton", _OnHoverBuildingButton);
    EventManager.RemoveListener("UnhoverBuildingButton", _OnUnhoverBuildingButton);
  }

  private void SetResourceText(string resource, int value) {
    _resourceTexts[resource].text = value.ToString();
  }

  private void _OnHoverBuildingButton(CustomEventData data) {
    SetInfoPanel(data.buildingData);
    ShowInfoPanel(true);
  }

  private void _OnUnhoverBuildingButton() {
    ShowInfoPanel(false);
  }

  private void _OnUpdateResourceTexts() {
    foreach (KeyValuePair<string, GameResource> pair in Globals.GAME_RESOURCES) {
      SetResourceText(pair.Key, pair.Value.Amount);
    }
  }

  private void _AddBuildingButtonListener(Button b, int i) {
    b.onClick.AddListener(() => _buildingPlacer.SelectPlacedBuilding(i));
  }

  private void _OnCheckBuildingButtons() {
    foreach (UnitData data in Globals.BUILDING_DATA) {
      _buildingButtons[data.code].interactable = data.CanBuy();
    }
  }

  private void SetInfoPanel(UnitData data) {
    // update texts
    if (data.code != "") { _infoPanelTitleText.text = data.code.FirstCharacterToUpper(); }
    if (data.description != "") { _infoPanelDescriptionText.text = data.description; }

    // clear resource costs andc reinstantiate new ones
    foreach(Transform child in _infoPanelResourcesCostParent) {
      Destroy(child.gameObject);
    }

    if(data.cost.Count > 0) {
      GameObject g; Transform t;
      foreach(ResourceValue resource in data.cost) {
        g = GameObject.Instantiate(gameResourceCostPrefab, _infoPanelResourcesCostParent);
        t = g.transform;
        t.Find("Text").GetComponent<TMP_Text>().text = resource.amount.ToString();
        t.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>(
          $"Textures/GameResources/{resource.code}");
        if (Globals.GAME_RESOURCES[resource.code].Amount < resource.amount) {
          t.Find("Text").GetComponent<TMP_Text>().color = invalidTextColor;
        }
      }
    }
  }

  public void ShowInfoPanel(bool show) {
    infoPanel.SetActive(show);
  }
}
