namespace CommonUtils.Models;

using Autodesk.Revit.DB;
using Services;

public class TransactionWrapper : ITransaction
{
    private readonly Transaction _transaction;

    public TransactionWrapper(Document document)
    {
        _transaction = new Transaction(document);
    }

    public TransactionWrapper(Document document, string name)
    {
        _transaction = new Transaction(document, name);
    }

    public void Dispose()
    {
        _transaction.Dispose();
    }

    public string Name
    {
        get => _transaction.GetName();
        set => _transaction.SetName(value);
    }

    public void Commit()
    {
        _transaction.Commit();
    }

    public void Rollback()
    {
        _transaction.RollBack();
    }
}
