using LibVLCSharp.Shared;

namespace JukeCore
{
    public interface IMediaFactory
    {
        Media CreateFromPath(string path);
    }
}