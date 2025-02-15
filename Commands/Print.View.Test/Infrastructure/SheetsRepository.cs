namespace Print.View.Test.Infrastructure;

using Application.Application;
using Application.Domain;

public class SheetsRepository : ISheetsRepository
{

    public Task<IEnumerable<Sheet>> GetSheets()
    {
        var rand = new Random();
        var list = new List<Sheet>();
        for (int i = 1; i < 15; i++)
        {
            var flag = i < 5;
            var sheet = new Sheet()
            {
                ParentId = flag ? null : rand.Next(1, i),
                Id = i,
                Name = $"Sheet {i}"
            };
            list.Add(sheet);
        }
        return Task.FromResult(list.AsEnumerable());
    }
}
