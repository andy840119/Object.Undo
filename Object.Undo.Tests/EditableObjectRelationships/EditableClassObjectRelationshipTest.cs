// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Object.Undo.Properties;

namespace Object.Undo.Tests.EditableObjectRelationships;

/// <summary>
/// Test the <see cref="EditableObjectRelationship"/> in the class object type.
/// </summary>
public class EditableClassObjectRelationshipTest : BaseEditableObjectRelationshipTest
{
    [Test]
    public void TestCreateRelationshipInProperty()
    {
        var child = new ChildClass();
        var parent = new ParentClassWithChildProperty
        {
            Child = child
        };
        AssertRelationShip(parent, child, "[Child]");
    }

    [Test]
    public void TestCreateRelationshipInList()
    {
        var child = new ChildClass();
        var parent = new ParentClassWithChildList
        {
            Children = new List<ChildClass>
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
        var child = new ChildClass();
            var parent = new ParentClassWithChildDictionary
        {
            Children1 = new Dictionary<string, ChildClass>
            {
                { "1", new ChildClass() },
                { "2", child }
            }
        };
        AssertRelationShip(parent, child, "[Children1, key:2]");
    }

    [Test]
    public void TestCreateRelationshipInObjectKeyDictionary()
    {
        var child = new ChildClass();
        var parent = new ParentClassWithChildDictionary
        {
            Children2 = new Dictionary<DictionaryClassKey, ChildClass>
            {
                { new DictionaryClassKey(), new ChildClass() },
                { new DictionaryClassKey(), child }
            }
        };
        AssertRelationShip(parent, child, "[Children1, key:1]");
    }

    [EditableObject]
    private class ParentClassWithChildProperty
    {
        public ChildClass? Child { get; set; }
    }

    [EditableObject]
    private class ParentClassWithChildList
    {
        public IList<ChildClass>? Children { get; set; }
    }

    [EditableObject]
    private class ParentClassWithChildDictionary
    {
        public IDictionary<string, ChildClass>? Children1 { get; set; }

        // todo: should support able to get the property if the key is not string.
        public IDictionary<DictionaryClassKey, ChildClass>? Children2 { get; set; }

        // todo: should be able to get the editable object from the key.
        public IDictionary<ChildClass, string>? Children3 { get; set; }

        // todo: should be able to get the editable object from the key or value.
        public IDictionary<ChildClass, ChildClass>? Children4 { get; set; }

    }

    private class DictionaryClassKey
    {

    }

    [EditableObject]
    private class ChildClass
    {

    }
}
