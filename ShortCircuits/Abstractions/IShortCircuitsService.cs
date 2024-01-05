namespace ShortCircuits.Abstractions;

using Autodesk.Revit.DB;

///<summary>
/// Сервис для расчета токов короткого замыкания
/// </summary>
public interface IShortCircuitsService
{
    ///<summary>
    /// Выполняет расчет токов короткого замыкания в документе.
    /// </summary>
    void Calculate(Document document);
}