using LTTP.Sprites;
using Nez;
using Nez.Tiled;

namespace LTTP.Entities.Sword
{
    public class SwordCollisionHandler
    {
        private TiledMapRenderer _tiledMapRenderer;
        private TmxLayer _detailLayer => _tiledMapRenderer.TiledMap.Layers["details"] as TmxLayer;

        public SwordCollisionHandler(TiledMapRenderer tiledMapRenderer)
        {
            _tiledMapRenderer = tiledMapRenderer;
        }

        public bool HandleReplaceTileCollision(TmxLayerTile tile, TileLookupEnum collisionTile, TileLookupEnum newTile)
        {
            if (tile.Gid != TileLookup.GetTileValue(collisionTile))
                return false;

            //_detailLayer.RemoveTile(tile.X, tile.Y);
            tile.Gid = TileLookup.GetTileValue(newTile);

            _tiledMapRenderer.CollisionLayer.RemoveTile(tile.X, tile.Y);

            return true;
        }

        public void HandleCollisionResult(SwordCollisionResult collisionResult)
        {
            var removedCount = 0;

            foreach (var tile in collisionResult.DetailTiles)
            {
                if (HandleReplaceTileCollision(tile, TileLookupEnum.GrassBush, TileLookupEnum.GrassBushChopped))
                    removedCount++;

                if (HandleReplaceTileCollision(tile, TileLookupEnum.GrassBushPurple, TileLookupEnum.GrassBushChoppedPurple))
                    removedCount++;
            }

            if (removedCount > 0)
            {
                _tiledMapRenderer.RemoveColliders();
                _tiledMapRenderer.AddColliders();
            }
        }
    }
}