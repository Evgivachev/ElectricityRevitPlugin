namespace PrintCmd.Infrastructure;

using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using CommonUtils.Comparer;
using Print.Application.Application;
using Print.Application.Domain;

public class SheetRepository(RevitTask revitTask) : ISheetsRepository
{


    public async Task<IEnumerable<Sheet>> GetSheets()
    {
        return await revitTask.Run(uiApp =>
        {
            var doc = uiApp.ActiveUIDocument.Document;
            var br = BrowserOrganization.GetCurrentBrowserOrganizationForSheets(doc);
            var sheets = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Sheets)
                .OfClass(typeof(ViewSheet))
                .Cast<ViewSheet>()
                .Where(x => x.IsPlaceholder == false)
                .OrderBy(x => string.Join("", br.GetFolderItems(x.Id).Select(fi => fi.Name))
                )
                .ThenBy(x => x.SheetNumber, new RevitNameComparer());


            var groupOfSheet = new Dictionary<int, Sheet>();

            groupOfSheet[br.Id.IntegerValue] = new Sheet()
            {
                Id = br.Id.IntegerValue,
                Name = br.Name,
                ParentId = null
            };

            foreach (var sheet in sheets)
            {
                var folderItems = br.GetFolderItems(sheet.Id);
                var parentId = br.Id.IntegerValue;
                foreach (var currentFolder in folderItems)
                {
                    // currentFolder.ElementId.IntegerValue не уникален, может быть группа с таким же Id и другим именем
                    var id = - (currentFolder.ElementId.IntegerValue, currentFolder.Name).GetHashCode();

                    if (groupOfSheet.ContainsKey(id))
                    {
                        parentId = id;
                        continue;
                    }
                    groupOfSheet[id] = new Sheet()
                    {
                        Id = id,
                        Name = currentFolder.Name,
                        ParentId = parentId
                    };
                    parentId = id;

                }
                groupOfSheet[sheet.Id.IntegerValue] = new Sheet()
                {
                    Id = sheet.Id.IntegerValue,
                    Name = sheet.Name,
                    ParentId = parentId
                };

            }
            return groupOfSheet.Values;

        });
    }

}
