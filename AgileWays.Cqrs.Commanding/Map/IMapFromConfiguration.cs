using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileWays.Cqrs.Commanding.Map
{
    public interface IMapFromConfiguration
    {
        void UsingSectionName(string sectionName);
        void UsingDefaultSection();
    }
}
