using System;
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

                        var tiles = tileNode.SelectNodes("tile");
                        var slopeDict = new Dictionary<int, int>();
                        if (tiles != null)
                        {
                            foreach (var tile in tiles)
                            {
                                var node = tile as XmlNode;
                                var tileId = node.Attributes["id"].Value;
                                var props = node.SelectSingleNode("properties");
                                var slopePropery = props.SelectSingleNode("property");
                                if (slopePropery.Attributes["name"].Value == "slope")
                                {
                                    var slopeValue = slopePropery.Attributes["value"].Value;
                                    slopeDict.Add(int.Parse(tileId) + 1, int.Parse(slopeValue));
                                }

                            }
                        }

                        var level = new Level
                        {
                            Tiles = new ITile[width, height],
                            Background = new ITile[width, height],
                            Foreground = new ITile[width, height],
                            Collisions = new ICollisionTile[width, height],
                            Deaths = new List<Rectangle>()
                        };

                        //level.StartPoint = new List<ISpawnPoint>();

                        CreateTiles(level, doc, imageWidth, tileWidth, tileHeight, slopeDict);
                        CreateObjects(level, doc, tileWidth, tileHeight);

                        level.Name = stageName;
                        level.Tileset = tilesetName;

                        levels.Add(level);
                    }
                }
            }

            return levels;
        }

        private void CreateTiles(ILevel data, XmlDocument doc, int tilesetWidth, int tilewidth, int tileheight, Dictionary<int, int> slopeDict)
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

                    var slope = Slope.None;
                    if (slopeDict.TryGetValue(gid, out var slopeId))
                    {
                        if (slopeId == 0)
                            slope = Slope.FloorDown;
                        else if (slopeId == 1)
                            slope = Slope.FloorUp;
                        else if (slopeId == 2)
                            slope = Slope.RoofDown;
                        else if (slopeId == 3)
                            slope = Slope.RoofUp;
                    }

                    data.Collisions[x, y] = new CollisionTile(new Rectangle(x * tilewidth, y * tileheight, tilewidth, tileheight), slope);
                }

                x++;
                if (x >= width)
                {
                    x = 0;
                    y++;
                }
            }
        }

        private void CreateObjects(ILevel data, XmlDocument doc, int tilewidth, int tileheight)
        {
            var objectGroup = doc?.DocumentElement?.SelectSingleNode("objectgroup[@name='objects']");
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
                            var posX = x;
                            var posY = y;
                            data.Start = new Vector2(posX, posY);
                        }
                    }
                }
            }
        }
    }
}
