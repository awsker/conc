using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace tile
{
    public interface ICollisionTile
    {
        Rectangle Bounds { get; set; }
        Slope Slope { get; set; }
        bool IsPointInside(Point p);
    }

    [DataContract(Name = "CollisionTile")]
    public class CollisionTile : ICollisionTile
    {
        public CollisionTile(Rectangle bounds, Slope type)
        {
            Bounds = bounds;
            Slope = type;
        }

        [DataMember]
        public Rectangle Bounds { get; set; }

        [DataMember]
        public Slope Slope { get; set; }

        public bool IsPointInside(Point p)
        {
            var insideSquare = p.X >= Bounds.X && p.X < Bounds.X + Bounds.Width &&
                               p.Y >= Bounds.Y && p.Y < Bounds.Y + Bounds.Height;

            if (!insideSquare)
                return false;

            if(Slope == Slope.None)
                return true;

            var rx = p.X - Bounds.X;
            var ry = p.Y - Bounds.Y;
            var ryi = Bounds.Height - 1 - ry;

            if (Slope == Slope.FloorDown)
            {
                return rx <= ry;
            }
            if (Slope == Slope.FloorDown)
            {
                return rx <= ryi;
            }
            if (Slope == Slope.RoofDown)
            {
                return rx >= ry;
            }
            if (Slope == Slope.RoofDown)
            {
                return rx >= ryi;
            }
            return false;
        }
    }

    [DataContract]
    public enum Slope
    {
        [EnumMember]
        None,
        [EnumMember]
        FloorDown,
        [EnumMember]
        FloorUp,
        [EnumMember]
        RoofDown,
        [EnumMember]
        RoofUp
    }
}
