using System.Linq;

using Nez;
using Nez.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Nez.PhysicsShapes;
using LTTP.Sprites;
using Nez.Tiled;
using LTTP.Entities.Link;
using LTTP.Entities.Sword;

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
            var tiledEntity = CreateEntity("tiled-map");
            var map = Content.LoadTiledMap("Content/maps/sample_map.tmx");
            
            var tiledMapRenderer = tiledEntity.AddComponent(new TiledMapRenderer(map, "collision"));
            tiledMapRenderer.SetLayersToRender(new[] { "bg", "terrain", "details" });
            tiledMapRenderer.RenderLayer = 10;

            // render our above-details layer after the player so the player is occluded by it when walking behind things
            var tiledMapDetailsComp = tiledEntity.AddComponent(new TiledMapRenderer(map));
            tiledMapDetailsComp.SetLayerToRender("fg details");
            tiledMapDetailsComp.RenderLayer = -10;
            
            // the details layer will write to the stencil buffer so we can draw a shadow when the player is behind it. we need an AlphaTestEffect
            // here as well
            tiledMapDetailsComp.Material = Material.StencilWrite();
            tiledMapDetailsComp.Material.Effect = Content.LoadNezEffect<SpriteAlphaTestEffect>();

            // setup our camera bounds with a 1 tile border around the edges (for the outside collision tiles)
            var topLeft = new Vector2(map.TileWidth, map.TileWidth);
            var bottomRight = new Vector2(map.TileWidth * (map.Width), map.TileWidth * (map.Height ));
            tiledEntity.AddComponent(new CameraBounds(topLeft, bottomRight));

            var texture = Content.Load<Texture2D>(Nez.Content.Characters.Link2);
            var linkSpriteAtlasFactory = new LinkSpriteAtlasFactory(texture);


            Core.DebugRenderEnabled = true;


            // Create Link
            var linkEntityFactory = new LinkEntityFactory(this, linkSpriteAtlasFactory);
            var startingPosition = new Vector2(430, 1580);
            var playerEntity = linkEntityFactory.CreateEntity(startingPosition);

            // Create a sword
            var swordEntityFactory = new SwordEntityFactory(this, linkSpriteAtlasFactory);
            var offscreen = new Vector2(-100, -100);
            swordEntityFactory.CreateEntity(offscreen);


            // add a component to have the Camera follow the player
            //Camera.ZoomIn(0.75f);
            Camera.Entity.AddComponent(new FollowCamera(playerEntity));

        }

        
    }
}
