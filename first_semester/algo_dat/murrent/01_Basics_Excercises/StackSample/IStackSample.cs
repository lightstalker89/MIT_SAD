namespace StackSample
{
    public interface IStackSample<T>
    {
        /// <summary>
        /// Adds an object to the end of the stack
        /// </summary>
        /// <param name="stackItem">
        /// Type: T
        /// The object to add to the end of the stack
        /// </param>
        /// <exception cref="StackSample.StackOutOfSpaceException">
        /// </exception>
        void Push(T stackItem);

        /// <summary>
        /// Remove and return the object at the beginning of the concurrent stack
        /// </summary>
        /// <returns>Type: T
        /// The first item of the stack</returns>
        /// <exception cref="StackSample.StackIsEmptyException"></exception>
        T Pop();

        /// <summary>
        /// Pick the first item out of the stack without deleting it
        /// </summary>
        /// <returns>The first item of the stack</returns>
        T Peek();

        /// <summary>
        /// Gets or sets the maximum stack items.
        /// </summary>
        /// <value>
        /// The maximum stack items.
        /// </value>
        int MaxStackItems { get; set; }
    }
}