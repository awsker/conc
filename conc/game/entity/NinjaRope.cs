using conc.game.entity.baseclass;
using conc.game.entity.movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.entity
{
    public class NinjaRope : Entity
    {
        public override int Width => 0;
        public override int Height => 0;

        private Vector2 _position;
        private IMovingEntity _owner;

        public NinjaRope(Vector2 position, IMovingEntity owner)
        {
            _position = position;
            _owner = owner;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
