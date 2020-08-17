using LibVLCSharp.Shared;

namespace JukeCore
{
    public class MediaFactory : IMediaFactory
    {
        private readonly LibVLC _libVlc;

        public MediaFactory(LibVLC libVlc)
        {
            _libVlc = libVlc;
        }

        public Media CreateFromPath(string path)
        {
            return new Media(_libVlc, path);
        }
    }
}