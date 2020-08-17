using System.Collections.Generic;
using LibVLCSharp.Shared;

namespace JukeCore
{
    public interface IPlaylist
    {
        void Set(IEnumerable<Media> medias);
        bool AnyNext();
        Media Next();
        bool AnyPrevious();
        Media Previous();
    }
}