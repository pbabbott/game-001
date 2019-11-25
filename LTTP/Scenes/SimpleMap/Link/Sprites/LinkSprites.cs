using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez.Sprites;
using Nez.Textures;
using System;
using System.Linq;

namespace LTTP.Scenes.SimpleMap.Link.Sprites
{
    

    public class LinkSprites : ILinkSpriteAtlasFactory
    {
        public LinkColor LinkColor = LinkColor.Green;
        private float frameRate = 5;
        private readonly Texture2D texture;

        public LinkSprites(Texture2D texture)
        {
            this.texture = texture;
        }

        private Point StartingPoint => GetStartingPoint();
        private Point GreenStartingPoint = new Point(16, 16);

        public Point GetStartingPoint()
        {
            if (LinkColor == LinkColor.Green)
                return GreenStartingPoint;
            else
                throw new NotImplementedException();
        }

        private float defaultSize = 16;

        private int GetOffset(float n, float? size = null)
        {
            if (size == null)
            {
                size = defaultSize;
            }
            return Convert.ToInt32(defaultSize * n);
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

        public NamedSprite BodyStandRight => new NamedSprite("BodyStandRight",
            new Sprite(texture, StartingPoint.X + GetOffset(0), StartingPoint.Y + GetOffset(1), defaultSize, defaultSize));

        public NamedSpriteAnimation BodyWalkRight => new NamedSpriteAnimation(
            "BodyWalkRight",
            new SpriteAnimation(
                new Sprite[]
                {
                    BodyStandRight.Sprite,
                    new Sprite(texture, StartingPoint.X + GetOffset(1), StartingPoint.Y + GetOffset(1), defaultSize, defaultSize),
                    new Sprite(texture, StartingPoint.X + GetOffset(2), StartingPoint.Y + GetOffset(1), defaultSize, defaultSize),
                },
                frameRate));

        public NamedSprite BodyStandDown => new NamedSprite("BodyStandDown",
            new Sprite(texture, StartingPoint.X + GetOffset(3), StartingPoint.Y + GetOffset(1), defaultSize, defaultSize));

        public NamedSpriteAnimation BodyWalkDown => new NamedSpriteAnimation(
           "BodyWalkDown",
           new SpriteAnimation(
               new Sprite[]
               {
                    BodyStandDown.Sprite,
                    new Sprite(texture, StartingPoint.X + GetOffset(4), StartingPoint.Y + GetOffset(1), defaultSize, defaultSize),
                    new Sprite(texture, StartingPoint.X + GetOffset(5), StartingPoint.Y + GetOffset(1), defaultSize, defaultSize),
               },
               frameRate));

        public NamedSprite BodyStandUp => new NamedSprite("BodyStandUp",
          new Sprite(texture, StartingPoint.X + GetOffset(1), StartingPoint.Y + GetOffset(2), defaultSize, defaultSize));

        public NamedSpriteAnimation BodyWalkUp => new NamedSpriteAnimation(
          "BodyWalkUp",
          new SpriteAnimation(
              new Sprite[]
              {
                    new Sprite(texture, StartingPoint.X + GetOffset(6), StartingPoint.Y + GetOffset(1), defaultSize, defaultSize),
                    new Sprite(texture, StartingPoint.X + GetOffset(0), StartingPoint.Y + GetOffset(2), defaultSize, defaultSize),
                    BodyStandUp.Sprite,
              },
              frameRate));
    }
}