namespace CableJournalCmd.Application;

using System.Collections.Generic;
using CommonUtils.Services;
using Domain;

public interface ICableRepository : ITransactionRepository
{
    IReadOnlyCollection<Cable> GetAll();

    void UpdateCables(IEnumerable<Cable> cables);
}
