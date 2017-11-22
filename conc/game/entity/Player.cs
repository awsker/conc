using System;
using System.Linq;
using conc.game.entity.animation;
using conc.game.entity.baseclass;
using conc.game.entity.movement;
using conc.game.input;
using conc.game.scenes;
using conc.game.util;
using Microsoft.Xna.Framework;
using tile.math;

namespace conc.game.entity
{
    public interface IPlayer : IAnimatedEntity, IMovingEntity
    {
        bool IsAlive { get; }
    }

    public class Player : AnimatedEntity, IPlayer
    {
        private bool _onGround, _onLeftWall, _onRightWall;
        private Line _currentGround;
        private readonly PlayerMovementSettings _settings;
        private int _playerNo = 0;

        private float _jumpPeak;
        private bool _isJumping;
        private bool _cancelJump;

        private Rectangle _checkpoint;

        public Player(Vector2 position, IAnimator animator) : base(position, animator)
        {
            CollisionSettings = new CollisionSettings(true, ActionOnCollision.PushOut, 0f, 1f);
            _settings = new PlayerMovementSettings
            {
                Acceleration = 1000f,
                Deacceleration = 1000f,
                JumpSpeed = 500f,
                JumpHeight = 75f,
                MaxSpeed = 200f,
                Gravity = 300f,
                TerminalVelocity = 800f,
                InAirAccelerationFactor = .7f,
                InAirDeaccelerationFactor = .7f
            };

            if (Scene is GameScene gameScene)
                _checkpoint = gameScene.CurrentLevel.Checkpoints[0];
        }
        
        public Vector2 Velocity { get; set; }
        public CollisionSettings CollisionSettings { get; }

        public void OnCollisionWithBackground(MovementHandler handler, Vector2 collision, Line line)
        {
            if(isLineValidGround(line))
            {
                _onGround = true;
                _currentGround = line;
            }
            if (isLineCeiling(line))
                _isJumping = false;

        }

        private bool isLineValidGround(Line line)
        {
            var lineAngle = Vector2.Dot(line.Vector.Normalized(), new Vector2(1f, 0f));
            return lineAngle > 0.707;
        }

        private bool isLineCeiling(Line line)
        {
            var lineAngle = Vector2.Dot(line.Vector.Normalized(), new Vector2(1f, 0f));
            return lineAngle < -0.999;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            updateJump();
            updateInAirStatus();
            updateLevelObjects();
            applyGravity(gameTime);
            readInput(gameTime);
        }

        private void updateJump()
        {
            if (_isJumping)
            {
                var distanceLeft = Transform.Position.Y - _jumpPeak;
                if (distanceLeft <= 20f)
                {
                    _isJumping = false;
                    _cancelJump = false;
                    return;
                }

                if (_cancelJump)
                {
                    _jumpPeak = Transform.Position.Y - 20f;
                    distanceLeft = Transform.Position.Y - _jumpPeak;
                    _cancelJump = false;
                }

                var newY = -(distanceLeft * _settings.JumpSpeed)*.005f;
                Velocity = new Vector2(Velocity.X, newY);
            }
        }

        private void applyGravity(GameTime gameTime)
        {
            if (!_onGround)
            {
                var newY = Math.Min(Velocity.Y + _settings.Gravity * (float) gameTime.ElapsedGameTime.TotalSeconds, _settings.TerminalVelocity);
                Velocity = new Vector2(Velocity.X, newY);
            }
        }

        private void updateInAirStatus()
        {
            var inputManager = Scene.GameManager.Get<InputManager>();
            bool holdingLeft = inputManager.IsDown(ControlButtons.Left, _playerNo);
            bool holdingRight = inputManager.IsDown(ControlButtons.Right, _playerNo);
            if (Scene is GameScene gamescene)
            {
                var lines = gamescene.CurrentLevel.CollisionLines;

                //var roofLine = createTouchSensorCrossLines(BoundingBox.Lines[0]);
                var rightSideLine = createTouchSensorLine(BoundingBox.Lines[1], 0.5f);
                var footLines = createTouchSensorCrossLines(BoundingBox.Lines[2]);
                var leftSideLine = createTouchSensorLine(BoundingBox.Lines[3], 0.5f);

                //_onRoof = lines.Any(l => l.Intersecting(roofLine));
                _onRightWall = holdingRight && lines.Any(l => l.Intersecting(rightSideLine));
                _onLeftWall = holdingLeft && lines.Any(l => l.Intersecting(leftSideLine));
                _currentGround = isSensorTouchingGroundLine(footLines, lines);
                _onGround = _currentGround != null;
            }
        }

