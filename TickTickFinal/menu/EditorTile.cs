using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class EditorTile : SpriteGameObject
{
    protected bool pressed;
    public string tileType;

    public EditorTile(string imageAsset, int layer = 0, string id = "")
        : base(imageAsset, layer, id)
    {
        tileType = id;
        pressed = false;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        pressed = inputHelper.MouseLeftButtonPressed() &&
            BoundingBox.Contains((int)inputHelper.MousePosition.X + GameEnvironment.Camera.offset.X, (int)inputHelper.MousePosition.Y + GameEnvironment.Camera.offset.Y);
    }

    public override void Reset()
    {
        base.Reset();
        pressed = false;
    }

    public bool Pressed
    {
        get { return pressed; }
    }
}
