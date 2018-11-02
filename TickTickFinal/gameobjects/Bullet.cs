using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Bullet : SpriteGameObject
{

     public Bullet(int layer = 1, string id = "") : base("Sprites/spr_water", layer, id)
     {
     }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (!visible || sprite == null)
        {
            return;
        }
        sprite.Draw(spriteBatch, position, origin);
    }

    public override void Reset()
    {
        visible = false;
        velocity = Vector2.Zero;
    }

    public void Shoot(Vector2 position, bool moveToLeft)
    {
        if(visible)
        {
            return;
        }
        visible = true;
        velocity.X = 450;
        this.position = position;
        this.position.Y -= 70;
        Mirror = moveToLeft;
        if(Mirror)
        {
            velocity.X = -450;
        }
    }

    public void CheckCollision()
    {
        GameObjectList enemies = GameWorld.Find("enemies") as GameObjectList;
        foreach (GameObject e in enemies.Children)
        {
            if (e.Visible)
            {
                if(CollidesWith(e as SpriteGameObject))
                {
                    e.Reset();
                    this.visible = false;
                    e.Visible = false;
                }
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (visible)
        {
            CheckCollision();
        }
        Rectangle screenBox = new Rectangle(0, 0, GameEnvironment.Screen.X, GameEnvironment.Screen.Y);
        if (!screenBox.Intersects(this.BoundingBox))
        {
            Reset();
        }
    }

}

