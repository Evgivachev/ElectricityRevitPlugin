namespace CommonUtils;

using System;
using Autodesk.Revit.DB;

public abstract class UpdaterBase : IUpdater
{
    private readonly AddInId _addInId;
    protected UpdaterBase(AddInId addInId)
    {
        _addInId = addInId;
    }
    protected abstract Guid Guid { get; }

    protected abstract ElementFilter ElementFilter { get; }

    protected abstract ChangeType ChangeType { get; }

    public abstract void Execute(UpdaterData data);

    public UpdaterId GetUpdaterId() => new(_addInId, Guid);

    public abstract ChangePriority GetChangePriority();

    public abstract string GetUpdaterName();

    public abstract string GetAdditionalInformation();

    public abstract bool IsOptional { get; }
}
