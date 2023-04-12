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

        var action = new SetChangeAction<string>(obj, nameof(EditableObjectWithSet.Strings), "test2", CollectionChangeActionType.Add);
        AssertUndoRedoPropertyInClass(action, () => obj.Strings);
    }

    [Test]
    public void TestInterfaceList()
    {
        var obj = new EditableObjectWithSet
        {
            Interfaces = new HashSet<IStruct> { new Struct() },
        };

        var action = new SetChangeAction<IStruct>(obj, nameof(EditableObjectWithSet.Interfaces), new Struct(), CollectionChangeActionType.Add);
        AssertUndoRedoPropertyInClass(action, () => obj.Interfaces);
    }

    [Test]
    public void TestStructList()
    {
        var obj = new EditableObjectWithSet
        {
            Structs = new HashSet<Struct> { new() },
        };

        var action = new SetChangeAction<Struct>(obj, nameof(EditableObjectWithSet.Structs), new Struct(), CollectionChangeActionType.Add);
        AssertUndoRedoPropertyInClass(action, () => obj.Structs);
    }

    [Test]
    public void TestEditableObjectList()
    {
        var obj = new EditableObjectWithSet
        {
            EditableObjects = new HashSet<EditableObject> { new() },
        };

        var action = new SetChangeAction<EditableObject>(obj, nameof(EditableObjectWithSet.EditableObjects), new EditableObject(), CollectionChangeActionType.Add);
        AssertUndoRedoPropertyInClass(action, () => obj.EditableObjects);
    }

    [Test]
    public void TestIEditableObjectList()
    {
        var obj = new EditableObjectWithSet
        {
            EditableObjectInterfaces = new HashSet<IEditableObject> { new EditableObject() },
        };

        var action = new SetChangeAction<IEditableObject>(obj, nameof(EditableObjectWithSet.EditableObjectInterfaces), new EditableObject(), CollectionChangeActionType.Add);
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

    protected override T GetUndoValue<T>(ChangeAction<T> action)
    {
        throw new NotImplementedException();
    }

    protected override T GetRedoValue<T>(ChangeAction<T> action)
    {
        throw new NotImplementedException();
    }
}
