using System.Collections.Generic;

public class Globals {
  public static BuildingData[] BUILDING_DATA;

  public static int TERRAIN_LAYER_MASK = 1 << 8; // Layer-Index bitwise

  public static Dictionary<string, GameResource> GAME_RESOURCES =
    new Dictionary<string, GameResource>() {
      { "gold", new GameResource("Gold", 2000) },
      { "wood", new GameResource("Wood", 750) },
      { "stone", new GameResource("Stone", 250) }
    };

  public static List<UnitManager> SELECTED_UNITS = new List<UnitManager>();
}