using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using tile;
using tile.math;

namespace BGStageGenerator
{
    public class CollisionPolygonGenerator
    {
        public Line[] GetCollisionPolygons(ILevel level)
        {
            var tileWidth = level.Tilesets[0].TileWidth;
            var tileHeight = level.Tilesets[0].TileHeight;
            var levelData = new Dictionary<Point, LineDirection>();//new LineDirection[level.Width + 1, level.Height + 1];
            for (int x = 0; x < level.Width; ++x)
            {
                for (int y = 0; y < level.Height; ++y)
                {
                    var tile = level.Tiles[x, y];
                    if (tile == null)
                        continue;

                    if (tile.Slope != Slope.FloorUp && tile.Slope != Slope.FloorDown)
                        AddLine(LineDirection.Right, levelData, x, y);
                    if (tile.Slope != Slope.FloorDown && tile.Slope != Slope.RoofUp)
                        AddLine(LineDirection.Down, levelData, x + 1, y);
                    if (tile.Slope != Slope.RoofUp && tile.Slope != Slope.RoofDown)
                        AddLine(LineDirection.Left, levelData, x + 1, y + 1);
                    if (tile.Slope != Slope.RoofDown && tile.Slope != Slope.FloorUp)
                        AddLine(LineDirection.Up, levelData, x, y + 1);
                    if (tile.Slope == Slope.FloorDown)
                        AddLine(LineDirection.DownRight, levelData, x, y);
                    if (tile.Slope == Slope.FloorUp)
                        AddLine(LineDirection.UpRight, levelData, x, y + 1);
                    if (tile.Slope == Slope.RoofDown)
                        AddLine(LineDirection.UpLeft, levelData, x + 1, y + 1);
                    if (tile.Slope == Slope.RoofUp)
                        AddLine(LineDirection.DownLeft, levelData, x + 1, y);
                }
            }
            var lines = new List<Line>();

            //Remove any wall data that was 0 because two walls overlapped
            foreach (var p in levelData.Keys.ToList())
            {
                if(levelData[p] == 0)
                    levelData.Remove(p);
            }
            while (levelData.Count > 0)
            {
                var pointsThisPolygon = new List<Point>();
                var start = levelData.First();
                var currentLinestart = start.Key;
                var currentPoint = start.Key;
                var currentld = nextLineDirection(start.Value, LineDirection.Right);
                do
                {
                    pointsThisPolygon.Add(currentPoint);
                    //Consume the current wall
                    levelData[currentPoint] &= ~currentld;
                    var offset = Offset(currentld);
                    var nextP = new Point(currentPoint.X + offset.X, currentPoint.Y + offset.Y);
                    currentPoint = nextP;
                    var nextld = nextLineDirection(levelData[nextP], currentld);
                    if (nextld != currentld)
                    {
                        lines.Add(new Line(currentLinestart.X * tileWidth, currentLinestart.Y * tileHeight, currentPoint.X * tileWidth, currentPoint.Y * tileHeight));
                        currentLinestart = currentPoint;
                        currentld = nextld;
                    }
                } while (currentPoint != start.Key);

                //Remove all point that have all their walls consumed
                foreach (var p in pointsThisPolygon)
                {
                    if (levelData[p] == 0)
                        levelData.Remove(p);
                }
            }
            return lines.ToArray();
        }

        private void AddLine(LineDirection ld, Dictionary<Point, LineDirection> data, int x, int y)
        {
            var offset = Offset(ld);
            var opposite = Opposite(ld);
            var p = new Point(x, y);
            var op = new Point(x + offset.X, y + offset.Y);
            LineDirection prevld;
            if (data.TryGetValue(op, out prevld))
            {
                if ((prevld & opposite) > 0)
                {
                    data[op] &= ~opposite;
                    return;
                }
            }
            if (data.TryGetValue(p, out prevld))
            {
                data[p] = prevld | ld;
            }
            else
            {
                data[p] = ld;
            }
        } 

        private Point Offset(LineDirection ld)
        {
            switch (ld)
            {
                case LineDirection.Right:
                    return new Point(1, 0);
                case LineDirection.UpRight:
                    return new Point(1, -1);
                case LineDirection.Up:
                    return new Point(0, -1);
                case LineDirection.UpLeft:
                    return new Point(-1, -1);
                case LineDirection.Left:
                    return new Point(-1, 0);
                case LineDirection.DownLeft:
                    return new Point(-1, 1);
                case LineDirection.Down:
                    return new Point(0, 1);
                case LineDirection.DownRight:
                    return new Point(1, 1);
                default:
                    return Point.Zero;
            }
        }

        private LineDirection Opposite(LineDirection ld)
        {
            var d = (int) ld;
            return (LineDirection) (((d & 15) << 4) + ((d & 240) >> 4));
        }

        private LineDirection nextLineDirection(LineDirection ld, LineDirection startDirection)
        {
            int d = (int)startDirection + ((int)startDirection << 8);
            do
            { 
                if ((ld & (LineDirection)d) > 0)
                    break;
                d >>= 1;
            } while (d > 0);
            return (LineDirection) (d & 255);
        }

        [Flags]
        private enum LineDirection
        {
            Right = 1,
            UpRight = 2,
            Up = 4,
            UpLeft = 8,
            Left = 16,
            DownLeft =32,
            Down = 64,
            DownRight = 128
        }
    }


}
