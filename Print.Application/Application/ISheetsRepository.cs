namespace Print.Application.Application;

using Domain;

public interface ISheetsRepository
{
    Task<IEnumerable<Sheet>> GetSheets();
}