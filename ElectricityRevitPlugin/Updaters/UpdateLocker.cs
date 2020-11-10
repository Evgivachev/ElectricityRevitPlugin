using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace ElectricityRevitPlugin.Updaters
{
    public class UpdateLocker : IDisposable
    {
        private static UpdateLocker _updateLocker;

        public static UpdateLocker GetUpdateLocker()
        {
            if(_updateLocker is null)
                _updateLocker = new UpdateLocker();
            return _updateLocker;
        }
        public ICollection<Tuple<ElementId, ChangeType>> ElementsIds { get; set; }

        private UpdateLocker()
        {
            ElementsIds = new List<Tuple<ElementId, ChangeType>>();
        }

        public UpdateLocker Lock()
        {
            _isLocked = true;
            return this;
        }
        private bool _isLocked = false;
        public bool IsLocked(Document doc)
        {
            if (_isLocked)
                return true;
            if (null == ElementsIds)
                return false;
            return ElementsIds.Any(t =>
            {
                var id = t.Item1;
                var el = doc.GetElement(id);
                return el != null && el.IsValidObject;
            });
        }


        public void Dispose()
        {
            _isLocked = false;
            ElementsIds = null;
        }
    }
}
