using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zuehlke.Kicker.Core.Services
{
    public interface IBackgroundService
    {
        void Start();
        void Stop();
    }
}
