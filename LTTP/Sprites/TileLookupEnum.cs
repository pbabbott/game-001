namespace LTTP.Sprites
{
    public static class TileLookup
    {
        /// <summary>
        /// Translates TiledMapEditor to Nez GID
        /// </summary>
        public static int GetTileValue(TileLookupEnum tile) => (int)tile + 1;
    }

    /// <summary>
    /// Values match what's in TiledMapEditor
    /// </summary>
    public enum TileLookupEnum
    {
        GrassBush = 679,
        GrassBushChopped = 776,
        GrassBushPurple = 1380,
        GrassBushChoppedPurple = 1477
    }
}