using Mentoring.Core.Module1.Services.Interface;

namespace Mentoring.Core.Module1.Services
{
    public class MimeGuesser : IMimeGuesser
    {
        public string GuessMimeType(byte[] content)
        {
            return HeyRed.Mime.MimeGuesser.GuessMimeType(content);
        }
    }
}
