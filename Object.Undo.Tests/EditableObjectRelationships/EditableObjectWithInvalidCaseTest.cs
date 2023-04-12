// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Object.Undo.Interfaces;
using Object.Undo.Properties;

namespace Object.Undo.Tests.EditableObjectRelationships;

/// <summary>
/// Test the <see cref="EditableObjectRelationship"/> in the class object type.
/// </summary>
public class EditableObjectWithInvalidCaseTest : BaseEditableObjectRelationshipTest
{
    [Test]
    public void TestWrongOrder()
    {
        var child = new Child();
        var parent = new ParentWithChildProperty
        {
            Child1 = child
        };
        AssertRelationShip(child, parent, null);
    }

    [Test]
    public void TestChildIsNotInParent()
    {
        var child = new Child();
        var parent = new ParentWithChildProperty();
        AssertRelationShip(child, parent, null);
    }

    private class ParentWithChildProperty : IEditableObject
    {
        [EditableProperty]
        public Child? Child1 { get; set; }

        public Guid Id { get; } = Guid.NewGuid();
    }
}
