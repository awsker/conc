using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace conc.game.util
{
    public class VideoModeHelper
    {
        private static VideoModeHelper _instance;
        private GraphicsDeviceManager _manager;

        public static VideoModeHelper Instance => _instance;

        public static void Initialize(GraphicsDeviceManager manager)
        {
            _instance = new VideoModeHelper(manager);
        }
        private VideoModeHelper(GraphicsDeviceManager manager)
        {
            _manager = manager;
        }

        public void SetFullscreen(bool fullscreen)
        {
            if (_manager.IsFullScreen != fullscreen)
                _manager.ToggleFullScreen();            
        }

        public IEnumerable<DisplayMode> AvailableDisplayModes
        {
            
            get
            {
                /*
                var resolutionDict = new Dictionary<string, DisplayMode>();
                var modes = _manager.GraphicsDevice.Adapter.SupportedDisplayModes;
                foreach(var mode in modes)
                {
                    DisplayMode prev;
                    if(resolutionDict.TryGetValue(keyFromDisplayMode(mode), out prev))
                    {

                    }
                }*/
                return null;
            }
        }

        private string keyFromDisplayMode(DisplayMode mode)
        {
            return mode.Width + ":" + mode.Height;
        }

        public void SetDisplayMode(DisplayMode mode)
        {

        }
    }
}
