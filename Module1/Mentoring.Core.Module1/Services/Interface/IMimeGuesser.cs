using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mentoring.Core.Module1.Services.Interface
{
    public interface IMimeGuesser
    {
        string GuessMimeType(byte[] content);
    }
}
