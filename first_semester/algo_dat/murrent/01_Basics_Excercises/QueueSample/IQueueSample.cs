// *******************************************************
// * <copyright file="IQueueSample.cs" company="MDMCoWorks">
// * Copyright (c) 2014 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace QueueSample
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface IQueueSample<T>
    {
        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        T Peek();

        /// <summary>
        /// </summary>
        /// <param name="queueItem">
        /// </param>
        void Enqueue(T queueItem);

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        T Dequeue();

        /// <summary>
        /// </summary>
        int MaxQueueItems { get; set; }
    }
}