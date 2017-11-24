using conc.game.scenes;

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

        public CommandType CommandType { get; }
        public SceneType SceneType { get; }
    }
}