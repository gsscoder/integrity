using System.Linq;
using RailwaySharp;

namespace Integrity
{
    public static class ApplicationExtensions
    {
        public static ProvingContext ToProvingContext(this Application application)
        {
            var paths = new PathExistence(
                (from @this in application.PropertiesOf<Paths>() select @this.Content).SelectMany(p => p));
            var hosts = new HostAvailability(
                (from @this in application.PropertiesOf<Hosts>() select @this.Content).SelectMany(p => p));
            return new ProvingContext(paths, hosts);
        }
    }
}