using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class Camera
{
    private float zoom = 1.0f;
    private Vector2 position = Vector2.Zero;
    private float rotation = 0.0f;
    private bool locked = false;

    public Camera()
    {
    }

    public void Reset()
    {
        position = Vector2.Zero;
        rotation = 0.0f;
        zoom = 1.0f;
    }

    public float Zoom
    {
        get { return zoom; }
        set { if (!locked) { zoom = value; if (zoom < 0.1f) zoom = 0.1f; } }
    }

    public float Rotation
    {
        get { return rotation; }
        set { if (!locked) { rotation = value; } }
    }

    public bool Locked
    {
        get { return locked; }
        set { locked = value; }
    }

    public void Move(Vector2 amount)
    {
        position += amount;
    }

    public Vector2 Position
    {
        get { return position; }
        set { if (!Locked) position = value; }
    }

    public Matrix GetTransformation(GraphicsDevice graphicsDevice)
    {
        return Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0)) *
                                     Matrix.CreateRotationZ(Rotation) *
                                     Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                     Matrix.CreateScale(new Vector3(
                                         (float)graphicsDevice.Viewport.Width / GameEnvironment.Screen.X,
                                         (float)graphicsDevice.Viewport.Height / GameEnvironment.Screen.Y, 1)) *
                                     Matrix.CreateTranslation(Vector3.Zero);
    }
}
