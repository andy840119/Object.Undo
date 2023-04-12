// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using System.Reflection;
using Object.Undo.Interfaces;

namespace Object.Undo.Actions;

public abstract class ChangeAction<TValue>
{
    private readonly IEditableObject obj;
    private readonly string propertyName;

    protected ChangeAction(IEditableObject editableObject, string propertyName, TValue newValue)
    {
        obj = editableObject;
        this.propertyName = propertyName;

        var propertyInfo = getPropertyInfo(obj, propertyName);
        var propertyValue = (TValue)propertyInfo.GetValue(obj);

        BackupValue(propertyValue, newValue);

        // perform the action after method call.
        Redo();
    }

    protected abstract void BackupValue(TValue oldValue, TValue newValue);

    public void Undo()
    {
        var propertyInfo = getPropertyInfo(obj, propertyName);
        Undo(obj, propertyInfo, GetUndoValue());
    }

    public void Redo()
    {
        var propertyInfo = getPropertyInfo(obj, propertyName);
        Redo(obj, propertyInfo, GetRedoValue());
    }

    private static PropertyInfo getPropertyInfo(IEditableObject obj, string propertyName)
        => obj.GetType().GetProperties().Single(x => x.Name == propertyName);

    protected abstract TValue GetUndoValue();

    protected abstract TValue GetRedoValue();

    protected abstract void Undo(IEditableObject editableObject, PropertyInfo propertyInfo, TValue? property);

    protected abstract void Redo(IEditableObject editableObject, PropertyInfo propertyInfo, TValue? property);
}
