using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using conc.game.commands;
using conc.game.entity.baseclass;
using Microsoft.Xna.Framework;
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
    }

    public abstract class Scene : IScene
    {
        protected readonly GameManager _gameManager;

        protected Scene(GameManager gameManager)
        {
            _gameManager = gameManager;
            _entities = new List<IEntity>();
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
        }

        public GameManager GameManager => _gameManager;

        public virtual void LoadContent() { }

        public virtual void Update(GameTime gameTime)
        {
            //Remove all destroyed entities
            for (int i = _entities.Count - 1; i >= 0; --i)
            {
                if(_entities[i].IsDestroyed)
                    _entities.RemoveAt(i);
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}