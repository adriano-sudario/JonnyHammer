using System.Collections.Generic;

namespace Chamboco.Engine.Helpers
{
    public class AsepriteModel
    {
        public class SpriteSize  {
            public int X { get; set; }
            public int Y { get; set; }
            public int W { get; set; }
            public int H { get; set; }
        }

        public class Size    {
            public int W { get; set; }
            public int H { get; set; }
        }

        public class FrameTag    {
            public string Name { get; set; }
            public int From { get; set; }
            public int To { get; set; }
            public string Direction { get; set; }
        }
        public class Layer    {
            public string Name { get; set; }
            public int Opacity { get; set; }
            public string BlendMode { get; set; }
        }

        public class FrameData    {
            public SpriteSize Frame { get; set; }
            public bool Rotated { get; set; }
            public bool Trimmed { get; set; }
            public SpriteSize SpriteSourceSize { get; set; }
            public Size SourceSize { get; set; }
            public int Duration { get; set; }
        }

        public class MetaData    {
            public string App { get; set; }
            public string Version { get; set; }
            public string Image { get; set; }
            public string Format { get; set; }
            public Size Size { get; set; }
            public string Scale { get; set; }
            public List<FrameTag> FrameTags { get; set; }
            public List<Layer> Layers { get; set; }
        }

        public Dictionary<string, FrameData> Frames { get; set; }
        public MetaData Meta { get; set; }

    }
}
