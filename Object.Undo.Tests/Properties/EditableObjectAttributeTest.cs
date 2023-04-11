// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Object.Undo.Properties;

namespace Object.Undo.Tests.Properties;

public class EditableObjectAttributeTest
{
    [Test]
    public void TestGetEditableClass()
    {
        assertEditableObject<EditableClass>();
    }

    [Test]
    public void TestGetEditableBaseClass()
    {
        assertEditableObject<EditableChildClass>();
    }

    [Test]
    public void TestGetEditableStruct()
    {
        assertEditableObject<EditableStruct>();
    }

    [Test]
    public void TestGetEditableChildClassWithDuplicatedAttribute()
    {
        assertEditableObject<EditableChildClassWithDuplicatedAttribute>();
    }

    private void assertEditableObject<TEditableObject>() where TEditableObject : new()
    {
        var attribute1 = EditableObjectAttribute.GetEditableObjectAttribute(new TEditableObject());
        var attribute2 = EditableObjectAttribute.GetEditableObjectAttribute(new TEditableObject());

        Assert.NotNull(attribute1);
        Assert.NotNull(attribute2);

        // different instance of class or struct should have have the same instance of attribute.
        Assert.AreNotSame(attribute1, attribute2);
    }

    [EditableObject]
    private class EditableClass
    {
    }

    [EditableObject]
    private abstract class EditableBaseClass
    {
    }

    private class EditableChildClass : EditableBaseClass
    {
    }

    [EditableObject]
    private struct EditableStruct
    {
    }

    [EditableObject]
    private class EditableChildClassWithDuplicatedAttribute : EditableBaseClass
    {
    }
}
