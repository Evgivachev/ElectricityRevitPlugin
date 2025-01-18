namespace CommonUtils;

using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

public class TransactionsService(UIApplication application) : ITransactionsService, IDisposable
{
    private Transaction? _transaction;

    public IDisposable StartTransaction(string name)
    {
        if (_transaction != null)
        {
            return this;
        }
        
        var doc = application.ActiveUIDocument.Document;
        _transaction = new Transaction(doc, name);
        _transaction.Start();
        return this;
    }

    public void Commit()
    {
        _transaction?.Commit();
        _transaction = null;
    }

    public void Dispose()
    {
        if (_transaction != null && _transaction.GetStatus() == TransactionStatus.Started)
            _transaction.RollBack();
        _transaction?.Dispose();
    }
}
