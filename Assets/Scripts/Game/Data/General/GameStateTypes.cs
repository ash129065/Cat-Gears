namespace Game.Data.Base
{
    public enum Area { SunnyIsland, CityStreets, NightStreets }
    public enum SlotOrder { RowCol, ColRow }
    public enum Origin { TopLeft, BottomLeft }
    public enum Dir { Cw, Ccw } // clockwise, counter-clockwise
    public enum GearSize { Small, Big }
    public enum CatType { Club, Spear, Archer, Frost, Rider, Wizard }
    public enum SpecialType { Coin, Rusty, Broken }
    public enum RaiderType { Raider, Runner, Brute, Boss }
    // TODO :: (1)
    public enum RoadLayout { Loop, TopBottom } // Loop: up to 6 cols; TopBottom: 7-9 cols
}