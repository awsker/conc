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
            row.Size = new Vector2(Size.X - 20, 40);
            row.Position = new Vector2(10, 10 + _rows.Count * row.Size.Y + _rows.Count * 4);
            _rows.Add(row);
            AddChild(row);
        }

        public bool HasFocus => _rows.Any(x => x.Activated);

        public override void Update(GameTime gameTime)
        {
            GameDebug.Log("MP", Mouse.GetState().Position);

            foreach (var row in _rows)
            {
                row.Update(gameTime);

                if (row is KeybindRow keybindRow)
                {
                    var nextKey = _inputManager.GetNextKeyPress();
                    var nextMouseKey = _inputManager.GetNextMouseKeyPress();

                    if (nextKey == Keys.Delete && keybindRow.Key1Highlighted)
                    {
                        keybindRow.SetKey1(string.Empty);
                        ExecuteCommand?.Invoke(new Command(keybindRow.ControlButton, keybindRow.Key1Text, keybindRow.Key2Text));
                        return;
                    }

                    if (nextKey == Keys.Delete && keybindRow.Key2Highlighted)
                    {
                        keybindRow.SetKey2(string.Empty);
                        ExecuteCommand?.Invoke(new Command(keybindRow.ControlButton, keybindRow.Key1Text, keybindRow.Key2Text));
                        return;
                    }

                    if (!keybindRow.Activated)
                    {
                        if (_inputManager.IsMouseDownOverBounds(keybindRow.Key1Bounds, 0))
                        {
                            keybindRow.Key1Activated = true;
                            keybindRow.SetKey1Text("press key");
                            _currentRow = keybindRow;
                            return;
                        }
                        if (_inputManager.IsMouseDownOverBounds(keybindRow.Key2Bounds, 0))
                        {
                            keybindRow.Key2Activated = true;
                            keybindRow.SetKey2Text("press key");
                            _currentRow = keybindRow;
                            return;
                        }
                    }

                    if (keybindRow.Activated)
                    {
                        if (nextKey == Keys.Escape)
                        {
                            keybindRow.Key1Activated = false;
                            keybindRow.Key2Activated = false;
                            return;
                        }

                        if (keybindRow.Activated && (nextKey != Keys.None || nextMouseKey != MouseKeys.None))
                        {
                            var keyText = string.Empty;
                            if (nextKey != Keys.None)
                                keyText = nextKey.ToString();
                            if (nextMouseKey != MouseKeys.None)
                                keyText = nextMouseKey.ToString();

                            if (keybindRow.Key1Activated)
                                keybindRow.SetKey1(keyText);
                            else if (keybindRow.Key2Activated)
                                keybindRow.SetKey2(keyText);

                            keybindRow.Key1Activated = false;
                            keybindRow.Key2Activated = false;

                            ExecuteCommand?.Invoke(new Command(keybindRow.ControlButton, keybindRow.Key1Text, keybindRow.Key2Text));
                        }
                    }
                }
            }
        }
    }
}