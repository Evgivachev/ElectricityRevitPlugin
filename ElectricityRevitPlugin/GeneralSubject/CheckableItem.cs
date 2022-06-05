namespace ElectricityRevitPlugin.GeneralSubject
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using MoreLinq.Extensions;

    public class CheckableItem : INotifyPropertyChanged
    {
        private readonly CheckableItem _parent;
        private bool? _isChecked = false;

        public CheckableItem(CheckableItem parent = null)
        {
            _parent = parent;
        }

        public object Item { get; set; }
        public string Name { get; set; }
        public ObservableCollection<CheckableItem> Children { get; } = new ObservableCollection<CheckableItem>();
        bool IsInitiallySelected { get; }

        public bool? IsChecked
        {
            get { return _isChecked; }
            set { SetIsChecked(value, true, true); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == _isChecked)
                return;
            _isChecked = value;
            if (updateChildren && _isChecked.HasValue)
                Children.ForEach(c => c.SetIsChecked(_isChecked, true, false));
            if (updateParent)
                _parent?.VerifyCheckState();
            OnPropertyChanged("IsChecked");
        }

        void VerifyCheckState()
        {
            bool? state = null;
            for (int i = 0; i < Children.Count; ++i)
            {
                bool? current = Children[i].IsChecked;
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

            SetIsChecked(state, false, true);
        }

        public IEnumerable<CheckableItem> GetSelectedCheckableItems()
        {
            var queue = new Queue<CheckableItem>();
            queue.Enqueue(this);
            if (IsChecked == true)
                yield return this;
            while (queue.Count > 0)
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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
