// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Object.Undo.Actions;
using Object.Undo.Interfaces;

namespace Object.Undo.Tests.Actions;

public abstract class BasePropertyChangeActionTest
{
    protected void AssertUndoRedoPropertyInClass<TProperty, TValue>(ChangeAction<TProperty> action, Func<TValue> getValueAction)
    {
        action.Undo();
        AssertValue(GetUndoValue<TProperty, TValue>(action), getValueAction());

        action.Redo();
        AssertValue(GetRedoValue<TProperty, TValue>(action), getValueAction());
    }

    protected abstract TValue? GetUndoValue<TProperty, TValue>(ChangeAction<TProperty> action);

    protected abstract TValue? GetRedoValue<TProperty, TValue>(ChangeAction<TProperty> action);

    protected abstract void AssertValue<TValue>(TValue expected, TValue actual);

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
