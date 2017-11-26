using System;
using System.Linq;
using conc.game.entity.animation;
using conc.game.entity.baseclass;
using conc.game.entity.movement;
using conc.game.extensions;
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

        private int _doubleJumpsRemaining;
        private GameTime _lastGroundTouch;
        private GameTime _lastJump;
        private bool _hasJumpedThisPress;

        private RopeProjectile _rope;

        private Rectangle _checkpoint;

        public Player(Vector2 position, IAnimator animator) : base(position, animator)
        {
            CollisionSettings = new CollisionSettings(true, ActionOnCollision.PushOut, 0f, 1f);
            _settings = new PlayerMovementSettings
            {
                Acceleration = 800f,
                Deacceleration = 800f,
                JumpSpeed = 600f,
                JumpHeight = 75f,
                MaxSpeed = 200f,
                Gravity = 400f,
                TerminalVelocity = 800f,
                InAirAccelerationFactor = .7f,
                InAirDeaccelerationFactor = .7f,
                JumpTimeSlack = TimeSpan.FromMilliseconds(100), //100 milliseconds of Wile E. Coyote time
                WallSlideMaxDownSpeed = 75f
            };

            if (Scene is GameScene gameScene)
                _checkpoint = gameScene.CurrentLevel.Checkpoints[0];
        }
        
        public Vector2 Velocity { get; set; }
        public CollisionSettings CollisionSettings { get; }

        public void OnCollisionWithBackground(IMovementHandler handler, Vector2 collision, Line line)
        {
            if (isLineCeiling(line))
            {
                _isJumping = false;
                _cancelJump = true;
            }
            retractRope();
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
            updateInAirStatus(gameTime);
            updateLevelObjects();
            applyGravity(gameTime);
            applyWallSlideDeacceleration(gameTime);
            readInput(gameTime);
            if (Velocity.Y < 0)
                GameDebug.Log("Velocity: ", Velocity.Y);
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

        private void updateInAirStatus(GameTime gametime)
        {
            var inputManager = Scene.GameManager.Get<InputManager>();
            bool holdingLeft = inputManager.IsDown(ControlButtons.Left, _playerNo);
            bool holdingRight = inputManager.IsDown(ControlButtons.Right, _playerNo);
            if (Scene is GameScene gamescene)
            {
                var lines = gamescene.CurrentLevel.CollisionLines;

                var rightSideLine = createTouchSensorLine(BoundingBox.Lines[1], 0.5f);
                var footLines = createTouchSensorCrossLines(BoundingBox.Lines[2]);
                var leftSideLine = createTouchSensorLine(BoundingBox.Lines[3], 0.5f);

                //_onRoof = lines.Any(l => l.Intersecting(roofLine));
                _onRightWall = /*holdingRight &&*/ lines.Any(l => l.Intersecting(rightSideLine));
                _onLeftWall = /*holdingLeft &&*/ lines.Any(l => l.Intersecting(leftSideLine));
                _currentGround = isSensorTouchingGroundLine(footLines, lines);
                _onGround = _currentGround != null;

                if (isTouchingJumpableGround(gametime))
                {
                    _lastGroundTouch = new GameTime(gametime.TotalGameTime, gametime.ElapsedGameTime);
                    _doubleJumpsRemaining = 1;
                }
                if (_rope != null && _rope.IsHooked())
                {
                    _doubleJumpsRemaining = 1;
                }
            }
        }

        private void applyWallSlideDeacceleration(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var deacceleration = _settings.Deacceleration * (_onGround ? 1f : _settings.InAirDeaccelerationFactor) * dt;
            if (isClingingToWall() && Velocity.Y > _settings.WallSlideMaxDownSpeed)
            {
                Velocity = new Vector2(Velocity.X, Math.Max(Velocity.Y - deacceleration, _settings.WallSlideMaxDownSpeed));
            }
        }
        
        private bool isTouchingJumpableGround(GameTime time)
        {
            var enoughTimePassedSinceLastJump = _lastJump == null || time.TotalGameTime - _lastJump.TotalGameTime > TimeSpan.FromMilliseconds(100);
            return enoughTimePassedSinceLastJump && (_onRightWall || _onLeftWall || _onGround);
        }

        private bool isClingingToWall()
        {
            return !_onGround && (_onLeftWall || _onRightWall);
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
                Velocity = Vector2.Zero;
                IsAlive = true;
            }
        }

        private bool canJump(GameTime time)
        {
            var enoughTimePassedSinceLastJump = _lastJump == null || time.TotalGameTime - _lastJump.TotalGameTime > TimeSpan.FromMilliseconds(100);
            return enoughTimePassedSinceLastJump && 
                (time.TotalGameTime - _lastGroundTouch.TotalGameTime <= _settings.JumpTimeSlack ||
                _doubleJumpsRemaining > 0);
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
            if (inputManager.IsDown(ControlButtons.Left, _playerNo))
            {
                LookDirection = LookDirection.Left;
                if (!inputManager.IsDown(ControlButtons.Right, _playerNo) && Velocity.X > -maxSpeedX)
                {
                    var directionVector = _currentGround?.Vector.Normalized() * -1 ?? new Vector2(-1f, 0f);
                    Velocity = Vector2.Add(Velocity, directionVector * acceleration);
                }
            }
            //Move right
            if (inputManager.IsDown(ControlButtons.Right, _playerNo))
            {
                LookDirection = LookDirection.Right;
                if (!inputManager.IsDown(ControlButtons.Left, _playerNo) && Velocity.X < maxSpeedX)
                {
                    var directionVector = _currentGround?.Vector.Normalized() ?? new Vector2(1f, 0f);
                    Velocity = Vector2.Add(Velocity, directionVector * acceleration);
                }
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

            //Jump - If holding key and (ground)jump has not been performed on the current press, or if button was just pressed
            if (inputManager.IsDown(ControlButtons.Jump, _playerNo) && !_hasJumpedThisPress ||
                inputManager.IsPressed(ControlButtons.Jump, _playerNo))
            {
                jump(gameTime);
            }
            if (!inputManager.IsDown(ControlButtons.Jump, _playerNo))
            {
                _hasJumpedThisPress = false;
            }

            if (!inputManager.IsDown(ControlButtons.Jump, _playerNo) && _isJumping)
            {
                _cancelJump = true;
            }

            //Fire ninja rope
            if (inputManager.IsPressed(ControlButtons.FireRope, _playerNo) && (_rope == null || _rope.IsDestroyed))
            {
                fireNinjaRope();
            }
            //Remove rope when key is let go
            if (!inputManager.IsDown(ControlButtons.FireRope, _playerNo))
            {
                retractRope();
            }
        }

        private void jump(GameTime currentTime)
        {
            if (canJump(currentTime))
            {
                if (currentTime.TotalGameTime - _lastGroundTouch.TotalGameTime > _settings.JumpTimeSlack)
                    --_doubleJumpsRemaining;
                
                _hasJumpedThisPress = true;
                _jumpPeak = Transform.Position.Y - _settings.JumpHeight;
                if (isClingingToWall())
                {
                    if(_onLeftWall && !_onRightWall)
                        Velocity = new Vector2(_settings.JumpSpeed * 0.4f, Velocity.Y);
                    if(_onRightWall && !_onLeftWall)
                        Velocity = new Vector2(-_settings.JumpSpeed * 0.4f, Velocity.Y);
                }
                _isJumping = true;
                _cancelJump = false;
                _onGround = false;
                _lastJump = new GameTime(currentTime.TotalGameTime, currentTime.ElapsedGameTime);
            }
        }

        
        private void fireNinjaRope()
        {
            var rope = new RopeProjectile(this, LookDirection);
            Scene.AddEntity(rope);
            _rope = rope;
        }

        private void retractRope()
        {
            _rope?.Retract();
        }

        public bool IsAlive { get; private set; }
        public LookDirection LookDirection { get; private set; }
    }

    public enum LookDirection
    {
        Right = 0,
        Left = 1
    }
}