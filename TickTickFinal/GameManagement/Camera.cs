using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


// class to deal with the camera for side scrolling
public class Camera
{
    public Vector2 offset;
    public int currentLevelWidth;
    public int currentLevelHeight;

    public Camera()
    {
        offset = new Vector2(0,0);
    }

    public void getOffset(Vector2 playerPos)
    {
        if(playerPos.X < GameEnvironment.Screen.X/2)
        {
            offset.X = 0;
        }else if (playerPos.X > currentLevelWidth - GameEnvironment.Screen.X / 2)
        {
            offset.X = currentLevelWidth - GameEnvironment.Screen.X;
        }
        else
        {
            offset.X = playerPos.X - GameEnvironment.Screen.X / 2;
        }
        if (playerPos.Y < GameEnvironment.Screen.Y / 2)
        {
            offset.Y = 0;
        }
        else if (playerPos.Y > currentLevelHeight - GameEnvironment.Screen.Y / 2)
        {
            offset.Y = currentLevelHeight - GameEnvironment.Screen.Y;
        }
        else
        {
            offset.Y = playerPos.Y - GameEnvironment.Screen.Y / 2;
        }
    }
}
