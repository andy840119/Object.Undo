// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Object.Undo.Actions;
using Object.Undo.Interfaces;

namespace Object.Undo.Tests.Actions;

public class PropertyChangeActionTest : BasePropertyChangeActionTest
{
    #region Property

    [Test]
    public void TestStringProperty()
    {
        var obj = new EditableObjectWithProperty
        {
            String = "test",
        };

        var action = new PropertyChangeAction<string>(obj, nameof(EditableObjectWithProperty.String), "test2");
        AssertUndoRedoPropertyInClass(action, () => obj.String);
    }

    [Test]
    public void TestInterfaceProperty()
    {
        var obj = new EditableObjectWithProperty
        {
            Interface = new Struct(),
        };

        var action = new PropertyChangeAction<IStruct>(obj, nameof(EditableObjectWithProperty.Interface), null);
        AssertUndoRedoPropertyInClass(action, () => obj.Interface);
    }

    [Test]
    public void TestStructProperty()
    {
        var obj = new EditableObjectWithProperty
        {
            Struct = new Struct(),
        };

        var action = new PropertyChangeAction<Struct>(obj, nameof(EditableObjectWithProperty.Struct), null);
        AssertUndoRedoPropertyInClass(action, () => obj.Struct);
    }

    [Test]
    public void TestEditableObjectProperty()
    {
        var obj = new EditableObjectWithProperty
        {
            EditableObject = new EditableObject(),
        };

        var action = new PropertyChangeAction<EditableObject>(obj, nameof(EditableObjectWithProperty.EditableObject), null);
        AssertUndoRedoPropertyInClass(action, () => obj.EditableObject);
    }

    [Test]
    public void TestIEditableObjectProperty()
    {
        var obj = new EditableObjectWithProperty
        {
            EditableObjectInterface = new EditableObject(),
        };

        var action = new PropertyChangeAction<IEditableObject>(obj, nameof(EditableObjectWithProperty.EditableObjectInterface), null);
        AssertUndoRedoPropertyInClass(action, () => obj.EditableObjectInterface);
    }

    private class EditableObjectWithProperty : IEditableObject
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string String { get; set; } = string.Empty;

        public IStruct? Interface { get; set; }

        public Struct? Struct { get; set; }

        public EditableObject? EditableObject { get; set; }

        public IEditableObject? EditableObjectInterface { get; set; }
    }

    #endregion

    #region List

    [Test]
    public void TestStringList()
    {
        var obj = new EditableObjectWithList
        {
            Strings = new List<string> { "test" },
        };

        var action = new PropertyChangeAction<IList<string>>(obj, nameof(EditableObjectWithList.Strings), null);
        AssertUndoRedoPropertyInClass(action, () => obj.Strings);
    }

    [Test]
    public void TestInterfaceList()
    {
        var obj = new EditableObjectWithList
        {
            Interfaces = new List<IStruct> { new Struct() },
        };

        var action = new PropertyChangeAction<IList<IStruct>>(obj, nameof(EditableObjectWithList.Interfaces), null);
        AssertUndoRedoPropertyInClass(action, () => obj.Interfaces);
    }

    [Test]
    public void TestStructList()
    {
        var obj = new EditableObjectWithList
        {
            Structs = new List<Struct> { new() },
        };

        var action = new PropertyChangeAction<IList<Struct>>(obj, nameof(EditableObjectWithList.Structs), null);
        AssertUndoRedoPropertyInClass(action, () => obj.Structs);
    }

    [Test]
    public void TestEditableObjectList()
    {
        var obj = new EditableObjectWithList
        {
            EditableObjects = new List<EditableObject> { new() },
        };

        var action = new PropertyChangeAction<IList<EditableObject>>(obj, nameof(EditableObjectWithList.EditableObjects), null);
        AssertUndoRedoPropertyInClass(action, () => obj.EditableObjects);
    }

    [Test]
    public void TestIEditableObjectList()
    {
        var obj = new EditableObjectWithList
        {
            EditableObjectInterfaces = new List<IEditableObject> { new EditableObject() },
        };

        var action = new PropertyChangeAction<IList<IEditableObject>>(obj, nameof(EditableObjectWithList.EditableObjectInterfaces), null);
        AssertUndoRedoPropertyInClass(action, () => obj.EditableObjectInterfaces);
    }

    private class EditableObjectWithList : IEditableObject
    {
        public Guid Id { get; } = Guid.NewGuid();

        public IList<string>? Strings { get; set; }

        public IList<IStruct>? Interfaces { get; set; }

        public IList<Struct>? Structs { get; set; }

        public IList<EditableObject>? EditableObjects { get; set; }

        public IList<IEditableObject>? EditableObjectInterfaces { get; set; }
    }

    #endregion

    #region Failed case

    [Test]
    public void TestUndoRedoListWithFailedCase()
    {
        var list  = new List<string> { "test" };
        var obj = new FailedEditableObject
        {
            FailedProperty = list,
        };

        // should not be ok in here because the change will not be recorded.
        list.Clear();
        list.Add("test");

        Assert.Throws<InvalidOperationException>(() =>
        {
            var _ = new PropertyChangeAction<List<string>>(obj, nameof(FailedEditableObject.FailedProperty), list);
        });
    }

    private class FailedEditableObject : IEditableObject
    {
        public Guid Id { get; } = Guid.NewGuid();

        public List<string> FailedProperty { get; set; } = new();
    }

    #endregion

    protected override TValue? GetUndoValue<TProperty, TValue>(ChangeAction<TProperty> action) where TValue : default
    {
        if (action is not PropertyChangeAction<TProperty> propertyChangeAction)
            throw new InvalidCastException();

        var oldValue = propertyChangeAction.OldValue;
        if (oldValue is null)
            return default;

        if (oldValue is not TValue value)
            throw new InvalidCastException();

        return value;
    }

    protected override TValue? GetRedoValue<TProperty, TValue>(ChangeAction<TProperty> action) where TValue : default
    {
        if (action is not PropertyChangeAction<TProperty> propertyChangeAction)
            throw new InvalidCastException();

        var newValue = propertyChangeAction.NewValue;
        if (newValue is null)
            return default;

        if (newValue is not TValue value)
            throw new InvalidCastException();

        return value;
    }

    protected override void AssertValue<T>(T expected, T actual)
    {
        // undo/redo value and the stored value should be the same instance.
        Assert.AreSame(expected, actual);
    }
}
