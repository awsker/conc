using System.Collections.Generic;
using System.Collections.ObjectModel;
using conc.game.commands;
using conc.game.entity.baseclass;
using conc.game.gui.components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.scenes.baseclass
{
    public interface IScene
    {
        GameManager GameManager { get; }
        void LoadContent();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        void AddEntity(IEntity entity);
        IList<IGuiComponent> GuiComponents { get; }
    }

    public abstract class Scene : IScene
    {
        protected readonly GameManager _gameManager;

        protected Scene(GameManager gameManager)
        {
            _gameManager = gameManager;
            _entities = new List<IEntity>();

            GuiComponents = new List<IGuiComponent>();
        }

        protected void ExecuteCommand(Command command)
        {
            _gameManager?.ExecuteCommand(command);
        }

        protected readonly IList<IEntity> _entities;

        protected IReadOnlyCollection<IEntity> Entities => new ReadOnlyCollection<IEntity>(_entities);

        public virtual void AddEntity(IEntity entity)
        {
            _entities.Add(entity);
            entity.Scene = this;
            entity.LoadContent(_gameManager.Get<ContentManager>());
        }

        public IList<IGuiComponent> GuiComponents { get; }

        public GameManager GameManager => _gameManager;

        public virtual void LoadContent() { }

        public virtual void RemoveDestroyedEntities()
        {
            //Remove all destroyed entities
            for (int i = _entities.Count - 1; i >= 0; --i)
            {
                if (_entities[i].IsDestroyed)
                    _entities.RemoveAt(i);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            RemoveDestroyedEntities();
        }


        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}