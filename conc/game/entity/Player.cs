using conc.game.entity.@base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.entity
{
    public interface IPlayer : IEntity
    {
    }

    public class Player : Entity, IPlayer
    {
        private Texture2D _texture;

        public Player(Vector2 position = new Vector2()) : base(position)
        {
        }

        public override void LoadContent(ContentManager contentManager)
        {
            _texture = contentManager.Load<Texture2D>("trash/temp");
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Transform.Position, Color.White);
        }
    }
}