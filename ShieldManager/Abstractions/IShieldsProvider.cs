namespace ShieldManager.Abstractions;

using System.Collections.Generic;
using Models;

public interface IShieldsProvider
{
    public IEnumerable<ShieldWrapper> GetShields();
}