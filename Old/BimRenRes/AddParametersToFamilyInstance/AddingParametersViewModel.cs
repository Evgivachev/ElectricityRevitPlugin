using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Autodesk.Revit.DB;
using BimRenRes.Annotations;
using MessageBox = System.Windows.Forms.MessageBox;

namespace BimRenRes.AddParametersToFamilyInstance;

public class AddingParametersViewModel : INotifyPropertyChanged
{
    private Document _doc;
    public List<CheckableItem> Parameters { get; } = new List<CheckableItem>();
    private readonly GroupOfParameters[] _parameterGroups = Enum.GetValues(typeof(BuiltInParameterGroup))
        .Cast<BuiltInParameterGroup>()
        .Select(x => new GroupOfParameters((int)x, LabelUtils.GetLabelFor(x)))
        .OrderBy(x => x.Name)
        .ToArray();
    public GroupOfParameters[] ParameterGroups => _parameterGroups;
    public GroupOfParameters SelectedGroup { get; set; }
    public bool IsTypeParameter { get; set; } = true;
    private DefinitionFile SharedParameterFile
    {
        get
        {
                try
                {
                    return _doc.Application.OpenSharedParameterFile();

                }
                catch (Exception)
                {
                    throw new ArgumentException("Не удалось открыть файл общих параметров");
                }
            }
    }

    public ICommand AddParametersCommand { get; } 
    public AddingParametersViewModel(Document document)
    {
            _doc = document;
            var groups = SharedParameterFile.Groups;
            foreach (var group in groups.OrderBy(x => x.Name))
            {
                var grCheckableItem = new CheckableItem();

                grCheckableItem.Name = group.Name;
                grCheckableItem.Item = group;
                foreach (var definition in group.Definitions)
                {
                    var p = new CheckableItem();
                    p.Name = definition.Name;
                    p.Item = definition;
                    grCheckableItem.Children.Add(p);
                }
                if (grCheckableItem.Children.Count > 0)
                    Parameters.Add(grCheckableItem);
            }
            AddParametersCommand =  new RelayCommand(o=>
            {
                AddButton_Click();

                if(!(o is Window window))
                    throw new NullReferenceException();
                window.DialogResult = true;
                window.Close();
            }, (o => true));
            SelectedGroup = ParameterGroups.First(x => x.Id == (int) BuiltInParameterGroup.INVALID);
        }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    private void AddButton_Click()
    {
            var selectedParameters = Parameters
                .SelectMany(i => i.Children)
                    .Where(cb => cb.IsChecked ==true)
                    .Select(x => (ExternalDefinition)x.Item);
            var parameterSetter = new ParameterSetter();
            parameterSetter.AddParameters(_doc, selectedParameters,(BuiltInParameterGroup)SelectedGroup.Id, !IsTypeParameter);

            var notAddedParameters = parameterSetter.GetErrorMessage();
            if (!string.IsNullOrEmpty(notAddedParameters)) MessageBox.Show(notAddedParameters);
        }
}