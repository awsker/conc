﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using tile;

namespace BGStageGenerator
{
    public interface ILevelGenerator
    {
        IList<ILevel> Generate();
    }

    public class LevelGenerator : ILevelGenerator
    {
        public IList<ILevel> Generate()
        {
            var levels = new List<ILevel>();

            foreach (var file in Directory.GetFiles(@"..\..\content"))
            {
                if (!file.EndsWith(".tmx"))
                    continue;

                Console.WriteLine("Generating stage for file {0}", file);

                using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    var doc = new XmlDocument();
                    doc.Load(stream);

                    if (doc.DocumentElement != null)
                    {
                        var stageName = Path.GetFileNameWithoutExtension(file);

                        var width = int.Parse(doc.DocumentElement.GetAttribute("width"));
                        var height = int.Parse(doc.DocumentElement.GetAttribute("height"));

                        var tileNode = doc.DocumentElement.SelectSingleNode("tileset");
                        var tilesetName = tileNode.Attributes["name"].Value;
                        var tileWidth = int.Parse(tileNode.Attributes["tilewidth"].Value);
                        var tileHeight = int.Parse(tileNode.Attributes["tileheight"].Value);

                        var imageNode = tileNode.SelectSingleNode("image");
                        var imageWidth = int.Parse(imageNode.Attributes["width"].Value);
                        var imageHeight = int.Parse(imageNode.Attributes["height"].Value);

                        var level = new Level
                        {
                            Tiles = new ITile[width, height],
                            Background = new ITile[width, height],
                            Foreground = new ITile[width, height],
                            Collisions = new ICollisionTile[width, height],
                            Deaths = new List<Rectangle>()
                        };

                        //level.StartPoint = new List<ISpawnPoint>();

                        CreateTiles(level, doc, imageWidth, tileWidth, tileHeight);
                        CreateObjects(level, doc, tileWidth, tileHeight);

                        level.Name = stageName;
                        level.Tileset = tilesetName;

                        levels.Add(level);
                    }
                }
            }

            return levels;
        }

        private void CreateTiles(ILevel data, XmlDocument doc, int tilesetWidth, int tilewidth, int tileheight)
        {
            var width = int.Parse(doc.DocumentElement.GetAttribute("width"));

            var tileLayer = doc.DocumentElement.SelectSingleNode("layer[@name='tiles']");
            var tiles = tileLayer.SelectSingleNode("data").SelectNodes("tile");

            var tilesetCols = tilesetWidth / tilewidth;

            var x = 0;
            var y = 0;

            for (int i = 0; i < tiles?.Count; i++)
            {
                var gid = int.Parse(tiles[i].Attributes["gid"].Value);
                if (gid != 0)
                {
                    var col = (gid - 1) % tilesetCols;
                    var row = (gid - 1) / tilesetCols;

                    data.Tiles[x, y] = new Tile(x, y, new Rectangle(col*tilewidth, row*tileheight, tilewidth, tileheight));
                    data.Collisions[x, y] = new CollisionTile(new Rectangle(x * tilewidth, y * tileheight, tilewidth, tileheight), CollisionType.Solid);
                }

                x++;
                if (x >= width)
                {
                    x = 0;
                    y++;
                }
            }

            for (y = 1; y < data.Collisions.GetLength(1)-1; y++)
            {
                for (x = 1; x < data.Collisions.GetLength(0)-1; x++)
                {
                    var tile = data.Collisions[x, y];
                    if (tile != null && tile.Type == CollisionType.Solid)
                    {
                        // bottom right edge of tile
                        if (data.Collisions[x + 1, y] == null && data.Collisions[x, y + 1] == null && data.Collisions[x + 1, y + 1] == null)
                            data.Collisions[x + 1, y + 1] = new CollisionTile(new Rectangle((x + 1)*tilewidth, (y + 1)*tileheight, tilewidth, tileheight), CollisionType.Edge);
                        // bottom left edge of tile
                        if (data.Collisions[x - 1, y] == null && data.Collisions[x, y + 1] == null && data.Collisions[x - 1, y + 1] == null)
                            data.Collisions[x - 1, y + 1] = new CollisionTile(new Rectangle((x - 1) * tilewidth, (y + 1) * tileheight, tilewidth, tileheight), CollisionType.Edge);
                        // top right edge of tile
                        if (data.Collisions[x + 1, y] == null && data.Collisions[x, y - 1] == null && data.Collisions[x + 1, y - 1] == null)
                            data.Collisions[x + 1, y - 1] = new CollisionTile(new Rectangle((x + 1) * tilewidth, (y - 1) * tileheight, tilewidth, tileheight), CollisionType.Edge);
                        // top left edge of tile
                        if (data.Collisions[x - 1, y] == null && data.Collisions[x, y - 1] == null && data.Collisions[x - 1, y - 1] == null)
                            data.Collisions[x - 1, y - 1] = new CollisionTile(new Rectangle((x - 1) * tilewidth, (y - 1) * tileheight, tilewidth, tileheight), CollisionType.Edge);
                    }
                }
            }
        }

        private void CreateObjects(ILevel data, XmlDocument doc, int tilewidth, int tileheight)
        {
            var objectGroup = doc?.DocumentElement?.SelectSingleNode("objectgroup[@name='Objects']");
            if (objectGroup == null)
                return;

            var objects = objectGroup.SelectNodes("object");

            for (int i = 0; i < objects?.Count; i++)
            {
                var x = int.Parse(objects[i].Attributes["x"].Value);
                var y = int.Parse(objects[i].Attributes["y"].Value);
                var width = int.Parse(objects[i].Attributes["width"].Value);
                var height = int.Parse(objects[i].Attributes["height"].Value);
                var name = objects[i].Attributes["name"].Value.ToLower();

                if (name == "death")
                {
                    var rect = new Rectangle(x-1, y-1, width+1, height+1);
                    data.Deaths.Add(rect);
                    continue;
                }

                for (var ny = 0; ny < height / tileheight; ny++)
                {
                    for (var nx = 0; nx < width / tilewidth; nx++)
                    {

                        if (name == "start")
                        {
                            var posX = x + width / 2f;
                            var posY = y + height + 1.9f;
                            data.Start = new Vector2(posX, posY);
                        }

                        if (name == "ladder")
                        {
                            var type = CollisionType.Ladder;
                            if (ny == 0 && nx == 0)
                                type = CollisionType.LadderTop;
                            if (ny == width / tilewidth - 1 && nx == height / tileheight - 1)
                                type = CollisionType.LadderBottom;

                            var collisionTile = new CollisionTile(new Rectangle(x + nx * tilewidth, y + ny * tileheight, tilewidth, tileheight), type);
                            data.Collisions[x / tilewidth + nx, y / tileheight + ny] = collisionTile;
                        }

                        if (name == "platform")
                        {
                            var collisionTile = new CollisionTile(new Rectangle(x + nx * tilewidth, y + ny * tileheight, tilewidth, tileheight), CollisionType.Platform);
                            data.Collisions[x / tilewidth + nx, y / tileheight + ny] = collisionTile;
                        }
                    }
                }
            }
        }
    }
}
