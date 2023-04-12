// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Reflection;
using Object.Undo.Interfaces;

namespace Object.Undo.Actions;

public class PropertyChangeAction<TValue> : ChangeAction<TValue?>
{
    public TValue? OldValue { get; private set; }
    public TValue? NewValue { get; private set; }

    public PropertyChangeAction(IEditableObject editableObject, string propertyName, TValue? value)
        : base(editableObject, propertyName, value)
    {
        NewValue = value;
    }

    protected sealed override void BackupValue(TValue? oldValue, TValue? newValue)
    {
        OldValue = oldValue;
        NewValue = newValue;

        if (OldValue == null && NewValue == null)
            throw new InvalidOperationException("Old value and new value should not be null at the same time.");

        if (OldValue?.Equals(NewValue) ?? false)
            throw new InvalidOperationException("Old value should not same as new value.");
    }

    protected sealed override TValue? GetUndoValue() => OldValue;

    protected sealed override TValue? GetRedoValue() => NewValue;


    protected override void Undo(IEditableObject editableObject, PropertyInfo propertyInfo, TValue? property)
    {
        propertyInfo.SetValue(editableObject, property);
    }

    protected override void Redo(IEditableObject editableObject, PropertyInfo propertyInfo, TValue? property)
    {
        propertyInfo.SetValue(editableObject, property);
    }
}
