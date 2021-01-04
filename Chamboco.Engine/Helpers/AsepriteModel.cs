using System.Collections.Generic;

namespace Chamboco.Engine.Helpers
{
    public record AsepriteModel
    {
        public record SpriteSize (int X, int Y, int W, int H);
        public record Size (int W, int H);
        public record FrameTag (string Name, int From, int To, string Direction);
        public record Layer (string Name, int Opacity, string BlendMode);
        public record FrameData (
            SpriteSize Frame, bool Rotated, bool Trimmed, SpriteSize SpriteSourceSize, Size SourceSize, int Duration);

        public record MetaData (
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
