namespace Diagrams.ViewContext
{
    using System.Collections.Generic;
    using PikTools.Ui.ViewModels;

    public class SelectableGroupModel<T, TInner> : SelectableViewModel<T>
    {
        private bool? _isChecked = false;

        public SelectableGroupModel(T value, bool isChecked)
            : base(value, isChecked)
        {
        }

        public List<SelectableViewModel<TInner>> InnerItems { get; set; } = new List<SelectableViewModel<TInner>>();

        public new bool? IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                RaisePropertyChanged(nameof(IsChecked));
            }
        }
    }
}
