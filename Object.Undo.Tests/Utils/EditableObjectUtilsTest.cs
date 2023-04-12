// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Object.Undo.Interfaces;
using Object.Undo.Utils;

namespace Object.Undo.Tests.Utils;

public class EditableObjectUtilsTest
{
    #region Class

    [Test]
    public void TestSameEditableClass()
    {
        var editableObject = new EditableClass();
        Assert.IsTrue(EditableObjectUtils.IsSameInstance(editableObject, editableObject));
    }

    [Test]
    public void TestDifferentEditableClass()
    {
        var editableObject1 = new EditableClass();
        var editableObject2 = new EditableClass();
        Assert.IsFalse(EditableObjectUtils.IsSameInstance(editableObject1, editableObject2));
    }

    [Test]
    public void TestSameEditableClassWithWrongId()
    {
        var editableObject = new WrongEditableClass();
        Assert.Throws<ArgumentException>(() => EditableObjectUtils.IsSameInstance(editableObject, editableObject));
    }

    [Test]
    public void TestDifferentEditableClassWithWrongId()
    {
        var editableObject1 = new WrongEditableClass();
        var editableObject2 = new WrongEditableClass();
        Assert.Throws<ArgumentException>(() => EditableObjectUtils.IsSameInstance(editableObject1, editableObject2));
    }

    private class EditableClass : IEditableObject
    {
        public Guid Id { get; } = Guid.NewGuid();
    }

    private class WrongEditableClass : IEditableObject
    {
        public Guid Id { get; } = Guid.Empty;
    }

    #endregion

    #region Struct

    [Test]
    public void TestSameEditableStruct()
    {
        var editableObject = new EditableStruct();
        Assert.IsTrue(EditableObjectUtils.IsSameInstance(editableObject, editableObject));
    }

    [Test]
    public void TestDifferentEditableStruct()
    {
        var editableObject1 = new EditableStruct();
        var editableObject2 = new EditableStruct();
        Assert.IsFalse(EditableObjectUtils.IsSameInstance(editableObject1, editableObject2));
    }

    [Test]
    public void TestSameEditableStructWithWrongId()
    {
        var editableObject = new WrongEditableStruct();
        Assert.Throws<ArgumentException>(() => EditableObjectUtils.IsSameInstance(editableObject, editableObject));
    }

    [Test]
    public void TestDifferentEditableStructWithWrongId()
    {
        var editableObject1 = new WrongEditableStruct();
        var editableObject2 = new WrongEditableStruct();
        Assert.Throws<ArgumentException>(() => EditableObjectUtils.IsSameInstance(editableObject1, editableObject2));
    }

    private struct EditableStruct : IEditableObject
    {
        public EditableStruct()
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }

    private struct WrongEditableStruct : IEditableObject
    {
        public WrongEditableStruct()
        {
        }

        public Guid Id { get; } = Guid.Empty;
    }

    #endregion
}
