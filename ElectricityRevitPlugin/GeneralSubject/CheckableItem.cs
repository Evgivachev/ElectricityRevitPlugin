using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ElectricityRevitPlugin.Annotations;
using MoreLinq.Extensions;

namespace ElectricityRevitPlugin.GeneralSubject
{
    public class CheckableItem : INotifyPropertyChanged
    {
        public CheckableItem(CheckableItem parent = null)
        {
            _parent = parent;
        }
        public object Item { get; set; }
        public string Name { get; set; }

        private readonly CheckableItem _parent;

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<CheckableItem> Children { get; } = new ObservableCollection<CheckableItem>();
        bool IsInitiallySelected { get; }
        private bool? _isChecked = false;
        public bool? IsChecked
        {
            get { return _isChecked; }
            set { this.SetIsChecked(value, true, true); }
        }
        void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == _isChecked)
                return;

            _isChecked = value;

            if (updateChildren && _isChecked.HasValue)
                this.Children.ForEach(c => c.SetIsChecked(_isChecked, true, false));

            if (updateParent)
                _parent?.VerifyCheckState();

            this.OnPropertyChanged("IsChecked");
        }
        void VerifyCheckState()
        {
            bool? state = null;
            for (int i = 0; i < this.Children.Count; ++i)
            {
                bool? current = this.Children[i].IsChecked;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }
            this.SetIsChecked(state, false, true);
        }

        public IEnumerable<CheckableItem> GetSelectedCheckableItems()
        {
            var queue = new Queue<CheckableItem>();
            queue.Enqueue(this);
            if(IsChecked==true)
                yield return this;
            
            while (queue.Count>0)
            {
                var current = queue.Dequeue();

                foreach (var child in current.Children)
                {
                    if (child.IsChecked == true)
                        yield return child;
                    queue.Enqueue(child);
                }
            }
        }
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
