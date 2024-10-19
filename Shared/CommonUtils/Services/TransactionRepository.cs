namespace CommonUtils.Services;

using Autodesk.Revit.DB;
using Models;

public class TransactionRepository : ITransactionRepository
{
    private readonly Document _document;

    public TransactionRepository(Document document)
    {
        _document = document;
    }

    public ITransaction StartTransaction(string? name = null)
    {
        var tr = string.IsNullOrEmpty(name) 
        ? new Transaction(_document)
            : new Transaction(_document, name);
        tr.Start();
        return new TransactionWrapper(tr);
    }
}
