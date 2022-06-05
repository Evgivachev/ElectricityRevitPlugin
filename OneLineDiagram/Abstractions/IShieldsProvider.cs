namespace Diagrams.Abstractions
{
    using System.Collections.Generic;
    using Models;

    public interface IShieldsProvider
    {
        public IEnumerable<Shield> GetShields();
    }
}
