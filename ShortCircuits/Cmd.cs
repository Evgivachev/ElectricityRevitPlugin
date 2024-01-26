using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using CommonUtils;
using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using ShortCircuits.Abstractions;
using ShortCircuits.Services;

namespace ShortCircuits
{
    using System;

    /// <inheritdoc />
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd : CmdBase
    {
        /// <inheritdoc />
        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IShortCircuitsService, ShortCircuitsService>();
        }

        /// <inheritdoc />
        protected override Result Execute(IServiceProvider serviceProvider)
        {
            var doc = serviceProvider.GetService<Document>();
            serviceProvider.GetService<IShortCircuitsService>().Calculate(doc);
            return Result.Succeeded;
        }
    }
}