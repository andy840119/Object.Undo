// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Object.Undo.Interfaces;
using Object.Undo.Properties;

namespace Object.Undo.Tests.EditableObjectRelationships;

public class EditableObjectInListTest : BaseEditableObjectRelationshipTest
{
    [Test]
    public void TestChildInList()
    {
        var child = new Child();
        var parent = new ParentWithChildList
        {
            Children1 = new List<Child>
            {
                new(),
                child
            }
        };
        AssertRelationShip(parent, child, "[Children1, 1]");
    }

    [Test]
    public void TestChildInListInterface()
    {
        var child = new Child();
        var parent = new ParentWithChildList
        {
            Children2 = new List<IEditableObject>
            {
                new Child(),
                child
            }
        };
        AssertRelationShip(parent, child, "[Children2, 1]");
    }

    private class ParentWithChildList : IEditableObject
    {
        [EditableProperty]
        public IList<Child>? Children1 { get; set; }

        [EditableProperty]
        public IList<IEditableObject>? Children2 { get; set; }

        public Guid Id { get; } = Guid.NewGuid();
    }
}
