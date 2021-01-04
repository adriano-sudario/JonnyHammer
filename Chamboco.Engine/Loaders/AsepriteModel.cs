using System.Collections.Generic;

namespace Chamboco.Engine.Helpers
{
    internal record AsepriteModel
    {
        internal record SpriteSize (int X, int Y, int W, int H);
        internal record Size (int W, int H);
        internal record FrameTag (string Name, int From, int To, string Direction);
        internal record Layer (string Name, int Opacity, string BlendMode);
        internal record FrameData (
            SpriteSize Frame, bool Rotated, bool Trimmed, SpriteSize SpriteSourceSize, Size SourceSize, int Duration);

        internal record MetaData (
            string App,
            string Version,
            string Image,
            string Format,
            Size Size,
            string Scale,
            IEnumerable<FrameTag> FrameTags,
            IEnumerable<Layer> Layers);

        public Dictionary<string, FrameData> Frames { get; set; }
        public MetaData Meta { get; set; }

    }
}
