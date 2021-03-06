﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;
using tile;
using Microsoft.Xna.Framework;
using tile.math;

namespace BGStageGenerator
{
    public class LevelGenerator
    {
        public IList<ILevel> Generate()
        {
            var levels = new List<ILevel>();
            foreach (var file in Directory.GetFiles(@"..\..\content\", "*.tmx"))
            {
                Console.WriteLine("Generating stage for file {0}", file);
                var level = readLevelFromFile(file);
                if (level != null)
                    levels.Add(level);
            }
            return levels;
        }

        private ILevel readLevelFromFile(string file)
        {
            using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                var doc = new XmlDocument();
                doc.Load(stream);

                if (doc.DocumentElement != null)
                {
                    IList<ITileset> tileSets = readTilesets(doc);
                    var stageName = Path.GetFileNameWithoutExtension(file);

                    var width = int.Parse(doc.DocumentElement.GetAttribute("width"));
                    var height = int.Parse(doc.DocumentElement.GetAttribute("height"));
                    var tileWidth = int.Parse(doc.DocumentElement.GetAttribute("tilewidth"));
                    var tileHeight = int.Parse(doc.DocumentElement.GetAttribute("tileheight"));

                    var level = new Level
                    {
                        Tiles = new ITile[width, height],
                        Background = new ITile[width, height],
                        Foreground = new ITile[width, height],
                        Deaths = new List<Rectangle>(),
                        Checkpoints = new List<Checkpoint>(),
                        TileWidth = tileWidth, 
                        TileHeight = tileHeight
                    };
                    CreateTiles(level, doc, width, height, tileSets);
                    CreateObjects(level, doc);

                    level.Name = stageName;
                    level.Tilesets = tileSets.ToArray();
                    level.CollisionLines = getCollisionPolygons(level);

                    return level;
                }
                //No document found
                return null;
            }
        }

        private IList<ITileset> readTilesets(XmlDocument doc)
        {
            var tilesets = new List<ITileset>();

            var tilesetNodes = doc.DocumentElement.SelectNodes("tileset");
            foreach (XmlNode tileset in tilesetNodes)
            {
                var tilesetName = tileset.Attributes["name"].Value;
                var tileWidth = int.Parse(tileset.Attributes["tilewidth"].Value);
                var tileHeight = int.Parse(tileset.Attributes["tileheight"].Value);

                var firstgid = int.Parse(tileset.Attributes["firstgid"].Value);
                var tilecount = int.Parse(tileset.Attributes["tilecount"].Value);
                var columns = int.Parse(tileset.Attributes["columns"].Value);
                var rows = tilecount / (columns + 1) + 1;

                var imageNode = tileset.SelectSingleNode("image");
                var imageWidth = int.Parse(imageNode.Attributes["width"].Value);
                var imageHeight = int.Parse(imageNode.Attributes["height"].Value);
                var source = imageNode.Attributes["source"].Value;

                source = Path.GetFileNameWithoutExtension(source);

                var newTileset = new Tileset(source, firstgid, tilecount, tileWidth, tileHeight, columns, rows);
                foreach (XmlNode tile in tileset.SelectNodes("tile"))
                {
                    var id = int.Parse(tile.Attributes["id"].Value);
                    var propertiesNode = tile.SelectSingleNode("properties");
                    if (propertiesNode != null)
                    {
                        var props = new TileProperties();
                        foreach (XmlNode property in propertiesNode)
                        {
                            var propName = property.Attributes["name"].Value;
                            var value = property.Attributes["value"].Value;
                            props[propName] = value;
                        }
                        if (props.Count > 0)
                            newTileset.Tiles[id] = props;
                    }
                }
                tilesets.Add(newTileset);
            }
            return tilesets;
        }

        private int getTilesetIndexFromGid(IList<ITileset> tilesets, int gid, out Rectangle spriteRect,
            out TileProperties properties)
        {
            for (int i = 0; i < tilesets.Count; ++i)
            {
                var tileset = tilesets[i];
                if (gid >= tileset.FirstGid && gid < tileset.FirstGid + tileset.TileCount)
                {
                    int interalId = gid - tileset.FirstGid;
                    int x = interalId % tileset.Columns;
                    int y = interalId / tileset.Columns;
                    spriteRect = new Rectangle(x * tileset.TileWidth, y * tileset.TileHeight, tileset.TileWidth,
                        tileset.TileHeight);
                    properties = tileset.Tiles[interalId];
                    return i;
                }
            }
            spriteRect = Rectangle.Empty;
            properties = null;

            return -1;
        }

        private void CreateTiles(ILevel data, XmlDocument doc, int width, int height, IList<ITileset> tilesets)
        {
            foreach (XmlNode layerNode in doc.DocumentElement.SelectNodes("layer"))
            {
                var layerWidth = int.Parse(layerNode.Attributes["width"].Value);
                var layerHeight = int.Parse(layerNode.Attributes["height"].Value);
                int count = -1;
                foreach (XmlNode tileNode in layerNode.SelectSingleNode("data").SelectNodes("tile"))
                {
                    ++count;
                    var gid = int.Parse(tileNode.Attributes["gid"].Value);
                    Rectangle bounds;
                    TileProperties prop;
                    int tilesetIndex = getTilesetIndexFromGid(tilesets, gid, out bounds, out prop);
                    if (tilesetIndex == -1)
                        continue;

                    int levelCol = count % layerWidth;
                    int levelRow = count / layerWidth;
                    if (levelCol >= width || levelRow >= height) //Out of bounds -> skip
                        continue;

                    var tile = new Tile(levelCol, levelRow, tilesetIndex, bounds);
                    data.Tiles[levelCol, levelRow] = tile;

                    Slope slope = Slope.None;
                    if (prop != null && prop.TryGetValue("slope", out var slopeString))
                    {
                        slope = (Slope) int.Parse(slopeString) + 1;
                        tile.Slope = slope;
                    }

                }
            }
        }

        private void CreateObjects(ILevel data, XmlDocument doc)
        {
            var objectGroup = doc?.DocumentElement?.SelectSingleNode("objectgroup[@name='objects']");
            if (objectGroup == null)
                return;

            var spawnList = new List<Point>();
            foreach (XmlNode objectsNode in objectGroup.SelectNodes("object"))
            {
                var x = int.Parse(objectsNode.Attributes["x"].Value);
                var y = int.Parse(objectsNode.Attributes["y"].Value);
                int width = 0;
                int height = 0;
                if(objectsNode.Attributes["width"] != null) 
                    width = int.Parse(objectsNode.Attributes["width"].Value);
                if (objectsNode.Attributes["height"] != null)
                    height = int.Parse(objectsNode.Attributes["height"].Value);
                var name = objectsNode.Attributes["name"].Value.ToLower();
                if (name == "death")
                {
                    data.Deaths.Add(new Rectangle(x - 1, y - 1, width + 1, height + 1));
                }
                else if (name == "start")
                {
                    data.Start = new Point(x, y);
                }
                else if (name == "checkpoint")
                {
                    data.Checkpoints.Add(new Checkpoint() {Rectangle = new Rectangle(x, y, width, height)});
                }
                else if (name == "spawn")
                {
                    spawnList.Add(new Point(x, y));
                }
                else if (name == "goal")
                {
                    data.Goal = new Rectangle(x, y, width, height);
                }
            }
            //Resolve spawns to their respective checkpoints
            foreach(var checkpoint in data.Checkpoints)
            {
                Point point = spawnList.FirstOrDefault(p => pointWithinRectangle(checkpoint.Rectangle, p));
                if (point == Point.Zero)
                    throw new Exception("No spawn set for checkpoint");

                checkpoint.Spawn = point;
            }
        }

        private bool pointWithinRectangle(Rectangle rect, Point p)
        {
            return p.X >= rect.X && p.Y >= rect.Y && p.X <= rect.X + rect.Width && p.Y <= rect.Y + rect.Height;
        }

        private Line[] getCollisionPolygons(ILevel level)
        {
            var collisionGenerator = new CollisionPolygonGenerator();
            return collisionGenerator.GetCollisionPolygons(level);
        }

    }
}
