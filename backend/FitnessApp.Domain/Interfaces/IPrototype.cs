using System;

namespace FitnessApp.Domain.Interfaces;

/// <summary>
/// Generic Prototype interface for cloneable objects
/// </summary>
/// <typeparam name="T">Type of object to clone</typeparam>
public interface IPrototype<T> where T : class
{
    /// <summary>
    /// Creates a deep copy of the object
    /// </summary>
    /// <returns>Cloned instance</returns>
    T Clone();
}