namespace CommonUtils.Services;

public interface ITransactionRepository
{
    ITransaction StartTransaction(string? name = null);
}
