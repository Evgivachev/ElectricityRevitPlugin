namespace CommonUtils;

using System;

public interface ITransactionsService
{
    IDisposable StartTransaction(string name);

    void Commit();
}
