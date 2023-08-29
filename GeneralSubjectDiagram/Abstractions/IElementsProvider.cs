namespace GeneralSubjectDiagram.Abstractions;

using System.Collections.Generic;

public interface IElementsProvider
{
    /// <summary>
    /// Возвращает элементы
    /// </summary>
    /// <param name="familyName"></param>
    /// <returns></returns>
    IEnumerable<object> GetElements(string familyName);
    
}
