using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFMLproj.Game
{
    public class Map
    {
        private Texture texture;
        private Sprite sprite;
        public Map()
        {
            LoadContent();
        }

        public void LoadContent()
        {
            texture = new Texture("./sprites/map.png");
            sprite = new Sprite(texture);
        }

        public void DrawData(GameLoop gameLoop)
        {
            gameLoop.Window.Draw(sprite);
        }
    }
}
