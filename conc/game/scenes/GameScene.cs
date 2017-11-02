using System.IO;
using conc.game.scenes.@base;
using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using tile;

namespace conc.game.scenes
{
    public interface IGameScene : IScene
    {
        void SetLevel(ILevel level);
    }

    public class GameScene : Scene, IGameScene
    {
        private ILevel _level;
        private Texture2D _tileset;
        private readonly ISpriteBank _spriteBank;

        public GameScene(GraphicsDevice graphicsDevice, ISpriteBank spriteBank) : base(graphicsDevice)
        {
            _spriteBank = spriteBank;
        }

        public override void LoadContent(ContentManager contentManager)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        public void SetLevel(ILevel level)
        {
            _level = level;
            
            var tilesetPath = Path.GetFullPath(@"..\..\..\..\content\tiledgenerator\content\");
            _tileset = _spriteBank.Get(tilesetPath + _level.Tileset);
        }
    }
}