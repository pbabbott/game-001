﻿using LTTP.Scenes.SimpleMap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;


namespace LTTP
{
    public class Game1 : Core
    {
        protected override void Initialize()
        {
            base.Initialize();

            Window.AllowUserResizing = true;
            Scene = new SimpleMapScene();
        }
    }
}
