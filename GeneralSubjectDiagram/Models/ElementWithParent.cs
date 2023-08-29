namespace GeneralSubjectDiagram.Models;

public class ElementWithParent<T>
{
    public ElementWithParent(T? parent, T element)
    {
        Parent = parent;
        Element = element;
    }

    public T Element;

    public T? Parent;
}
