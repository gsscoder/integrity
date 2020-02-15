namespace Integrity
{
    public abstract class Application
    {
        protected Paths Paths => new Paths();

        protected Hosts Hosts => new Hosts();
    }
}