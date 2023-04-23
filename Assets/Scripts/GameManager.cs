using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
  private void Awake() {
    DataHandler.LoadGameData();

    Debug.Log(Screen.width + "/" + Screen.height);
  }
}
