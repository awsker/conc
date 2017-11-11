using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace tile
{
    public static class LevelSerializer
    {
        public static void Serialize(IList<ILevel> levels)
        {
            foreach (var level in levels)
            {
                var serializer = new DataContractSerializer(typeof(Level), null, int.MaxValue, false, false, null, new LevelDataContractResolver());
                var ms = new MemoryStream();
                using (var writer = XmlDictionaryWriter.CreateBinaryWriter(ms))
                    serializer.WriteObject(writer, level);
                ms.Close();

                var stageBytes = ms.ToArray();

                var filePath = $@"..\..\content\output\{level.Name + ".bg"}";

                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    using (var bw = new BinaryWriter(fs))
                    {
                        bw.Write(stageBytes);
                    }
                }
            }
        }

        public static IList<ILevel> DeSerialize()
        {
            var levels = new List<ILevel>();
            var path = Path.GetFullPath(@"..\..\..\..\..\tiledgenerator\content\output\");
            foreach (var fileName in Directory.GetFiles(path))
            {
                if (!fileName.EndsWith(".bg"))
                    continue;

                using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (var br = new BinaryReader(fs))
                    {
                        var numBytes = fs.Length;
                        var bytes = br.ReadBytes((int)numBytes);

                        var serializer = new DataContractSerializer(typeof(Level), null, int.MaxValue, false, false, null, new LevelDataContractResolver());
                        using (var stream = new MemoryStream(bytes))
                        using (var reader = XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max))
                            levels.Add((ILevel) serializer.ReadObject(reader));
                    }
                }
            }

            return levels;
        }
    }

    public class LevelDataContractResolver : DataContractResolver
    {
        public override bool TryResolveType(Type type, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
        {
            if (type == typeof(Tile))
            {
                var dictionary = new XmlDictionary();
                typeName = dictionary.Add("Tile");
                typeNamespace = dictionary.Add("tile");
                return true;
            }
            
            if (type == typeof(Tileset))
            {
                var dictionary = new XmlDictionary();
                typeName = dictionary.Add("Tileset");
                typeNamespace = dictionary.Add("tile");
                return true;
            }

            return knownTypeResolver.TryResolveType(type, declaredType, knownTypeResolver, out typeName, out typeNamespace);
        }

        public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
        {
            if (typeName == "Tile" && typeNamespace == "tile")
                return typeof(Tile);
            
            if (typeName == "Tileset" && typeNamespace == "tile")
                return typeof(Tileset);

            return knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, knownTypeResolver);
        }
    }
}
