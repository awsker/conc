using System;

namespace conc.game.entity
{
    public class PlayerMovementSettings
    {
        public float MaxSpeed, Acceleration, Deacceleration, JumpSpeed, JumpHeight, InAirAccelerationFactor, InAirDeaccelerationFactor, Gravity, TerminalVelocity, WallSlideMaxDownSpeed;
        public TimeSpan JumpTimeSlack;
        public int NumDoubleJumps;
    }
}
