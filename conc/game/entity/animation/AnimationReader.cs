using System;
using System.Collections.Generic;
using System.Xml;

namespace conc.game.entity.animation
{
    public interface IAnimationReader
    {
        IAnimator GetAnimator(string name);
        void LoadAllTemplates();
    }

    public class AnimationReader : IAnimationReader
    {
        private readonly IDictionary<string, IAnimator> _animationGroups = new Dictionary<string, IAnimator>();
        
        public void LoadAllTemplates()
        {
            foreach (var resourceName in GetType().Assembly.GetManifestResourceNames())
            {
                var fileIdx = resourceName.LastIndexOf('.');
                var file = resourceName.Substring(0, fileIdx);

                var folderIdx = file.LastIndexOf('.');
                var folder = file.Substring(0, folderIdx);

                if (folder != "conc.game.entity.animation.templates")
                    continue;

                using (var stream = GetType().Assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                        return;

                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(stream);

                    if (xmlDoc.DocumentElement == null)
                        return;

                    foreach (XmlNode animationGroupNode in xmlDoc.DocumentElement.ChildNodes)
                    {
                        if (animationGroupNode.Attributes == null)
                            continue;

                        var animationGroup = new Animator
                        {
                            Name = animationGroupNode.Attributes["Name"].Value,
                            SpriteSheet = animationGroupNode.Attributes["Spritesheet"].Value,
                            FrameWidth = int.Parse(animationGroupNode.Attributes["FrameWidth"].Value),
                            FrameHeight = int.Parse(animationGroupNode.Attributes["FrameHeight"].Value)
                        };

                        foreach (XmlNode animationNode in animationGroupNode.ChildNodes)
                        {
                            if (animationNode.Attributes == null)
                                continue;

                            var animation = new Animation
                            {
                                Type = (AnimationType)Enum.Parse(typeof(AnimationType), animationNode.Attributes["Type"].Value)
                            };

                            var frames = new List<IAnimationFrame>();
                            foreach (XmlNode frameNode in animationNode.ChildNodes)
                            {
                                if (frameNode.Attributes == null)
                                    continue;

                                var frame = new AnimationFrame
                                {
                                    X = int.Parse(frameNode.Attributes["X"].Value),
                                    Y = int.Parse(frameNode.Attributes["Y"].Value)
                                };

                                frames.Add(frame);
                            }

                            animation.Frames = frames.ToArray();
                            animationGroup.Animations[animation.Type] = animation;
                        }

                        _animationGroups[animationGroup.Name] = animationGroup;
                    }
                }
            }
        }

        public IAnimator GetAnimator(string name)
        {
            return _animationGroups[name];
        }
    }
}
