using System;

namespace Zuehlke.Kicker.Core.Services
{
    public class KickerState
    {
        public KickerState(bool free, DateTime since)
        {
            Free = free;
            Since = since;
        }

        public bool Free { get; private set; }
        public DateTime Since { get; private set; } 
    }
}