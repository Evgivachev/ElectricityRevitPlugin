namespace CommonUtils
{
    using Autodesk.Revit.DB;

    /// <summary> Базовый класс Wrapper'а </summary>
    /// <typeparam name="T">Тип</typeparam>
    public abstract class ElementWrapperBase<T>
        where T : Element
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementWrapperBase{T}"/> class.
        /// </summary>
        /// <param name="initialInstance">Элемент.</param>
        protected ElementWrapperBase(T initialInstance)
        {
            InitialInstance = initialInstance;
        }

        /// <summary> Исходный элемент </summary>
        public T InitialInstance { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return InitialInstance.ToString();
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return InitialInstance.IsValidObject ? InitialInstance.Id.IntegerValue.GetHashCode() : -1;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            var wrapper = obj as ElementWrapperBase<T>;
            var element = wrapper?.InitialInstance;
            if (element is null)
                return false;
            var elementId = element.Id.IntegerValue;
            return InitialInstance.Id.IntegerValue.Equals(elementId);
        }
    }
}
