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

        public Command(ControlButtons controlButton, string key1, string key2)
        {
            ControlButton = controlButton;
            Key1 = key1;
            Key2 = key2;
            CommandType = CommandType.SetKey;
        }
        
        public CommandType CommandType { get; }
        public SceneType SceneType { get; }

        public ControlButtons ControlButton { get; }
        public string Key1 { get; }
        public string Key2 { get; }
    }
}