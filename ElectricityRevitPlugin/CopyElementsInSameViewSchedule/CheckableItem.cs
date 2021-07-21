using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using ElectricityRevitPlugin.Annotations;

namespace ElectricityRevitPlugin.CopyElementsInSameViewSchedule
{
    public class CheckableItem: INotifyPropertyChanged

    {
        public string Name { get; }

        public bool IsChecked { get; set; } = false;

        public Element Element { get; private set; }

        public CheckableItem(Element element)
        {
            Name = element.Name;
            Element = element;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
