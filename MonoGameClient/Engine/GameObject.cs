using Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public abstract class GameObject : IGameLoopObject
{
    protected GameObject parent;
    protected Vector2 position, velocity;
    protected int layer;
    protected string id;
    protected bool visible;

    private Dictionary<Type, Component> components = new Dictionary<Type, Component>();

    public GameObject(int layer = 0, string id = "")
    {
        this.layer = layer;
        this.id = id;
        position = Vector2.Zero;
        velocity = Vector2.Zero;
        visible = true;
    }

    public virtual void HandleInput(InputHelper inputHelper)
    {
        foreach (var comp in components.Values)
        {
            comp.HandleInput(inputHelper);
        }
    }

    public virtual void Update(GameTime gameTime)
    {
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (delta > 1f / 20f)
        {
            delta = 1f / 60f;
        }
        position += velocity * delta;
        foreach (var comp in components.Values)
        {
            comp.Update(gameTime);
        }
    }

    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (var comp in components.Values)
        {
            comp.Draw(gameTime, spriteBatch);
        }
    }

    public virtual void Reset()
    {
        visible = true;
        foreach (var comp in components.Values)
        {
            comp.Reset();
        }
    }
    /// <summary>
    /// Gets called when GameObject is colliding this frame but not previous frame.
    /// </summary>
    public virtual void OnCollisionEnter(GameObject other)
    {
    }
    /// <summary>
    /// Gets called when GameObject is colliding this frame and previous frame.
    /// <para>Can get called in the same frame as OnCollisionEnter.</para>
    /// </summary>
    public virtual void OnCollisionStay(GameObject other)
    {
    }
    /// <summary>
    /// Gets called when GameObject is not colliding this frame but was previous frame.
    /// </summary>
    public virtual void OnCollisionExit(GameObject other)
    {
    }

    public T AddComponent<T>(T component) where T : Component
    {
        if (!HasComponent<T>())
        {
            components.Add(typeof(T), component);
            return component;
        }
        return null;
    }

    public bool HasComponent<T>() where T : Component
    {
        return components.ContainsKey(typeof(T));
    }

    public T GetComponent<T>() where T : Component
    {
        if (HasComponent<T>())
        {
            return components[typeof(T)] as T;
        }
        return null;
    }

    public bool GetComponent<T>(out T component) where T : Component
    {
        if (HasComponent<T>())
        {
            component = GetComponent<T>();
            return true;
        }
        component = null;
        return false;
    }

    public virtual Vector2 Position
    {
        get { return position; }
        set { position = value; }
    }

    public virtual Vector2 Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }

    public virtual Vector2 GlobalPosition
    {
        get
        {
            if (parent != null)
            {
                return parent.GlobalPosition + Position;
            }
            else
            {
                return Position;
            }
        }
    }

    public GameObject Root
    {
        get
        {
            if (parent != null)
            {
                return parent.Root;
            }
            else
            {
                return this;
            }
        }
    }

    public GameObjectList GameWorld
    {
        get
        {
            return Root as GameObjectList;
        }
    }

    public virtual int Layer
    {
        get { return layer; }
        set { layer = value; }
    }

    public virtual GameObject Parent
    {
        get { return parent; }
        set { parent = value; }
    }

    public string Id
    {
        get { return id; }
    }

    public bool Visible
    {
        get { return visible; }
        set { visible = value; }
    }

    public virtual Rectangle BoundingBox
    {
        get
        {
            return new Rectangle((int)GlobalPosition.X, (int)GlobalPosition.Y, 0, 0);
        }
    }

    public bool CollidesWith(GameObject obj)
    {
        if (!Visible || !obj.Visible)
        {
            return false;
        }
        if (this == obj)
        {
            return false;
        }

        Rectangle bbSelf = BoundingBox;
        Rectangle bbObj = obj.BoundingBox;

        if (GetComponent<ColliderComponent>(out var collider))
        {
            bbSelf = collider.BoundingBox;
        }
        if (obj.GetComponent<ColliderComponent>(out var objCollider))
        {
            bbObj = objCollider.BoundingBox;
        }

        if (bbSelf.Intersects(bbObj))
        {
            if(Id == "1" || obj.Id == "1")
            {

            }
            return true;
        }
        return false;
    }
}