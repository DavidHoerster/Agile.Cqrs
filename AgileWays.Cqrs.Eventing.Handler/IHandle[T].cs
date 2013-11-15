﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileWays.Cqrs.Eventing.Handler
{
    public interface IHandle<T> where T : EventBase
    {
        void Handle(T theEvent);
    }
}
