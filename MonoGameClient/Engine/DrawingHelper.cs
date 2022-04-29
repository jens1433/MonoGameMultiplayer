using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class DrawingHelper
{
    protected static Texture2D pixel;

    public static void Initialize(GraphicsDevice graphics)
    {
        pixel = new Texture2D(graphics, 1, 1);
        pixel.SetData(new[] { Color.White });
    }

    public static void DrawRectangle(Rectangle r, SpriteBatch spriteBatch, Color col)
    {
        int bw = 2; // Border width

        spriteBatch.Draw(pixel, new Rectangle(r.Left, r.Top, bw, r.Height), col); // Left
        spriteBatch.Draw(pixel, new Rectangle(r.Right, r.Top, bw, r.Height), col); // Right
        spriteBatch.Draw(pixel, new Rectangle(r.Left, r.Top, r.Width, bw), col); // Top
        spriteBatch.Draw(pixel, new Rectangle(r.Left, r.Bottom, r.Width, bw), col); // Bottom
    }

    public static void DrawCross(Point position, int size, SpriteBatch spriteBatch, Color col)
    {
        spriteBatch.Draw(pixel, new Rectangle(position.X - size / 2, position.Y - 1, size, 2), col); // Hor
        spriteBatch.Draw(pixel, new Rectangle(position.X - 1, position.Y - size / 2, 2, size), col); // Vert
    }
}
