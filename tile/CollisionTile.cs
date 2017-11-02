using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace tile
{
    public interface ICollisionTile
    {
        Rectangle Bounds { get; set; }
        CollisionType Type { get; set; }
    }

    [DataContract(Name = "CollisionTile")]
    public class CollisionTile : ICollisionTile
    {
        public CollisionTile(Rectangle bounds, CollisionType type)
        {
            Bounds = bounds;
            Type = type;
        }

        [DataMember]
        public Rectangle Bounds { get; set; }

        [DataMember]
        public CollisionType Type { get; set; }
    }

    [DataContract]
    public enum CollisionType
    {
        [EnumMember]
        None,
        [EnumMember]
        Solid,
        [EnumMember]
        Edge,
        [EnumMember]
        Platform,
        [EnumMember]
        Ladder,
        [EnumMember]
        LadderTop,
        [EnumMember]
        LadderBottom
    }
}
