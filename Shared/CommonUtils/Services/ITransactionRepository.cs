namespace CommonUtils.Services;

using Models;

public interface ITransactionRepository
{
    ITransaction StartTransaction(string? name = null);
}
