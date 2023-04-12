// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Object.Undo.Interfaces;
using Object.Undo.Properties;

namespace Object.Undo.Tests.EditableObjectRelationships;

/// <summary>
/// Test the <see cref="EditableObjectRelationship"/> in the class object type.
/// </summary>
public class EditableClassObjectRelationshipTest : BaseEditableObjectRelationshipTest
{
    [Test]
    public void TestChildInProperty()
    {
        var child = new ChildClass();
        var parent = new ParentClassWithChildProperty
        {
            Child1 = child
        };
        AssertRelationShip(parent, child, "[Child1]");
    }

    [Test]
    public void TestChildInInterfaceProperty()
    {
        var child = new ChildClass();
        var parent = new ParentClassWithChildProperty
        {
            Child2 = child
        };
        AssertRelationShip(parent, child, "[Child2]");
    }

    [Test]
    public void TestChildInList()
    {
        var child = new ChildClass();
        var parent = new ParentClassWithChildList
        {
            Children1 = new List<ChildClass>
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
        var child = new ChildClass();
        var parent = new ParentClassWithChildList
        {
            Children2 = new List<IEditableObject>
            {
                new ChildClass(),
                child
            }
        };
        AssertRelationShip(parent, child, "[Children2, 1]");
    }

    [Test]
    public void TestChildInDictionary()
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
    public void TestChildInObjectKeyDictionary()
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
        AssertRelationShip(parent, child, "[Children2, key:Object.Undo.Tests.EditableObjectRelationships.EditableClassObjectRelationshipTest+DictionaryClassKey]");
    }

    [Test]
    public void TestChildInDictionaryKey()
    {
        var child = new ChildClass();
        var parent = new ParentClassWithChildDictionary
        {
            Children3 = new Dictionary<ChildClass, string>
            {
                { new ChildClass(), "1" },
                { child, "2" }
            }
        };
        AssertRelationShip(parent, child, "[Children3, value:2]");
    }

    [Test]
    public void TestChildInDictionaryKey2()
    {
        var child = new ChildClass();
        var parent = new ParentClassWithChildDictionary
        {
            Children4 = new Dictionary<ChildClass, ChildClass>
            {
                { new ChildClass(), new ChildClass() },
                { child, new ChildClass() }
            }
        };
        AssertRelationShip(parent, child, "[Children4, value:Object.Undo.Tests.EditableObjectRelationships.EditableClassObjectRelationshipTest+ChildClass]");
    }

    [Test]
    public void TestWrongOrder()
    {
        var child = new ChildClass();
        var parent = new ParentClassWithChildProperty
        {
            Child1 = child
        };
        AssertRelationShip(child, parent, null);
    }

    [Test]
    public void TestChildIsNotInParent()
    {
        var child = new ChildClass();
        var parent = new ParentClassWithChildProperty();
        AssertRelationShip(child, parent, null);
    }

    private class ParentClassWithChildProperty : IEditableObject
    {
        [EditableProperty]
        public ChildClass? Child1 { get; set; }

        [EditableProperty]
        public IEditableObject? Child2 { get; set; }

        public Guid Id { get; } = Guid.NewGuid();
    }

    private class ParentClassWithChildList : IEditableObject
    {
        [EditableProperty]
        public IList<ChildClass>? Children1 { get; set; }

        [EditableProperty]
        public IList<IEditableObject>? Children2 { get; set; }

        public Guid Id { get; } = Guid.NewGuid();
    }

    private class ParentClassWithChildDictionary : IEditableObject
    {
        [EditableProperty]
        public IDictionary<string, ChildClass>? Children1 { get; set; }

        [EditableProperty]
        public IDictionary<DictionaryClassKey, ChildClass>? Children2 { get; set; }

        [EditableProperty]
        public IDictionary<ChildClass, string>? Children3 { get; set; }

        [EditableProperty]
        public IDictionary<ChildClass, ChildClass>? Children4 { get; set; }

        public Guid Id { get; } = Guid.NewGuid();
    }

    private class DictionaryClassKey
    {
    }

    private class ChildClass : IEditableObject
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}
