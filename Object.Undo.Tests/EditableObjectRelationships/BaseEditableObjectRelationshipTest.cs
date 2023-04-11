// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Object.Undo.Properties;

namespace Object.Undo.Tests.EditableObjectRelationships;

public class BaseEditableObjectRelationshipTest
{
    protected void AssertRelationShip(object parent, object child, string relationshipString)
    {
        var relationship = EditableObjectRelationship.CreateRelationship(parent, child);

        // Test should be able to get the relationship.
        Assert.AreEqual(relationshipString, relationship.ToString());

        // Test should be able to get the child object from the relationship.
        var expected = EditableObjectAttribute.GetEditableObjectAttribute(child);
        var actual = EditableObjectAttribute.GetEditableObjectAttribute(EditableObjectRelationship.GetPropertyFromRelationShip<object>(parent, relationship));
        Assert.AreSame(expected, actual);
    }
}
