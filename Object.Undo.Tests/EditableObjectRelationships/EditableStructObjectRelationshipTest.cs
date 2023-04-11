// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Object.Undo.Properties;

namespace Object.Undo.Tests.EditableObjectRelationships;

/// <summary>
/// Test the <see cref="EditableObjectRelationship"/> in the struct object type.
/// </summary>
public class EditableStructObjectRelationshipTest : BaseEditableObjectRelationshipTest
{
    [Test]
    public void TestCreateRelationshipInProperty()
    {
        var child = new ChildStruct();
        var parent = new ParentStructWithChildProperty
        {
            Child = child
        };
        AssertRelationShip(parent, child, "[Child]");
    }

    [Test]
    public void TestCreateRelationshipInList()
    {
        var child = new ChildStruct();
        var parent = new ParentStructWithChildList
        {
            Children = new List<ChildStruct>
            {
                new(),
                child
            }
        };
        AssertRelationShip(parent, child, "[Children, 1]");
    }

    [Test]
    public void TestCreateRelationshipInDictionary()
    {
        var child = new ChildStruct();
        var parent = new ParentStructWithChildDictionary
        {
            Children1 = new Dictionary<string, ChildStruct>
            {
                { "1", new ChildStruct() },
                { "2", child }
            }
        };
        AssertRelationShip(parent, child, "[Children1, key:2]");
    }

    [EditableObject]
    private struct ParentStructWithChildProperty
    {
        public ChildStruct? Child { get; set; }
    }

    [EditableObject]
    private struct ParentStructWithChildList
    {
        public IList<ChildStruct>? Children { get; set; }
    }

    [EditableObject]
    private struct ParentStructWithChildDictionary
    {
        public IDictionary<string, ChildStruct>? Children1 { get; set; }

        // todo: should support able to get the property if the key is not string.
        public IDictionary<DictionaryStructKey, ChildStruct>? Children2 { get; set; }

        // todo: should be able to get the editable object from the key.
        public IDictionary<ChildStruct, string>? Children3 { get; set; }
    }

    private struct DictionaryStructKey
    {
    }

    [EditableObject]
    private struct ChildStruct
    {
    }
}
