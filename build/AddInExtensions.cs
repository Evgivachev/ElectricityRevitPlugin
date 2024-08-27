namespace DefaultNamespace;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using RxBim.Nuke.Helpers;
using RxBim.Nuke.Revit.Generators.Models;

static class AddInExtensions
{
    /// <summary>
    /// Maps an <see cref="T:RxBim.Nuke.Revit.Generators.Models.AddIn" /> to the <see cref="T:System.Xml.Linq.XElement" />.
    /// </summary>
    /// <param name="addIn">An addin.</param>
    /// <returns>The <see cref="T:System.Xml.Linq.XElement" /> mapped from <paramref name="addIn" />.</returns>
    public static XElement ToXElement(this AddIn addIn) =>
        new((XName)"AddIn", new object[7]
        {
            (object)new XAttribute((XName)"Type", (object)addIn.Type),
            (object)new XElement((XName)"Assembly", (object)addIn.Assembly),
            (object)new XElement((XName)"Name", (object)addIn.Name),
            (object)new XElement((XName)"VendorDescription", (object)addIn.VendorDescription),
            (object)new XElement((XName)"VendorId", (object)addIn.VendorId),
            (object)new XElement((XName)"AddInId", (object)addIn.AddInId),
            (object)new XElement((XName)"FullClassName", (object)addIn.FullClassName)
        });

    /// <summary>
    /// Maps a <see cref="T:RxBim.Nuke.Revit.Generators.Models.RevitAddIns" /> to the <see cref="T:System.Xml.Linq.XDocument" />.
    /// </summary>
    /// <param name="revitAddIns">RevitAddIns.</param>
    /// <returns>The <see cref="T:System.Xml.Linq.XDocument" /> mapped from <paramref name="revitAddIns" />.</returns>
    public static XDocument ToXDocument(this RevitAddIns revitAddIns) =>
        new(new object[1]
        {
            (object)new XElement((XName)"RevitAddIns",
                (object)revitAddIns.AddIn.Ensure<List<AddIn>>("revitAddIns.AddIn")
                    .Select<AddIn, XElement>((Func<AddIn, XElement>)(x => x.ToXElement())))
        });
}
