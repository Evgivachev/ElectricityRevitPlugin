namespace ElectricityRevitPlugin.Application.Triggers;

using System;

public class UsedWithAttribute(string updaterName) : Attribute
{
    public string UpdaterName { get; } = updaterName;
}
