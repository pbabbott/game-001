using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTTP.Sprites
{
    public class NamedSpriteAnimation
    {
        public string Name { get; set; }
        public SpriteAnimation SpriteAnimation { get; set; }

        public NamedSpriteAnimation(string name, SpriteAnimation spriteAnimation)
        {
            Name = name;
            SpriteAnimation = spriteAnimation;
        }
    }
}
