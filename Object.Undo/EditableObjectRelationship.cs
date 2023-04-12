// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Object.Undo.Interfaces;
using Object.Undo.Properties;
using Object.Undo.Utils;

namespace Object.Undo;

public class EditableObjectRelationship
{
    private readonly IList<Layer> layers = new List<Layer>();

    #region Get relationshipt

    public static EditableObjectRelationship? CreateRelationship(IEditableObject parent, IEditableObject target)
    {
        var layers = findLayers(parent, target);
        if (layers == null)
            return null;

        var relationship = new EditableObjectRelationship();
        foreach (var layer in layers)
        {
            relationship.layers.Add(layer);
        }
        return relationship;
    }

    private static Stack<Layer>? findLayers(IEditableObject parent, IEditableObject target)
    {
        if (EditableObjectUtils.IsSameInstance(parent, target))
            return new Stack<Layer>();

        foreach (var propertyInfo in getEditableProperties(parent))
        {
            var propertyName = propertyInfo.Name;
            var propertyValue = propertyInfo.GetValue(parent);

            var childValue = propertyValue switch
            {
                IEditableObject editableObject => findLayerInProperty(propertyName, editableObject, target),
                IList list => findLayerInListProperty(propertyName, list, target),
                IDictionary dictionary => findLayerInDictionaryProperty(propertyName, dictionary, target),
                null => null,
                _ => throw new InvalidOperationException()
            };

            if (childValue != null)
                return childValue;
        }

        return null;

        static IEnumerable<PropertyInfo> getEditableProperties(object parent)
            => parent.GetType().GetProperties().Where(x => Attribute.IsDefined(x,typeof(EditablePropertyAttribute)));
    }

    private static Stack<Layer>? findLayerInProperty(string propertyName, IEditableObject propertyValue, IEditableObject target)
    {
        var stack = findLayers(propertyValue, target);
        stack?.Push(new PropertyLayer(propertyName));

        return stack;
    }

    private static Stack<Layer>? findLayerInListProperty(string propertyName, IList propertyValue, IEditableObject target)
    {
        for(int i = 0; i < propertyValue.Count; i++)
        {
            if(propertyValue[i] is not IEditableObject editableObject)
                continue;

            var stack = findLayers(editableObject, target);
            if (stack == null)
                continue;

            stack.Push(new ListLayer(propertyName, i));
            return stack;
        }

        return null;
    }

    private static Stack<Layer>? findLayerInDictionaryProperty(string propertyName, IDictionary propertyValue, IEditableObject target)
    {
        // get value by key
        foreach (var key in propertyValue.Keys)
        {
            if(propertyValue[key] is not IEditableObject editableObject)
                continue;

            var stack = findLayers(editableObject, target);
            if (stack == null)
                continue;

            stack.Push(new DictionaryValueLayer(propertyName, key));
            return stack;
        }

        // get key by value
        foreach (var value in propertyValue.Values)
        {
            if (getKeyByValue(propertyValue, value) is not IEditableObject editableObject)
                continue;

            var stack = findLayers(editableObject, target);
            if (stack == null)
                continue;

            stack.Push(new DictionaryKeyLayer(propertyName, value));
            return stack;
        }

        return null;
    }

    #endregion

    #region Apply relationship

    public static TEditableObject GetPropertyFromRelationShip<TEditableObject>(IEditableObject parent, EditableObjectRelationship relationship)
        where TEditableObject : IEditableObject
    {
        var value = GetPropertyFromRelationShip(parent, relationship);

        if (value is not TEditableObject editableObject)
            throw new InvalidCastException();

        return editableObject;
    }


    public static IEditableObject GetPropertyFromRelationShip(IEditableObject parent, EditableObjectRelationship relationship)
    {
        return relationship.layers.Aggregate(parent, getValue);

        static IEditableObject getValue(object parent, Layer layer)
        {
            var property = parent.GetType().GetProperty(layer.PropertyName);
            if (property == null)
                throw new NullReferenceException($"Property {layer.PropertyName} not found in {parent.GetType()}");

            var propertyValue = property.GetValue(parent);
            return getEditableObjectFromProperty(propertyValue, layer);
        }
    }

    private static IEditableObject getEditableObjectFromProperty(object propertyValue, Layer layer)
    {
        var childValue = layer switch
        {
            PropertyLayer => propertyValue,
            ListLayer listLayer => ((IList)propertyValue)[listLayer.Index],
            DictionaryKeyLayer dictionaryLayer => getKeyByValue((IDictionary)propertyValue, dictionaryLayer.Value),
            DictionaryValueLayer dictionaryLayer => ((IDictionary)propertyValue)[dictionaryLayer.Key],
            _ => throw new InvalidOperationException()
        };

        if (childValue is not IEditableObject editableObject)
            throw new InvalidCastException();

        return editableObject;
    }

    #endregion

    #region Utils

    private static object getKeyByValue(IDictionary dictionary, object value)
    {
        foreach (var key in dictionary.Keys)
        {
            // Note that will get the wrong key if the value is struct.
            if (dictionary[key].Equals(value))
                return key;
        }

        throw new NullReferenceException($"Key not found in dictionary for value {value}");
    }

    #endregion

    public override string ToString()
    {
        return string.Join(",", layers.Select(x => x.ToString()));
    }

    private abstract class Layer
    {
        public string PropertyName { get; }

        protected Layer(string propertyName)
        {
            PropertyName = propertyName;
        }
    }

    private class PropertyLayer : Layer
    {
        public PropertyLayer(string propertyName)
            : base(propertyName)
        {
        }

        public override string ToString()
        {
            return $"[{PropertyName}]";
        }
    }

    private class ListLayer : Layer
    {
        public int Index { get; }

        public ListLayer(string propertyName, int index)
            : base(propertyName)
        {
            Index = index;
        }

        public override string ToString()
        {
            return $"[{PropertyName}, {Index}]";
        }
    }

    private class DictionaryKeyLayer : Layer
    {
        public object Value { get; }

        public DictionaryKeyLayer(string propertyName, object value)
            : base(propertyName)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"[{PropertyName}, value:{Value}]";
        }
    }

    private class DictionaryValueLayer : Layer
    {
        public object Key { get; }

        public DictionaryValueLayer(string propertyName, object key)
            : base(propertyName)
        {
            Key = key;
        }

        public override string ToString()
        {
            return $"[{PropertyName}, key:{Key}]";
        }
    }
}
