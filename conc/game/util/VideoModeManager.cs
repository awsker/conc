using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace conc.game.util
{
    public class VideoModeManager
    {
        private GraphicsDeviceManager _manager;
        private GameWindow _window;
        private Point _positionBeforeFullscreen;
        private int _widthBeforeFullscreen;
        private int _heightBeforeFullscreen;

        public GraphicsDeviceManager GraphicsDeviceManager => _manager;
        public VideoModeManager(GraphicsDeviceManager manager, GameWindow window)
        {
            _manager = manager;
            _window = window;
        }

        public void ToggleFullscreen()
        {
            SetFullscreen(!IsFullscreen);
        }

        public void SetFullscreen(bool fullscreen)
        {
            if (IsFullscreen != fullscreen)
            {
                if (fullscreen)
                    enableFullscreen();
                else
                    disableFullscreen();
            }   
        }

        public bool IsFullscreen
        {
            get
            {
                return _window.IsBorderless;
            }
        }

        private void enableFullscreen()
        {
            //Store old values
            _positionBeforeFullscreen = _window.Position;
            _widthBeforeFullscreen = _manager.PreferredBackBufferWidth;
            _heightBeforeFullscreen = _manager.PreferredBackBufferHeight;

            _window.IsBorderless = true;
            _window.Position = new Point(0, 0);

            var width = _manager.GraphicsDevice.DisplayMode.Width; 
            var height = _manager.GraphicsDevice.DisplayMode.Width;

            _manager.PreferredBackBufferWidth = width;
            _manager.PreferredBackBufferHeight = height;

            _manager.ApplyChanges();
        }

        private void disableFullscreen()
        {
            _window.IsBorderless = false;
            _window.Position = _positionBeforeFullscreen;

            _manager.PreferredBackBufferWidth = _widthBeforeFullscreen;
            _manager.PreferredBackBufferHeight = _heightBeforeFullscreen;

            _manager.ApplyChanges();
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
