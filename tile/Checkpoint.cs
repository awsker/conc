using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace tile
{
    [DataContract(Name = "Checkpoint")]
    public class Checkpoint
    {
        [DataMember]
        public Point Spawn { get; set; }
        [DataMember]
        public Rectangle Rectangle { get; set; }
    }
}
