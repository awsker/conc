using System;
using System.Collections.Generic;
using System.Linq;
using conc.game.commands;
using conc.game.gui.components;
using conc.game.input;
using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace conc.game.gui
{
    public interface ISettingsPanel : IPanel
    {
        event Action<Command> ExecuteCommand;
        void AddRow(ISettingsPanelRow row);
        bool HasFocus { get; }
    }

    public abstract class SettingsPanel : Panel, ISettingsPanel
    {
        public event Action<Command> ExecuteCommand;

        protected readonly IList<ISettingsPanelRow> _rows;
        protected readonly InputManager _inputManager;
        protected SpriteFont _font;
        private ISettingsPanelRow _currentRow;

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

        public bool HasFocus => _rows.Any(x => x.Activated);

        public override void Update(GameTime gameTime)
        {
            foreach (var row in _rows)
            {
                row.Update(gameTime);
                if (_inputManager.IsMouseDownOverBounds(row.FocusBounds, 0))
                {
                    _currentRow?.Deactivate();

                    row.Activate();
                    _currentRow = row;
                }

                var nextKey = _inputManager.GetNextKeyPress();

                if (row is KeybindRow keybindRow)
                {
                    if (keybindRow.Activated)
                    {
                        if (nextKey == Keys.Escape)
                        {
                            keybindRow.Deactivate();
                            return;
                        }

                        keybindRow.SetText("press key");

                        if (keybindRow.Activated && nextKey != Keys.None)
                        {
                            keybindRow.SetKey(nextKey);
                            keybindRow.Deactivate();

                            ExecuteCommand?.Invoke(new Command(keybindRow.ControlButton, nextKey));
                        }
                    }
                }
            }
        }
    }
}