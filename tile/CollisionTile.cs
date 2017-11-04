using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace tile
{
    public interface ICollisionTile
    {
        Rectangle Bounds { get; set; }
        Slope Slope { get; set; }
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
