using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using System.Linq;

namespace LTTP.Sprites
{
    public class LinkSpriteAtlasFactory
    {
        public static float frameRate = 15f;
        public static float attackFrameRate = frameRate * 1.5f;

        private Texture2D texture;

        public LinkSpriteAtlasFactory(Texture2D texture)
        {
            this.texture = texture;
        }

        public SpriteAtlas GetSwordSpriteAtlas()
        {
            return CreateSpriteAtlas(
               new NamedSpriteAnimation[]
               {
                    SwordAttack
               },
               new NamedSprite[]
               {
                    SwordHeld
               });

        }

        public SpriteAtlas GetBodySpriteAtlas()
        {
            return CreateSpriteAtlas(
                new NamedSpriteAnimation[]
                {
                    BodyWalkDownEmptyHanded,

                    BodyWalkRight,
                    BodyWalkUp,
                    BodyAttackDown,
                    BodyAttackRight,
                    BodyAttackUp
                },
                new NamedSprite[]
                {
                    BodyStandUp,
                    BodyStandRight,
                    BodyStandDown,
                });
        }

        private static SpriteAtlas CreateSpriteAtlas(NamedSpriteAnimation[] namedAnimations, NamedSprite[] namedSprites)
        {
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

        public int[] RowY = new int[]
        {
            3,
            32,
            58,
            85,
            111,
            141
        };

        public int StartX = 1;

        public static string GetAtlasNameFromDirection(string prefix, Direction d)
        {
            if (d == Direction.Left)
                return prefix + Direction.Right.ToString();

            return prefix + d.ToString();
        }


        public NamedSprite BodyStandDown => new NamedSprite("BodyStandDown",
            new Sprite(texture, StartX, RowY[0], DefaultWidth, DefaultHeight));

        public NamedSprite BodyStandRight => new NamedSprite("BodyStandRight",
            new Sprite(texture, StartX, RowY[2], DefaultWidth, DefaultHeight));

        public NamedSprite BodyStandUp => new NamedSprite("BodyStandUp",
            new Sprite(texture, StartX, RowY[4], DefaultWidth, DefaultHeight));

        public NamedSpriteAnimation BodyWalkDownEmptyHanded => new NamedSpriteAnimation("BodyWalkDown",
          new SpriteAnimation(
              new Sprite[]
              {
                    new Sprite(texture, 19, RowY[0], DefaultWidth, DefaultHeight),
                    new Sprite(texture, 36, RowY[0]-1, DefaultWidth, DefaultHeight+1),
                    new Sprite(texture, 53, RowY[0]-2, DefaultWidth, DefaultHeight+2),
                    new Sprite(texture, 70, RowY[0], DefaultWidth, DefaultHeight),
                    new Sprite(texture, 87, RowY[0], DefaultWidth, DefaultHeight),
                    new Sprite(texture, 104, RowY[0]-1, DefaultWidth, DefaultHeight+1),
                    new Sprite(texture, 121, RowY[0]-2, DefaultWidth, DefaultHeight+2),
                    new Sprite(texture, 138, RowY[0], DefaultWidth, DefaultHeight),
              },
              frameRate));

        public NamedSpriteAnimation BodyWalkRight => new NamedSpriteAnimation("BodyWalkRight",
         new SpriteAnimation(
             new Sprite[]
             {
                    new Sprite(texture, 20, RowY[2]-1, DefaultWidth+1, DefaultHeight+1),
                    new Sprite(texture, 38, RowY[2]-1, DefaultWidth, DefaultHeight+1),
                    new Sprite(texture, 55, RowY[2], DefaultWidth+1, DefaultHeight),
                    new Sprite(texture, 73, RowY[2], DefaultWidth+1, DefaultHeight),
                    new Sprite(texture, 91, RowY[2]-1, DefaultWidth+1, DefaultHeight+1),
                    new Sprite(texture, 109, RowY[2]-1, DefaultWidth, DefaultHeight+1),
                    new Sprite(texture, 126, RowY[2], DefaultWidth, DefaultHeight),
                    new Sprite(texture, 144, RowY[2], DefaultWidth+1, DefaultHeight),
             },
             frameRate));

        public NamedSpriteAnimation BodyWalkUp => new NamedSpriteAnimation("BodyWalkUp",
            new SpriteAnimation(
               new Sprite[]
               {
                    new Sprite(texture, 19, RowY[4], DefaultWidth, DefaultHeight),
                    new Sprite(texture, 36, RowY[4]-1, DefaultWidth, DefaultHeight+1),
                    new Sprite(texture, 53, RowY[4]-2, DefaultWidth, DefaultHeight+2),
                    new Sprite(texture, 70, RowY[4], DefaultWidth, DefaultHeight),
                    new Sprite(texture, 87, RowY[4], DefaultWidth, DefaultHeight),
                    new Sprite(texture, 104, RowY[4]-1, DefaultWidth, DefaultHeight+1),
                    new Sprite(texture, 121, RowY[4]-2, DefaultWidth, DefaultHeight+2),
                    new Sprite(texture, 138, RowY[4], DefaultWidth, DefaultHeight),
               },
               frameRate));

        public NamedSpriteAnimation BodyAttackDown => new NamedSpriteAnimation("BodyAttackDown",
            new SpriteAnimation(
                new Sprite[]
                {
                    new Sprite(texture, StartX, RowY[1], DefaultWidth, DefaultHeight),
                    new Sprite(texture, 18, RowY[1]+1, DefaultWidth, DefaultHeight-1),
                    new Sprite(texture, 35, RowY[1]+2, DefaultWidth, DefaultHeight-2),
                    new Sprite(texture, 52, RowY[1]+5, DefaultWidth, DefaultHeight-5),
                    new Sprite(texture, 69, RowY[1]+3, DefaultWidth, DefaultHeight-3),
                    new Sprite(texture, 86, RowY[1]+2, DefaultWidth, DefaultHeight-2),
                },
                attackFrameRate));

        public NamedSpriteAnimation BodyAttackRight => new NamedSpriteAnimation("BodyAttackRight",
          new SpriteAnimation(
              new Sprite[]
                {
                    new Sprite(texture, StartX, RowY[3], DefaultWidth+1, DefaultHeight-1),
                    new Sprite(texture, 19, RowY[3], DefaultWidth+1, DefaultHeight-1),
                    new Sprite(texture, 37, RowY[3]+1, DefaultWidth+2, DefaultHeight-2),
                    new Sprite(texture, 57, RowY[3]+1, 23, 22),
                    new Sprite(texture, 81, RowY[3]+1, 19, 22),
                    new Sprite(texture, 101, RowY[3], DefaultWidth, DefaultHeight-1),
              },
              attackFrameRate));

        public NamedSpriteAnimation BodyAttackUp => new NamedSpriteAnimation("BodyAttackUp",
            new SpriteAnimation(
                new Sprite[]
                {
                    new Sprite(texture, StartX, RowY[5], DefaultWidth, DefaultHeight),
                    new Sprite(texture, 18, RowY[5]-1, DefaultWidth, DefaultHeight+1),
                    new Sprite(texture, 35, RowY[5]-2, DefaultWidth, DefaultHeight+2),
                    new Sprite(texture, 52, RowY[5]-5, DefaultWidth, DefaultHeight+5),
                    new Sprite(texture, 69, RowY[5]-2, DefaultWidth, DefaultHeight+2),
                    new Sprite(texture, 86, RowY[5], DefaultWidth, DefaultHeight),
                },
                attackFrameRate));

        public NamedSprite SwordHeld => new NamedSprite("SwordHeld",
            new Sprite(texture, StartX, 269, 8, 16));

        public NamedSpriteAnimation SwordAttack => new NamedSpriteAnimation("SwordAttack",
            new SpriteAnimation(
                new Sprite[]
                {
                    new Sprite(texture, 10, 269, 8, 16),
                    new Sprite(texture, 19, 269, 8, 16),
                    new Sprite(texture, 28, 269, 16, 16),
                    new Sprite(texture, 45, 269, 16, 16),
                    new Sprite(texture, 62, 277, 16, 8),
                    new Sprite(texture, 79, 277, 16, 8),
                },
                attackFrameRate));

    }
}