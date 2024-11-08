using System;
using System.Xml.Linq;
using RxBim.Nuke.Revit.Generators.Models;

public static class Extensions
{
    public static XDocument ToXDocument(this RevitAddIns addins)
    {
        // конвертация в xml
        var doc  = new XDocument();
        var revitAddIns = new XElement("RevitAddIns");
        doc.Add(revitAddIns);
        Console.WriteLine("ToXDocument");
        Console.WriteLine((addins.AddIn ?? []).Count);
        foreach (var addIn in addins.AddIn ?? [])
        {
            addIn.VendorId = "Eugene Ivachev";
            addIn.VendorDescription = "https://github.com/Evgivachev/ElectricityRevitPlugin";
            var xAddIn = new XElement("AddIn");
            xAddIn.SetAttributeValue("Type", addIn.Type);
            var xAssembly = new XElement("Assembly");
            xAssembly.Value = addIn.Assembly;
            xAddIn.Add(xAssembly);
            revitAddIns.Add(xAddIn);
            var xName = new XElement("Name");
            xName.Value = addIn.Name;
            xAddIn.Add(xName);
            var xAddInId = new XElement("AddInId");
            xAddInId.Value = addIn.AddInId;
            xAddIn.Add(xAddInId);
            var xFullClassName = new XElement("FullClassName");
            xFullClassName.Value = addIn.FullClassName;
            xAddIn.Add(xFullClassName);
            var xVendorId = new XElement("VendorId");
            xVendorId.Value = addIn.VendorId;
            xAddIn.Add(xVendorId);
            var xVendorDescription = new XElement("VendorDescription");
            xVendorDescription.Value = addIn.VendorDescription;
            xAddIn.Add(xVendorDescription);
        }
        return doc;
    }
}