        private void updateLevelObjects()
        {
            if (Scene is GameScene gameScene)
            {
                foreach (var checkpoint in gameScene.CurrentLevel.Checkpoints)
                {
                    var boundingRectangle = BoundingBox.Rectangle;
                    if (_checkpoint != checkpoint && boundingRectangle.Intersects(checkpoint))
                        _checkpoint = checkpoint;
                }

                foreach (var death in gameScene.CurrentLevel.Deaths)
                {
                    var boundingRectangle = BoundingBox.Rectangle;
                    if (boundingRectangle.Intersects(death))
                        IsAlive = false;
                }
            }

            if (!IsAlive)
            {
                Transform.Position = new Vector2(_checkpoint.X + _checkpoint.Width/2f, _checkpoint.Top);
                IsAlive = true;
            }
        }

        private Line createTouchSensorLine(Line side, float placeOnLine)
        {
            var start = side.Start * placeOnLine + side.End * (1f - placeOnLine);
            return new Line(start, Vector2.Add(start, side.Normal));
        }

        private Line[] createTouchSensorCrossLines(Line side)
        {
            Line[] lines = new Line[2];
            lines[0] = new Line(side.Start, Vector2.Add(side.End, side.Normal));
            lines[1] = new Line(side.End, Vector2.Add(side.Start, side.Normal));
            return lines;
        }

        private Line isSensorTouchingGroundLine(Line[] sensors, Line[] groundLines)
        {
            var linesToSearch = groundLines.Where(isLineValidGround);
            Line bestLine = null;
            float lowestY = 0;
            float lowestYsHighestY = 0;
            foreach (var gl in linesToSearch)
            {
                var glLowestY = Math.Min(gl.Start.Y, gl.End.Y);
                var glHighestY = Math.Max(gl.Start.Y, gl.End.Y);
                if (bestLine != null && lowestY < glLowestY) //Skip line if we've already touched a line higher up than this
                    continue;
                foreach (var sensor in sensors)
                {
                    if (sensor.Intersecting(gl) && (bestLine == null || glLowestY < lowestY || glLowestY.Equals(lowestY) && glHighestY < lowestYsHighestY))
                    {
                        bestLine = gl;
                        lowestY = glLowestY;
                        lowestYsHighestY = glHighestY;
                        break; //Remaining sensors don't need to check against this line
                    }
                }
            }
            return bestLine;
        }
        
        private void readInput(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var inputManager = Scene.GameManager.Get<InputManager>();

            var acceleration = _settings.Acceleration * (_onGround ? 1f : _settings.InAirAccelerationFactor) * dt;
            var deacceleration = _settings.Deacceleration * (_onGround ? 1f : _settings.InAirDeaccelerationFactor) * dt;
            
            var maxSpeedX = _settings.MaxSpeed;
            if (_currentGround != null)
            {
                var costheta = Vector2.Dot(_currentGround.Vector.Normalized(), new Vector2(1f, 0f));
                maxSpeedX = _settings.MaxSpeed * Math.Abs(costheta);
            }
            
            //Move left
            if (inputManager.IsDown(ControlButtons.Left, _playerNo) && !inputManager.IsDown(ControlButtons.Right, _playerNo) && Velocity.X > -maxSpeedX)
            {
                var directionVector = _currentGround?.Vector.Normalized()* -1 ?? new Vector2(-1f, 0f);
                Velocity = Vector2.Add(Velocity, directionVector * acceleration);
            }

            //Move right
            if (inputManager.IsDown(ControlButtons.Right, _playerNo) && !inputManager.IsDown(ControlButtons.Left, _playerNo) && Velocity.X < maxSpeedX)
            {
                var directionVector = _currentGround?.Vector.Normalized() ?? new Vector2(1f, 0f);
                Velocity = Vector2.Add(Velocity, directionVector * acceleration);
            }
            bool deaccelerate = true; //_onGround;
            //Deaccelerate
            if (inputManager.IsDown(ControlButtons.Right, _playerNo) == inputManager.IsDown(ControlButtons.Left, _playerNo))
            {
                Vector2 directionVector = Vector2.Zero;
                if (Velocity.X > 0)
                    directionVector = _currentGround?.Vector.Normalized() * -1 ?? new Vector2(-1f, 0f);
                else if (Velocity.X < 0)
                    directionVector = _currentGround?.Vector.Normalized() ?? new Vector2(1f, 0f);
                var deaccVector = directionVector * deacceleration;
                bool stopX = Velocity.X > 0 && -deaccVector.X > Velocity.X || Velocity.X < 0 && -deaccVector.X < Velocity.X;
                bool stopY = stopX && _onGround;
                Velocity = stopX ? new Vector2(0, stopY ? 0 : Velocity.Y) : Vector2.Add(Velocity, deaccVector);
            }

            //Jump
            if (inputManager.IsDown(ControlButtons.Jump, _playerNo) && _onGround)
            {
                _jumpPeak = Transform.Position.Y - _settings.JumpHeight;
                _isJumping = true;
                _onGround = false;
            }

            if (!inputManager.IsDown(ControlButtons.Jump, _playerNo) && _isJumping)
            {
                _cancelJump = true;
            }
        }

        public bool IsAlive { get; private set; }
    }
}