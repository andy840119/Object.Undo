// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Object.Undo.Interfaces;
using Object.Undo.Utils;

namespace Object.Undo.Tests.EditableObjectRelationships;

public class BaseEditableObjectRelationshipTest
{
    protected void AssertRelationShip(IEditableObject parent, IEditableObject child, string? relationshipString)
    {
        var relationship = EditableObjectRelationship.CreateRelationship(parent, child);

        if (relationshipString == null)
        {
            Assert.IsNull(relationship);
        }
        else
        {
            // Test should be able to get the relationship.
            Assert.AreEqual(relationshipString, relationship!.ToString());

            // Test should be able to get the child object from the relationship.
            var actual = EditableObjectRelationship.GetPropertyFromRelationShip(parent, relationship);
            Assert.IsTrue(EditableObjectUtils.IsSameInstance(child, actual));
        }
    }

    protected class Child : IEditableObject
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}
