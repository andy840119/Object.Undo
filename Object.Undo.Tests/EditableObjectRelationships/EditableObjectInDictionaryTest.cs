// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Object.Undo.Interfaces;
using Object.Undo.Properties;

namespace Object.Undo.Tests.EditableObjectRelationships;

public class EditableObjectInDictionaryTest : BaseEditableObjectRelationshipTest
{
    [Test]
    public void TestChildInDictionary()
    {
        var child = new Child();
        var parent = new ParentWithChildDictionary
        {
            Children1 = new Dictionary<string, Child>
            {
                { "1", new Child() },
                { "2", child }
            }
        };
        AssertRelationShip(parent, child, "[Children1, key:2]");
    }

    [Test]
    public void TestChildInObjectKeyDictionary()
    {
        var child = new Child();
        var parent = new ParentWithChildDictionary
        {
            Children2 = new Dictionary<DictionaryKey, Child>
            {
                { new DictionaryKey(), new Child() },
                { new DictionaryKey(), child }
            }
        };
        AssertRelationShip(parent, child, "[Children2, key:Object.Undo.Tests.EditableObjectRelationships.EditableClassObjectRelationshipTest+DictionaryClassKey]");
    }

    [Test]
    public void TestChildInDictionaryKey()
    {
        var child = new Child();
        var parent = new ParentWithChildDictionary
        {
            Children3 = new Dictionary<Child, string>
            {
                { new Child(), "1" },
                { child, "2" }
            }
        };
        AssertRelationShip(parent, child, "[Children3, value:2]");
    }

    [Test]
    public void TestChildInDictionaryKey2()
    {
        var child = new Child();
        var parent = new ParentWithChildDictionary
        {
            Children4 = new Dictionary<Child, Child>
            {
                { new Child(), new Child() },
                { child, new Child() }
            }
        };
        AssertRelationShip(parent, child, "[Children4, value:Object.Undo.Tests.EditableObjectRelationships.EditableClassObjectRelationshipTest+ChildClass]");
    }

    private class ParentWithChildDictionary : IEditableObject
    {
        [EditableProperty]
        public IDictionary<string, Child>? Children1 { get; set; }

        [EditableProperty]
        public IDictionary<DictionaryKey, Child>? Children2 { get; set; }

        [EditableProperty]
        public IDictionary<Child, string>? Children3 { get; set; }

        [EditableProperty]
        public IDictionary<Child, Child>? Children4 { get; set; }

        public Guid Id { get; } = Guid.NewGuid();
    }

    private class DictionaryKey
    {
    }
}
