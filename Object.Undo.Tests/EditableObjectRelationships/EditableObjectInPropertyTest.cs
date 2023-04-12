// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Object.Undo.Interfaces;
using Object.Undo.Properties;

namespace Object.Undo.Tests.EditableObjectRelationships;

public class EditableObjectInPropertyTest : BaseEditableObjectRelationshipTest
{
    [Test]
    public void TestChildInProperty()
    {
        var child = new Child();
        var parent = new ParentWithChildProperty
        {
            Child1 = child
        };
        AssertRelationShip(parent, child, "[Child1]");
    }

    [Test]
    public void TestChildInInterfaceProperty()
    {
        var child = new Child();
        var parent = new ParentWithChildProperty
        {
            Child2 = child
        };
        AssertRelationShip(parent, child, "[Child2]");
    }

    private class ParentWithChildProperty : IEditableObject
    {
        [EditableProperty]
        public Child? Child1 { get; set; }

        [EditableProperty]
        public IEditableObject? Child2 { get; set; }

        public Guid Id { get; } = Guid.NewGuid();
    }
}
