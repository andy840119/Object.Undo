// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Object.Undo.Actions;
using Object.Undo.Interfaces;

namespace Object.Undo.Tests.Actions;

public abstract class BasePropertyChangeActionTest
{
    protected void AssertUndoRedoPropertyInClass<T>(ChangeAction<T> action, Func<T> getPropertyAction)
    {
        // should test the value changed to the old value.
        action.Undo();
        Assert.AreEqual(GetUndoValue(action), getPropertyAction());

        // should test the value changed to the new value.
        action.Redo();
        Assert.AreEqual(GetRedoValue(action), getPropertyAction());
    }

    protected abstract T GetUndoValue<T>(ChangeAction<T> action);

    protected abstract T GetRedoValue<T>(ChangeAction<T> action);

    protected interface IStruct
    {

    }

    protected class Struct : IStruct
    {

    }

    protected class EditableObject : IEditableObject
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}
