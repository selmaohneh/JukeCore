using System.Collections.Generic;
using LibVLCSharp.Shared;

namespace JukeCore
{
    public interface IMediaFactory
    {
        IReadOnlyList<Media> Create(string id, string jukeCorePath);
    }
}