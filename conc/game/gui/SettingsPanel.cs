using System.Collections.Generic;
using System.Linq;
using conc.game.gui.components;
using conc.game.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.gui
{
    public interface ISettingsPanel : IPanel
    {
        void AddRow(ISettingsPanelRow row);
        bool HasFocus { get; }
    }

    public abstract class SettingsPanel : Panel, ISettingsPanel
    {
        protected readonly IList<ISettingsPanelRow> _rows;
        private SpriteFont _font;

        protected SettingsPanel(InputManager inputManager, SpriteFont font) : base(inputManager)
        {
            _font = font;
            _rows = new List<ISettingsPanelRow>();
            BackgroundColor = new Color(105, 143, 224);
        }

        public void AddRow(ISettingsPanelRow row)
        {
            row.Size = new Vector2(Size.X - 20, 40);
            row.Position = new Vector2(10, 10 + _rows.Count * row.Size.Y + _rows.Count * 4);
            _rows.Add(row);
            AddChild(row);
        }

        public bool HasFocus => _rows.Any(x => x.Activated);

        public override void Update(GameTime gameTime)
        {
            foreach (var row in _rows)
                row.Update(gameTime);
        }
    }
}