using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

public class EditorState : GameObjectList
{
    Button backButton;
    List<EditorTile> editorTiles = new List<EditorTile>();

    const int tileWidth = 72;
    const int tileHeight = 55;
    int levelWidth;
    int levelHeight;

    char[] availableTiles = ".-+@XW1#^*TRrSABC".ToCharArray();
    int selectorIndex = 0;
    EditorSelector selector;

    public EditorState()
    {
        backButton = new Button("Sprites/spr_button_back", 1);
        backButton.Position = new Vector2((GameEnvironment.Screen.X - backButton.Width) / 2, 750);
        Add(backButton);

        selector = new EditorSelector("Sprites/spr_empty");
        Add(selector);

        List<string> textLines = new List<string>();
        StreamReader fileReader = new StreamReader("Content/Levels/12.txt");
        string redundantTimeLine = fileReader.ReadLine(); // As we do not need the time in the level editor, but we do need to skip this line
        string line = fileReader.ReadLine();
        int width = line.Length;
        while (line != null)
        {
            textLines.Add(line);
            line = fileReader.ReadLine();
        }
        int height = textLines.Count - 1;

        
        levelWidth = width * tileWidth;
        levelHeight = height * tileHeight;

        for (int y = 0; y < 25; y++)
        {
            for(int x = 0; x < 25; x++)
            {
                EditorTile editorTile = new EditorTile(LoadTile(textLines[y][x]), 0, textLines[y][x].ToString());
                editorTile.Position = new Vector2(x * tileWidth, y * tileHeight);
                editorTiles.Add(editorTile);
                Add(editorTiles[editorTiles.Count - 1]);
            }
        }
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        const int cameraMovementSpeed = 3;

        base.HandleInput(inputHelper);

        if (backButton.Pressed)
        {
            SaveLevel();
            GameEnvironment.GameStateManager.SwitchTo("titleMenu");
        }

        if (inputHelper.KeyPressed(Keys.D1))
        {
            selectorIndex--;
            if (selectorIndex < 0)
                selectorIndex = 16;
            Remove(selector);
            selector = new EditorSelector(LoadTile(availableTiles[selectorIndex]));
            Add(selector);
        }
        if (inputHelper.KeyPressed(Keys.D2))
        {
            selectorIndex++;
            if (selectorIndex > 16)
                selectorIndex = 0;
            Remove(selector);
            selector = new EditorSelector(LoadTile(availableTiles[selectorIndex]));
            Add(selector);
        }

        if (inputHelper.IsKeyDown(Keys.Left))
        {
            if(GameEnvironment.Camera.offset.X < -tileWidth)
            {
                GameEnvironment.Camera.offset.X = -tileWidth;
            }
            else
            {
                GameEnvironment.Camera.offset.X -= cameraMovementSpeed;
            }
        }
        if (inputHelper.IsKeyDown(Keys.Right))
        {
            if (GameEnvironment.Camera.offset.X > levelWidth - GameEnvironment.Screen.X + tileWidth)
            {
                GameEnvironment.Camera.offset.X = levelWidth - GameEnvironment.Screen.X + tileWidth;
            }
            else
            {
                GameEnvironment.Camera.offset.X += cameraMovementSpeed;
            }
        }
        if (inputHelper.IsKeyDown(Keys.Up))
        {
            if (GameEnvironment.Camera.offset.Y < -tileHeight)
            {
                GameEnvironment.Camera.offset.Y = -tileHeight;
            }
            else
            {
                GameEnvironment.Camera.offset.Y -= cameraMovementSpeed;
            }
        }
        if (inputHelper.IsKeyDown(Keys.Down))
        {
            if (GameEnvironment.Camera.offset.Y > levelHeight - GameEnvironment.Screen.Y + tileHeight)
            {
                GameEnvironment.Camera.offset.Y = levelHeight - GameEnvironment.Screen.Y + tileHeight;
            }
            else
            {
                GameEnvironment.Camera.offset.Y += cameraMovementSpeed;
            }
        }

        int pressedTile = -1;
        foreach(EditorTile tile in editorTiles)
        {
            if (tile.Pressed)
            {
                pressedTile = (int)(tile.Position.X / tileWidth + (tile.Position.Y / tileHeight) * 25);
            }
        }
        if(pressedTile >= 0)
        {
            Remove(editorTiles[pressedTile]);
            editorTiles[pressedTile] = new EditorTile(LoadTile(availableTiles[selectorIndex]), 0, availableTiles[selectorIndex].ToString());
            editorTiles[pressedTile].Position = new Vector2((pressedTile % 25) * tileWidth, ((pressedTile - (pressedTile % 25)) / 25) * tileHeight);
            Add(editorTiles[pressedTile]);
        }
    }

    private string LoadTile(char tileType)
    {
        switch (tileType)
        {
            case '.':
                return "Sprites/spr_empty";
            case '-':
                return "Tiles/spr_platform";
            case '+':
                return "Tiles/spr_platform_hot";
            case '@':
                return "Tiles/spr_platform_ice";
            case 'X':
                return "Sprites/spr_goal";
            case 'W':
                return "Sprites/spr_water";
            case '1':
                return "Sprites/Player/spr_idle";
            case '#':
                return "Tiles/spr_wall";
            case '^':
                return "Tiles/spr_wall_hot";
            case '*':
                return "Tiles/spr_wall_ice";
            case 'T':
                return "Sprites/Turtle/spr_idle";
            case 'R':
                return "Sprites/Rocket/spr_idle_left";
            case 'r':
                return "Sprites/Rocket/spr_idle";
            case 'S':
                return "Sprites/Sparky/spr_idle";
            case 'A':
            case 'B':
            case 'C':
                return "Sprites/Flame/spr_idle";
            default:
                return "Sprites/spr_empty";
        }
    }

    private void SaveLevel()
    {
        List<string> writeLines = new List<string>();
        for(int y = 0; y < 25; y++)
        {
            string currentLine = "";
            for(int x = 0; x < 25; x++)
            {
                currentLine += editorTiles[x + y * 25].tileType;
            }
            writeLines.Add(currentLine);
        }
        StreamWriter writer = new StreamWriter("Content/Levels/12.txt", false);
        writer.WriteLine("1");
        foreach(string writeLine in writeLines)
        {
            Console.WriteLine(writeLine);
            writer.WriteLine(writeLine);
        }
        writer.WriteLine("Edit this level in the level editor!");
        writer.Close();
    }
}
