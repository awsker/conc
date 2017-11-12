using System;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace tile.math
{
    [DataContract(Name = "Line")]
    public class Line
    {
        private Vector2 _origin, _target;
        private LineType _type;

        private float _length;
        private float _lengthSquared;

        public Line(float startX, float startY, float targetX, float targetY, LineType type = LineType.BothCapped):this(new Vector2(startX, startY), new Vector2(targetX, targetY), type)
        { }

        public Line(Vector2 start, Vector2 end, LineType type = LineType.BothCapped)
        {
            _origin = start;
            _target = end;
            _type = type;
            recalculateLenghts();
        }
        
        [DataMember]
        public Vector2 Start
        {
            get { return _origin; }
            set
            {
                _origin = value;
                recalculateLenghts();
            }
        }

        [DataMember]
        public Vector2 End
        {
            get { return _target; }
            set
            {
                _target = value;
                recalculateLenghts();
            }
        }

        public float Length => _length;
        public float LengthSquared => _lengthSquared;

        public Vector2 Normal
        {
            get
            {
                var v = Vector;
                v.Normalize();
                return new Vector2(v.Y, -v.X);
            }
        }
        
        public Vector2 Vector => _target - _origin;

        [DataMember]
        public LineType LineType
        {
            get { return _type; }
            set
            {
                _type = value;
                recalculateLenghts();
            }
        }

        private void recalculateLenghts()
        {
            var v = Vector;
            _length = v.Length();
            _lengthSquared = v.LengthSquared();
        }
        
        public bool Intersecting(Line other)
        {
            return Intersecting(other, out Vector2 temp);
        }

        public bool Intersecting(Line other, out Vector2 pointOfIntersection)
        {
            //3d -> 2d
            Vector2 p1 = Start;
            Vector2 p2 = End;

            Vector2 p3 = other.Start;
            Vector2 p4 = other.End;

            float denominator = (p4.Y - p3.Y) * (p2.X - p1.X) - (p4.X - p3.X) * (p2.Y - p1.Y);

            pointOfIntersection = Vector2.Zero;
            //Make sure the denominator is > 0, if so the lines are parallel
            if (Math.Abs(denominator) > float.Epsilon)
            {
                float u_a = ((p4.X - p3.X) * (p1.Y - p3.Y) - (p4.Y - p3.Y) * (p1.X - p3.X)) / denominator;
                float u_b = ((p2.X - p1.X) * (p1.Y - p3.Y) - (p2.Y - p1.Y) * (p1.X - p3.X)) / denominator;

                pointOfIntersection = p1 + (p2 - p1) * u_a;

                //Is intersecting if u_a and u_b are between 0 and 1
                return withinLine(u_a, _type) && withinLine(u_b, other._type);
            }
            return false;
        }

        public bool IntersectingCircle(Vector2 origin, float radius)
        {
            return DistanceToPoint(origin) <= radius;
        }

        public bool Intersecting(IPolygon poly)
        {
            return Intersecting(poly, out Vector2 temp);
        }

        public bool Intersecting(IPolygon poly, out Vector2 pointOfIntersection)
        {
            float? minDist = null;
            pointOfIntersection = Vector2.Zero;
            foreach (var line in poly.Lines)
            {
                Vector2 p;
                if (Intersecting(line, out p))
                {
                    var length = (p - _origin).LengthSquared();
                    if (minDist == null || length < minDist.Value)
                    {
                        minDist = length;
                        pointOfIntersection = p;
                    }
                }
            }
            return minDist.HasValue;
        }

        public float DistanceToPoint(Vector2 point)
        {
            return DistanceToPoint(point, out Vector2 temp);
        }

        public float DistanceToPoint(Vector2 point, out Vector2 closestPointOnLine)
        {
            var l =  Vector2.Normalize(Vector);
            float distOnLine = Vector2.Dot(point - _origin, l);
            var u = distOnLine / Length;
            if (withinLine(u, _type))
            {
                closestPointOnLine = l * distOnLine;
            }
            else
                closestPointOnLine = u < 0 ? _origin : _target;
            return (point - closestPointOnLine).Length();
        }


        private bool withinLine(float u, LineType type)
        {
            return type == LineType.Infinite || u >= 0f && (type == LineType.InfiniteAtTarget || u <= 1f);
        }
    }
    
    public enum LineType
    {
        BothCapped,
        InfiniteAtTarget,
        Infinite
    }
}
