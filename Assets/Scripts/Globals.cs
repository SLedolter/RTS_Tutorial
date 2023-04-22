public class Globals {
  public static BuildingData[] BUILDING_DATA = new BuildingData[] {
    new BuildingData("House", 100),
    new BuildingData("Tower", 50)
  };

  public static int TERRAIN_LAYER_MASK = 1 << 8; // Layer-Index bitwise

}