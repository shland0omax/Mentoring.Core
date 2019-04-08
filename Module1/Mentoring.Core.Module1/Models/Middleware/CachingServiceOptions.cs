using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mentoring.Core.Module1.Models.Middleware
{
    public class CachingServiceOptions
    {
        public string DirectoryPath { get; set; }
        public int MaxImagesCached { get; set; }
        public TimeSpan ExpirationTime { get; set; }
    }
}
