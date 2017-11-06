using System.Collections.Generic;
using conc.game.entity.baseclass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.scenes.baseclass
{
    public interface IScene
    {
        void SetGameManager(IGameManager manager);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }

    public abstract class Scene : IScene
    {
        public Scene()
        {
            Entities = new List<IEntity>();
        }

        protected IList<IEntity> Entities { get; }
        public virtual void SetGameManager(IGameManager manager) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}