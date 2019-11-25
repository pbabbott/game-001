using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTTP.Scenes.SimpleMap.Link
{
    public class NamedSprite
    {
        public NamedSprite(string name, Sprite sprite)
        {
            Name = name;
            Sprite = sprite;
        }

        public string Name { get; set; }
        public Sprite Sprite{ get; set; }

       
    }
}
