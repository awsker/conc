using System.IO;
using Microsoft.Xna.Framework.Input;

namespace conc.game.util
{
    public interface IConfigManager
    {
        void WriteDefaultConfig();
    }

    public class ConfigManager : IConfigManager
    {
        private readonly string _fileName;

        public ConfigManager()
        {
            _fileName = "config.cfg";
        }

        public void WriteDefaultConfig()
        {
            if (File.Exists(_fileName))
                return;

            using (var file = new StreamWriter(_fileName))
            {
                file.WriteLine("Left = " + Keys.A);
                file.WriteLine("Right = " + Keys.D);
                file.WriteLine("Jump = " + Keys.LeftShift);
                file.WriteLine("Fire = " + Keys.Space);
            }
        }
    }
}