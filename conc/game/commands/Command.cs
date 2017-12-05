using conc.game.input;
using conc.game.scenes;
using Microsoft.Xna.Framework.Input;

namespace conc.game.commands
{
    public class Command
    {
        public Command()
        {
            CommandType = CommandType.Dummy;
        }

        public Command(SceneType sceneType)
        {
            SceneType = sceneType;
            CommandType = CommandType.ChangeScene;
        }

        public Command(CommandType commandType)
        {
            CommandType = commandType;
        }

        public Command(ControlButtons controlButton, Keys key)
        {
            ControlButton = controlButton;
            Key = key;
            CommandType = CommandType.SetKey;
        }
        
        public CommandType CommandType { get; }
        public SceneType SceneType { get; }

        public ControlButtons ControlButton { get; }
        public Keys Key { get; }
    }
}