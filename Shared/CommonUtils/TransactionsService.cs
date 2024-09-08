namespace CommonUtils;

using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

public class TransactionsService : ITransactionsService, IDisposable
{
    private readonly UIApplication _application;
    private Transaction? _transaction;
    public TransactionsService(UIApplication application)
    {
        _application = application;
    }

    public IDisposable StartTransaction(string name)
    {
        if (_transaction != null)
        {
            throw new InvalidOperationException("Transaction already started");
        }
        
        var doc = _application.ActiveUIDocument.Document;
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
