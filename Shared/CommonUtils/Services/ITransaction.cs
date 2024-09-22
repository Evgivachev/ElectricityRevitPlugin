namespace CommonUtils.Services;

using System;

public interface ITransaction : IDisposable
{
    string Name { get; set; }

    void Commit();

    void Rollback();
}
