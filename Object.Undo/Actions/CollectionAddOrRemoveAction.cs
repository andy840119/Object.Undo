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
/// <typeparam name="TValue"></typeparam>
public abstract class CollectionAddOrRemoveAction<TValue> : ChangeAction<IEnumerable<TValue>>
{
    private IEnumerable<TValue> values = Array.Empty<TValue>();
    private readonly CollectionChangeActionType actionType;

    protected CollectionAddOrRemoveAction(IEditableObject editableObject, string propertyName, TValue value, CollectionChangeActionType actionType)
        : this(editableObject, propertyName, new [] { value }, actionType)
    {
    }

    protected CollectionAddOrRemoveAction(IEditableObject editableObject, string propertyName, IEnumerable<TValue> values, CollectionChangeActionType actionType)
        : base(editableObject, propertyName, values)
    {
        this.actionType = actionType;
    }

    protected sealed override void BackupValue(IEnumerable<TValue> oldValue, IEnumerable<TValue> newValue)
    {
        values = newValue;
    }

    protected sealed override IEnumerable<TValue> GetUndoValue() => values;

    protected sealed override IEnumerable<TValue> GetRedoValue() => values;

    protected sealed override void Undo(IEditableObject editableObject, PropertyInfo propertyInfo, IEnumerable<TValue>? property)
    {
        if (propertyInfo.GetValue(editableObject) is not ICollection<TValue> collection)
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

    protected sealed override void Redo(IEditableObject editableObject, PropertyInfo propertyInfo, IEnumerable<TValue>? property)
    {
        if (propertyInfo.GetValue(editableObject) is not ICollection<TValue> collection)
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
    protected virtual void AddRange(ICollection<TValue> collection, IEnumerable<TValue> items)
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
    protected virtual void RemoveRange(ICollection<TValue> collection, IEnumerable<TValue> items)
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
