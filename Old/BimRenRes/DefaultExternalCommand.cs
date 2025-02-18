﻿using System;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BimRenRes;

[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
public abstract class DefaultExternalCommand : IExternalCommand
{
    protected UIApplication UiApp;
    protected UIDocument UiDoc;
    protected Document Doc;
    protected Application App;

    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
            if (commandData != null)
            {
                UiApp = commandData.Application;
                UiDoc = UiApp.ActiveUIDocument;
                Doc = UiDoc.Document;
                App = UiApp.Application;
            }
            Result result;
            try
            {
                result = DoWork(ref message, elements);
            }
            catch (Exception e)
            {
                message += e.Message + '\n' + e.StackTrace;
                result = Result.Failed;
            }
            return result;
        }
    protected abstract Result DoWork(ref string message, ElementSet elements);
}