// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using Object.Undo.Interfaces;

namespace Object.Undo.Actions;

/// <summary>
/// Use this property to add or remove the item in the set with re-assign the property.
/// </summary>
/// <typeparam name="TProperty"></typeparam>
public class SetChangeAction<TProperty> : CollectionAddOrRemoveAction<TProperty>
{

    public SetChangeAction(IEditableObject editableObject, string propertyName, TProperty value, CollectionChangeActionType actionType)
        : this(editableObject, propertyName, new [] {value }, actionType)
    {
    }

    public SetChangeAction(IEditableObject editableObject, string propertyName, IEnumerable<TProperty> values, CollectionChangeActionType actionType)
        : base(editableObject, propertyName, values, actionType)
    {
    }
}

