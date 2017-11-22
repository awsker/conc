namespace conc.game.entity.movement
{
    public class CollisionSettings
    {
        public bool CheckCollisionsWithBackground { get; set; }
        public ActionOnCollision ActionOnCollision { get; set; }
        public float BounceNormal { get; set; }
        public float BounceVector { get; set; }

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
