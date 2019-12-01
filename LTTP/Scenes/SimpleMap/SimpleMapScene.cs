
using Nez;
using Nez.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LTTP.Scenes.SimpleMap.Link;
using Nez.PhysicsShapes;

namespace LTTP.Scenes.SimpleMap
{
    public class SimpleMapScene : Scene
    {
        public override void Initialize()
        {
            base.Initialize();


            // setup a pixel perfect screen that fits our map
            SetDesignResolution(512, 256, SceneResolutionPolicy.ShowAllPixelPerfect);
            Screen.SetSize(512 * 3, 256 * 3);
            

            // load the TiledMap and display it with a TiledMapComponent
            var tiledEntity = CreateEntity("tiled-map-entity");
            var map = Content.LoadTiledMap("Content/maps/sample_map.tmx");
            
            var tiledMapRenderer = tiledEntity.AddComponent(new TiledMapRenderer(map, "collision"));
            tiledMapRenderer.SetLayersToRender(new[] { "bg", "terrain", "details", "collision" });

            // render below/behind everything else. our player is at 0 and projectile is at 1.
            tiledMapRenderer.RenderLayer = 10;


            // render our above-details layer after the player so the player is occluded by it when walking behind things
            var tiledMapDetailsComp = tiledEntity.AddComponent(new TiledMapRenderer(map));
            tiledMapDetailsComp.SetLayerToRender("fg details");
            tiledMapDetailsComp.RenderLayer = -1;
            
            // the details layer will write to the stencil buffer so we can draw a shadow when the player is behind it. we need an AlphaTestEffect
            // here as well
            tiledMapDetailsComp.Material = Material.StencilWrite();
            tiledMapDetailsComp.Material.Effect = Content.LoadNezEffect<SpriteAlphaTestEffect>();

            // setup our camera bounds with a 1 tile border around the edges (for the outside collision tiles)
            var topLeft = new Vector2(map.TileWidth, map.TileWidth);
            var bottomRight = new Vector2(map.TileWidth * (map.Width), map.TileWidth * (map.Height ));
            tiledEntity.AddComponent(new CameraBounds(topLeft, bottomRight));


            var playerEntity = InitPlayer();

            // add a component to have the Camera follow the player

            Camera.ZoomIn(0.5f);
            
            Camera.Entity.AddComponent(new FollowCamera(playerEntity));

        }

        private Entity InitPlayer()
        {
            //Core.DebugRenderEnabled = true;
            

            var collisionRadius = 7.0f;
            var playerEntity = CreateEntity("player", new Vector2(16 * 20, 16 * 116));
            //var playerEntity = CreateEntity("player", new Vector2(16 * 20, 16 * 50));
            playerEntity.AddComponent(new LinkComponent());
            var collider = playerEntity.AddComponent<CircleCollider>();
            collider.SetRadius(collisionRadius);

            // we only want to collide with the tilemap, which is on the default layer 0
            Flags.SetFlagExclusive(ref collider.CollidesWithLayers, 0);

            // move ourself to layer 1 so that we dont get hit by the projectiles that we fire
            Flags.SetFlagExclusive(ref collider.PhysicsLayer, 1);

            return playerEntity;

            
        }
    }
}
