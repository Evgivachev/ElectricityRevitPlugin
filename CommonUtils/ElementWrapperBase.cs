namespace CommonUtils;

using Autodesk.Revit.DB;
using RxBim.Tools;

/// <summary> Базовый класс Wrapper'а </summary>
/// <typeparam name="T">Тип</typeparam>
public abstract class ElementWrapperBase<T> : Wrapper<T>
    where T : Element
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ElementWrapperBase{T}"/> class.
    /// </summary>
    /// <param name="element">Элемент.</param>
    protected ElementWrapperBase(T element)
        : base(element)
    {
        TypeName = Object.Document.GetElement(Object.GetTypeId())?.Name ?? string.Empty;
    }

    /// <summary>
    /// Id
    /// </summary>
    public int Id => Object.IsValidObject ? Object.Id.IntegerValue : -1;

    /// <summary>
    /// Имя типа
    /// </summary>
    public string TypeName { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return Object.ToString();
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Object.IsValidObject ? Object.Id.IntegerValue.GetHashCode() : -1;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        var wrapper = obj as ElementWrapperBase<T>;
        var element = wrapper?.Object;
        if (element is null)
            return false;
        var elementId = element.Id.IntegerValue;
        return Object.IsValidObject && Object.Id.IntegerValue.Equals(elementId);
    }
}
