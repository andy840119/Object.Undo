// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Object.Undo.Actions;
using Object.Undo.Interfaces;

namespace Object.Undo.Tests.Actions;

public class SetChangeActionTest : BasePropertyChangeActionTest
{
    [Test]
    public void TestStringList()
    {
        var obj = new EditableObjectWithSet
        {
            Strings = new HashSet<string> { "test" },
        };

        var action = new TestSetChangeAction<string>(obj, nameof(EditableObjectWithSet.Strings), "test2", CollectionChangeActionType.Add);
        AssertUndoRedoPropertyInClass(action, () => obj.Strings);
    }

    [Test]
    public void TestInterfaceList()
    {
        var obj = new EditableObjectWithSet
        {
            Interfaces = new HashSet<IStruct> { new Struct() },
        };

        var action = new TestSetChangeAction<IStruct>(obj, nameof(EditableObjectWithSet.Interfaces), new Struct(), CollectionChangeActionType.Add);
        AssertUndoRedoPropertyInClass(action, () => obj.Interfaces);
    }

    [Test]
    public void TestStructList()
    {
        var obj = new EditableObjectWithSet
        {
            Structs = new HashSet<Struct> { new() },
        };

        var action = new TestSetChangeAction<Struct>(obj, nameof(EditableObjectWithSet.Structs), new Struct(), CollectionChangeActionType.Add);
        AssertUndoRedoPropertyInClass(action, () => obj.Structs);
    }

    [Test]
    public void TestEditableObjectList()
    {
        var obj = new EditableObjectWithSet
        {
            EditableObjects = new HashSet<EditableObject> { new() },
        };

        var action = new TestSetChangeAction<EditableObject>(obj, nameof(EditableObjectWithSet.EditableObjects), new EditableObject(), CollectionChangeActionType.Add);
        AssertUndoRedoPropertyInClass(action, () => obj.EditableObjects);
    }

    [Test]
    public void TestIEditableObjectList()
    {
        var obj = new EditableObjectWithSet
        {
            EditableObjectInterfaces = new HashSet<IEditableObject> { new EditableObject() },
        };

        var action = new TestSetChangeAction<IEditableObject>(obj, nameof(EditableObjectWithSet.EditableObjectInterfaces), new EditableObject(), CollectionChangeActionType.Add);
        AssertUndoRedoPropertyInClass(action, () => obj.EditableObjectInterfaces);
    }

    private class EditableObjectWithSet : IEditableObject
    {
        public Guid Id { get; } = Guid.NewGuid();

        public ISet<string>? Strings { get; set; }

        public ISet<IStruct>? Interfaces { get; set; }

        public ISet<Struct>? Structs { get; set; }

        public ISet<EditableObject>? EditableObjects { get; set; }

        public ISet<IEditableObject>? EditableObjectInterfaces { get; set; }
    }

    protected override TValue GetUndoValue<TProperty, TValue>(ChangeAction<TProperty> action) where TValue : default
    {
        if (action is not TestSetChangeAction<TValue> changeAction)
            throw new InvalidCastException();

        if (changeAction.GetUndoValue() is not TValue value)
            throw new InvalidCastException();

        return value;
    }

    protected override TValue GetRedoValue<TProperty, TValue>(ChangeAction<TProperty> action) where TValue : default
    {
        if (action is not TestSetChangeAction<TValue> changeAction)
            throw new InvalidCastException();

        if (changeAction.GetRedoValue() is not TValue value)
            throw new InvalidCastException();

        return value;
    }

    protected override void AssertValue<T>(T expected, T actual)
    {
        // as we already know that the this action will not change the instance in the property,
        // so we can just change the elements in the set.
        Assert.AreEqual(expected, actual);
    }

    // expose the testing property.
    private class TestSetChangeAction<TValue> : SetChangeAction<TValue>
    {
        public TestSetChangeAction(IEditableObject editableObject, string propertyName, TValue value, CollectionChangeActionType actionType)
            : base(editableObject, propertyName, value, actionType)
        {
        }

        public new ICollection<TValue> GetUndoValue()
        {
            return new HashSet<TValue>();
        }

        public new ICollection<TValue> GetRedoValue()
        {
            return new HashSet<TValue>();
        }
    }
}
