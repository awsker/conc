using System.Linq;
using conc.game.extensions;
using conc.game.math;
using Microsoft.Xna.Framework;
using tile;
using tile.math;

namespace conc.game.entity.movement
{
    public class MovementHandler : IMovementHandler
    {
        public void HandleMovement(GameTime time, IMovingEntity entity, ILevel level)
        {
            if (entity.CollisionSettings.CheckCollisionsWithBackground)
                overlappingWithBackground(entity, level.CollisionLines);

            var collisionLine = moveEntity(entity, time.ElapsedGameTime.TotalSeconds, level.CollisionLines);
        }

        private bool overlappingWithBackground(IMovingEntity entity, Line[] collisionLines)
        {
            var boundingBox = entity.BoundingBox;
            bool overlap = false;
            foreach (var cLine in collisionLines)
            {
                overlap |= overlappingWithLine(boundingBox, entity, cLine);
            }
            return overlap;
        }

        private bool overlappingWithLine(Box boundingBox, IMovingEntity entity, Line line)
        {
            var lines = boundingBox.Lines;
            bool overlap = false;
            for (int i = 0; i < lines.Length; ++i)
            {
                var tempPos = entity.Transform.Position;
                if (entity.CollisionSettings.ActionOnCollision == ActionOnCollision.PushOut)
                {
                    float backoff = 0.1f;
                    while (line.Intersecting(lines[i]))
                    {
                        overlap = true;
                        tempPos = Vector2.Add(tempPos, line.Normal * backoff);
                        //Try to keep the entity on an even pixel (unnecessary?)
                        entity.Transform.Position = tempPos;
                            //new Vector2((float) Math.Round(tempPos.X), (float) Math.Round(tempPos.Y));
                        boundingBox.Position = entity.Transform.Position;
                        lines = boundingBox.Lines;
                        backoff *= 2;
                    }
                }
                else
                {
                    if (line.Intersecting(lines[i]))
                        return true;
                }
            }
            return overlap;
        }

        private Line moveEntity(IMovingEntity entity, double totalSeconds, Line[] collisionLines)
        {
            var velocity = entity.Velocity * (float)totalSeconds;
            var normalizedVelocity = entity.Velocity.Normalized();
            if (velocity.Length() <= 0)
                return null;

            var moved = false;
            Line closestLine = null;
            if (entity.CollisionSettings.CheckCollisionsWithBackground)
            {
                var movementLines =
                    entity.BoundingBox.Lines.Select(l => new Line(l.Start, Vector2.Add(l.Start, velocity)));
                float? shortestDistance = null;
                Vector2 closestIntersection = Vector2.Zero;
                foreach (var mv in movementLines)
                {
                    foreach (var cl in collisionLines.Where(l => vectorsCanCollide(l.Normal, normalizedVelocity)))
                    {
                        if (cl.Intersecting(mv, out var intersection))
                        {
                            var dist = (intersection - mv.Start).Length();
                            if (closestLine == null || dist < shortestDistance.Value)
                            {
                                closestLine = cl;
                                shortestDistance = dist;
                                closestIntersection = intersection;
                            }
                        }
                    }
                }
                
                if (closestLine != null)
                {
                    if (entity.CollisionSettings.ActionOnCollision == ActionOnCollision.PushOut)
                    {
                        moved = true;
                        entity.Transform.Position = Vector2.Add(entity.Transform.Position, normalizedVelocity * shortestDistance.Value); //Move close to the wall as fuck
                        var closestLineVector = closestLine.Vector;
                        var closestLineNormal = new Vector2(closestLineVector.Y, -closestLineVector.X);

                        var newVectorVelocity = Vector2.Dot(entity.Velocity, closestLineVector) / Vector2.Dot(closestLineVector, closestLineVector) * closestLineVector * entity.CollisionSettings.BounceVector;
                        var newNormalVelocity = Vector2.Dot(entity.Velocity, closestLineNormal) / Vector2.Dot(closestLineNormal, closestLineNormal) * closestLineNormal * -entity.CollisionSettings.BounceNormal;
                        var newVelocity = newVectorVelocity + newNormalVelocity;
                        var fractLeftToMove = shortestDistance / velocity.Length();
                        entity.Velocity = newVelocity;
                        overlappingWithLine(entity.BoundingBox, entity, closestLine); //Make sure the entity is no longer touching the wall line
                        if (fractLeftToMove > 0.01f)
                        {
                            moveEntity(entity, (float)(totalSeconds * fractLeftToMove), collisionLines);
                        }
                    }
                    entity.OnCollisionWithBackground(this, closestIntersection, closestLine);
                }
            }
            if (!moved)
                entity.Transform.Position = Vector2.Add(entity.Transform.Position, velocity);

            return closestLine;
        }

        private bool vectorsCanCollide(Vector2 v1, Vector2 v2)
        {
            return Vector2.Dot(v1, v2) < 0;
        }
    }
}
