using System.Collections.Generic;
using conc.game.gui.components;
using conc.game.input;
using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.gui
{
    public interface ISettingsPanel : IPanel
    {
        void AddRow(ISettingsPanelRow row);
    }

    public abstract class SettingsPanel : Panel, ISettingsPanel
    {
        protected readonly IList<ISettingsPanelRow> _rows;
        protected readonly InputManager _inputManager;
        protected SpriteFont _font;

        protected SettingsPanel(ColorManager colorManager, InputManager inputManger, SpriteFont font) : base(colorManager)
        {
            _inputManager = inputManger;
            _font = font;
            _rows = new List<ISettingsPanelRow>();
            BackgroundColor = new Color(105, 143, 224);
        }

        public void AddRow(ISettingsPanelRow row)
        {
            row.Parent = this;
            row.Size = new Vector2(Size.X - 20, 30);
            row.Position = new Vector2(10, 10 + _rows.Count * row.Size.Y + _rows.Count * 4);
            _rows.Add(row);
            AddChild(row);
        }
    }
}