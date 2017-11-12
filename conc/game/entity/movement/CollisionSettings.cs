namespace conc.game.entity.movement
{
    public struct CollisionSettings
    {
        public bool CheckCollisionsWithBackground { get; }
        public ActionOnCollision ActionOnCollision { get; }
        public float BounceNormal { get; }
        public float BounceVector { get; }

        public CollisionSettings(bool checkCollisions, ActionOnCollision action, float bounceNormal, float bounceVector)
        {
            CheckCollisionsWithBackground = checkCollisions;
            ActionOnCollision = action;
            BounceNormal = bounceNormal;
            BounceVector = bounceVector;
        }
    }

    public enum ActionOnCollision
    {
        None,
        PushOut
    }
}
