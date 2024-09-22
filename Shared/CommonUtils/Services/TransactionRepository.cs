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
        if (name is null)
            return new TransactionWrapper(_document);
        return new TransactionWrapper(_document, name);
    }
}
