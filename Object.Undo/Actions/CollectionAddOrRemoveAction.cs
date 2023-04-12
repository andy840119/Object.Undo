// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Reflection;
using Object.Undo.Interfaces;

namespace Object.Undo.Actions;

/// <summary>
/// Use this class to add/remove the object without set the property.
/// </summary>
/// <typeparam name="TProperty"></typeparam>
public abstract class CollectionAddOrRemoveAction<TProperty> : PropertyChangeAction<IEnumerable<TProperty>>
{
    private readonly CollectionChangeActionType actionType;

    protected CollectionAddOrRemoveAction(IEditableObject editableObject, string propertyName, IEnumerable<TProperty> values, CollectionChangeActionType actionType)
        : base(editableObject, propertyName, values)
    {
        this.actionType = actionType;
    }

    protected sealed override void Undo(IEditableObject editableObject, PropertyInfo propertyInfo, IEnumerable<TProperty>? property)
    {
        if (propertyInfo.GetValue(editableObject) is not ICollection<TProperty> collection)
            throw new InvalidOperationException("Property from the getter should not be null.");

        if (property == null)
            throw new InvalidOperationException("Property should not be null.");

        switch (actionType)
        {
            case CollectionChangeActionType.Add:
                RemoveRange(collection, property);
                break;

            case CollectionChangeActionType.Remove:
                AddRange(collection, property);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected sealed override void Redo(IEditableObject editableObject, PropertyInfo propertyInfo, IEnumerable<TProperty>? property)
    {
        if (propertyInfo.GetValue(editableObject) is not ICollection<TProperty> collection)
            throw new InvalidOperationException("Property from the getter should not be null.");

        if (property == null)
            throw new InvalidOperationException("Property should not be null.");

        switch (actionType)
        {
            case CollectionChangeActionType.Add:
                AddRange(collection, property);
                break;

            case CollectionChangeActionType.Remove:
                RemoveRange(collection, property);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// Should override this method if want to add the item with the right index.
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="items"></param>
    protected virtual void AddRange(ICollection<TProperty> collection, IEnumerable<TProperty> items)
    {
        foreach (var item in items)
        {
            collection.Add(item);
        }
    }

    /// <summary>
    /// Should override this method if want to add the item with the right index.
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="items"></param>
    protected virtual void RemoveRange(ICollection<TProperty> collection, IEnumerable<TProperty> items)
    {
        foreach (var item in items)
        {
            collection.Remove(item);
        }
    }
}

public enum CollectionChangeActionType
{
    Add,
    Remove,
}
