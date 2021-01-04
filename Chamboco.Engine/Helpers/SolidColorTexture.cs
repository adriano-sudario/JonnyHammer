using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chamboco.Engine.Helpers
{
    public class SolidColorTexture
    {

        Color color;

        Texture2D texture;
        public Color Color
        {
            get => color;
            set
            {
                texture.SetData(new[] { value });
                color = value;
            }
        }

        public float Opacity { get; set; } = 1;

        public SolidColorTexture(Color color)
        {
            texture = new Texture2D(Core.Instance.GraphicsDevice, 1, 1);
            Color = color;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle bounds) =>
            spriteBatch.Draw(texture, bounds, Color.White * Opacity);
    }
}
