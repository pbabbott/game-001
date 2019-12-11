using LTTP.Entities.Core;
using LTTP.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTTP.Entities.Link
{
    public class LinkEntityFactory : IEntityFactory
    {
        public float CollisionRadius { get; } = 7.0f;
        public string EntityName { get; } = "player";
        protected Scene _scene;
        protected LinkSpriteAtlasFactory _linkSpriteAtlasFactory;

        public LinkEntityFactory(Scene scene, LinkSpriteAtlasFactory linkSpriteAtlasFactory)
        {
            _scene = scene;
            _linkSpriteAtlasFactory = linkSpriteAtlasFactory;
        }

        public Entity CreateEntity(Vector2 startingPosition)
        {
            var result = _scene.CreateEntity(EntityName, startingPosition);

            var bodySpriteAtlas = _linkSpriteAtlasFactory.GetBodySpriteAtlas();
            result.AddComponent(new LinkComponent(bodySpriteAtlas));

            var collider = result.AddComponent<CircleCollider>();
            collider.SetRadius(CollisionRadius);

            // we only want to collide with the tilemap, which is on the default layer 0
            Flags.SetFlagExclusive(ref collider.CollidesWithLayers, 0);

            // move ourself to layer 1 so that we dont get hit by the projectiles that we fire
            Flags.SetFlagExclusive(ref collider.PhysicsLayer, 1);

            return result;
        }

       
    }
}
