using Microsoft.Xna.Framework.Graphics;
using Nez.Sprites;
using Nez.Textures;
using System.Linq;

namespace LTTP.Scenes.SimpleMap.Link.Sprites
{
    public class LinkSpriteAtlasFactory : ILinkSpriteAtlasFactory
    {
        private Texture2D texture;
        private int frameRate = 10;

        public LinkSpriteAtlasFactory(Texture2D texture)
        {
            this.texture = texture;
        }

        public SpriteAtlas GetSpriteAtlas()
        {
            var namedAnimations = new NamedSpriteAnimation[]
            {
                BodyWalkRight,
                BodyWalkDown,
                BodyWalkUp
            };

            var namedSprites = new NamedSprite[]
            {
                BodyStandUp,
                BodyStandRight,
                BodyStandDown,
            };

            return new SpriteAtlas()
            {
                AnimationNames = namedAnimations.Select(x => x.Name).ToArray(),
                SpriteAnimations = namedAnimations.Select(x => x.SpriteAnimation).ToArray(),

                Names = namedSprites.Select(x => x.Name).ToArray(),
                Sprites = namedSprites.Select(x => x.Sprite).ToArray()
            };
        }

        public int DefaultWidth = 16;
        public int DefaultHeight = 24;
        public int Row0Y = 3;
        public int Row1Y = 32;
        public int Row2Y = 58;
        public int Row3Y = 85;
        public int Row4Y = 111;


        public int StartX = 1;

        public NamedSprite BodyStandDown => new NamedSprite("BodyStandDown",
            new Sprite(texture, StartX, Row0Y, DefaultWidth, DefaultHeight));

        public NamedSprite BodyStandRight => new NamedSprite("BodyStandRight",
            new Sprite(texture, StartX, Row2Y, DefaultWidth, DefaultHeight));

        public NamedSprite BodyStandUp => new NamedSprite("BodyStandUp",
            new Sprite(texture, StartX, Row4Y, DefaultWidth, DefaultHeight));

        public NamedSpriteAnimation BodyWalkDown => new NamedSpriteAnimation("BodyWalkDown",
          new SpriteAnimation(
              new Sprite[]
              {
                    new Sprite(texture, 19, Row0Y, DefaultWidth, DefaultHeight),
                    new Sprite(texture, 36, Row0Y-1, DefaultWidth, DefaultHeight+1),
                    new Sprite(texture, 53, Row0Y-2, DefaultWidth, DefaultHeight+2),
                    new Sprite(texture, 70, Row0Y, DefaultWidth, DefaultHeight),
                    new Sprite(texture, 87, Row0Y, DefaultWidth, DefaultHeight),
                    new Sprite(texture, 104, Row0Y-1, DefaultWidth, DefaultHeight+1),
                    new Sprite(texture, 121, Row0Y-2, DefaultWidth, DefaultHeight+2),
                    new Sprite(texture, 138, Row0Y, DefaultWidth, DefaultHeight),
              },
              frameRate));

        public NamedSpriteAnimation BodyWalkRight => new NamedSpriteAnimation("BodyWalkRight",
         new SpriteAnimation(
             new Sprite[]
             {
                    new Sprite(texture, 20, Row2Y-1, DefaultWidth+1, DefaultHeight+1),
                    new Sprite(texture, 38, Row2Y-1, DefaultWidth, DefaultHeight+1),
                    new Sprite(texture, 55, Row2Y, DefaultWidth+1, DefaultHeight),
                    new Sprite(texture, 73, Row2Y, DefaultWidth+1, DefaultHeight),
                    new Sprite(texture, 91, Row2Y-1, DefaultWidth+1, DefaultHeight+1),
                    new Sprite(texture, 109, Row2Y-1, DefaultWidth, DefaultHeight+1),
                    new Sprite(texture, 126, Row2Y, DefaultWidth, DefaultHeight),
                    new Sprite(texture, 144, Row2Y, DefaultWidth+1, DefaultHeight),

             },
             frameRate));

        public NamedSpriteAnimation BodyWalkUp => new NamedSpriteAnimation("BodyWalkUp",
            new SpriteAnimation(
           new Sprite[]
           {
                new Sprite(texture, 19, Row4Y, DefaultWidth, DefaultHeight),
                new Sprite(texture, 36, Row4Y-1, DefaultWidth, DefaultHeight+1),
                new Sprite(texture, 53, Row4Y-2, DefaultWidth, DefaultHeight+2),
                new Sprite(texture, 70, Row4Y, DefaultWidth, DefaultHeight),
                new Sprite(texture, 87, Row4Y, DefaultWidth, DefaultHeight),
                new Sprite(texture, 104, Row4Y-1, DefaultWidth, DefaultHeight+1),
                new Sprite(texture, 121, Row4Y-2, DefaultWidth, DefaultHeight+2),
                new Sprite(texture, 138, Row4Y, DefaultWidth, DefaultHeight),
           },
           frameRate));
    }
}