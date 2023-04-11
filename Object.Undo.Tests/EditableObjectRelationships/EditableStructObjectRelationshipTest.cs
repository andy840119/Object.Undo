// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Object.Undo.Interfaces;
using Object.Undo.Properties;

namespace Object.Undo.Tests.EditableObjectRelationships;

/// <summary>
/// Test the <see cref="EditableObjectRelationship"/> in the struct object type.
/// </summary>
public class EditableStructObjectRelationshipTest : BaseEditableObjectRelationshipTest
{
    [Test]
    public void TestChildInProperty()
    {
        var child = new ChildStruct();
        var parent = new ParentStructWithChildProperty
        {
            Child1 = child
        };
        AssertRelationShip(parent, child, "[Child1]");
    }

    [Test]
    public void TestChildInInterfaceProperty()
    {
        var child = new ChildStruct();
        var parent = new ParentStructWithChildProperty
        {
            Child2 = child
        };
        AssertRelationShip(parent, child, "[Child2]");
    }

    [Test]
    public void TestChildInList()
    {
        var child = new ChildStruct();
        var parent = new ParentStructWithChildList
        {
            Children1 = new List<ChildStruct>
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
        var child = new ChildStruct();
        var parent = new ParentStructWithChildList
        {
            Children2 = new List<IEditableObject>
            {
                new ChildStruct(),
                child
            }
        };
        AssertRelationShip(parent, child, "[Children2, 1]");
    }

    [Test]
    public void TestChildInDictionary()
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

    [Test]
    public void TestChildInObjectKeyDictionary()
    {
        var child = new ChildStruct();
        var parent = new ParentStructWithChildDictionary
        {
            Children2 = new Dictionary<DictionaryStructKey, ChildStruct>
            {
                { new DictionaryStructKey(), child }
            }
        };
        AssertRelationShip(parent, child, "[Children2, key:Object.Undo.Tests.EditableObjectRelationships.EditableStructObjectRelationshipTest+DictionaryStructKey]");
    }

    [Test]
    public void TestChildInDictionaryKey()
    {
        var child = new ChildStruct();
        var parent = new ParentStructWithChildDictionary
        {
            Children3 = new Dictionary<ChildStruct, string>
            {
                { new ChildStruct(), "1" },
                { child, "2" }
            }
        };
        AssertRelationShip(parent, child, "[Children3, value:2]");
    }

    [Test]
    public void TestChildInDictionaryKey2()
    {
        var child = new ChildStruct();
        var parent = new ParentStructWithChildDictionary
        {
            Children4 = new Dictionary<ChildStruct, ChildStruct>
            {
                { new ChildStruct(), new ChildStruct() },
                { child, new ChildStruct() }
            }
        };
        AssertRelationShip(parent, child, "[Children4, value:Object.Undo.Tests.EditableObjectRelationships.EditableStructObjectRelationshipTest+ChildStruct]");
    }

    [Test]
    [Ignore("will get the wrong key if the value is struct.")]
    public void TestChildInDictionaryKey3()
    {
        var child = new ChildStruct();
        var parent = new ParentStructWithChildDictionary
        {
            Children5 = new Dictionary<ChildStruct, DictionaryStructKey>
            {
                { new ChildStruct(), new DictionaryStructKey() },
                { child, new DictionaryStructKey() }
            }
        };
        AssertRelationShip(parent, child, "[Children4, value:Object.Undo.Tests.EditableObjectRelationships.EditableStructObjectRelationshipTest+ChildStruct]");
    }

    [Test]
    public void TestWrongOrder()
    {
        var child = new ChildStruct();
        var parent = new ParentStructWithChildProperty
        {
            Child1 = child
        };
        AssertRelationShip(child, parent, null);
    }

    [Test]
    public void TestChildIsNotInParent()
    {
        var child = new ChildStruct();
        var parent = new ParentStructWithChildProperty();
        AssertRelationShip(child, parent, null);
    }

    private struct ParentStructWithChildProperty : IEditableObject
    {
        public ParentStructWithChildProperty()
        {
            Child1 = null;
            Child2 = null;
        }

        [EditableProperty]
        public ChildStruct? Child1 { get; set; }

        [EditableProperty]
        public IEditableObject? Child2 { get; set; }

        public Guid Id { get; } = Guid.NewGuid();
    }

    private struct ParentStructWithChildList : IEditableObject
    {
        public ParentStructWithChildList()
        {
            Children1 = null;
            Children2 = null;
        }

        [EditableProperty]
        public IList<ChildStruct>? Children1 { get; set; }

        [EditableProperty]
        public IList<IEditableObject>? Children2 { get; set; }

        public Guid Id { get; } = Guid.NewGuid();
    }

    private struct ParentStructWithChildDictionary : IEditableObject
    {
        public ParentStructWithChildDictionary()
        {
            Children1 = null;
            Children2 = null;
            Children3 = null;
            Children4 = null;
            Children5 = null;
        }

        [EditableProperty]
        public IDictionary<string, ChildStruct>? Children1 { get; set; }


        [EditableProperty]
        public IDictionary<DictionaryStructKey, ChildStruct>? Children2 { get; set; }

        [EditableProperty]
        public IDictionary<ChildStruct, string>? Children3 { get; set; }

        [EditableProperty]
        public IDictionary<ChildStruct, ChildStruct>? Children4 { get; set; }

        [EditableProperty]
        public IDictionary<ChildStruct, DictionaryStructKey>? Children5 { get; set; }

        public Guid Id { get; } = Guid.NewGuid();
    }

    private struct DictionaryStructKey
    {
    }

    private struct ChildStruct : IEditableObject
    {
        public ChildStruct()
        {
        }

        public Guid Id { get; } = Guid.NewGuid();
    }
}
