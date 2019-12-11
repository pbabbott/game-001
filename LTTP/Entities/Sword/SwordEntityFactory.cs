using LTTP.Entities.Core;
using LTTP.Sprites;
using Microsoft.Xna.Framework;
using Nez;

namespace LTTP.Entities.Sword
{
    public class SwordEntityFactory : IEntityFactory
    {
        protected Scene _scene;
        protected LinkSpriteAtlasFactory _linkSpriteAtlasFactory;

        public string EntityName => "sword";

        public SwordEntityFactory(Scene scene, LinkSpriteAtlasFactory linkSpriteAtlasFactory)
        {
            _scene = scene;
            _linkSpriteAtlasFactory = linkSpriteAtlasFactory;
        }

        public Entity CreateEntity(Vector2 startingPosition)
        {
            var swordSpriteAtlas = _linkSpriteAtlasFactory.GetSwordSpriteAtlas();

            var result = _scene.CreateEntity(EntityName);

            var swordAnimation = _scene.CreateEntity("swordAnimation");
            swordAnimation.SetParent(result);
            var swordComponent = new SwordComponent(swordSpriteAtlas);
            swordAnimation.AddComponent(swordComponent);

            var swordCollider = _scene.CreateEntity("swordCollider");
            swordCollider.SetParent(result);
            swordCollider.AddComponent(new SwordCollider(swordComponent));

            return result;
        }
    }
}