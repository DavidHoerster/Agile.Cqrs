using System;
namespace AgileWays.Cqrs.Commanding.Map
{
    public interface ICommandRouteMapper
    {
        IMapToAConstructor AddConstructorRoute();
        IMapToAnInstanceMethod AddMethodRoute();
        IMapFromConfiguration FromConfiguration();
    }
}
