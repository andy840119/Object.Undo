// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Object.Undo.Properties;

namespace Object.Undo;

public class EditableObjectRelationship
{
    private readonly IList<Layer> layers = new List<Layer>();

    public static EditableObjectRelationship? CreateRelationship(object parent, object child)
    {
        var layers = getRelationship(parent, child, new List<Layer>());
        if (layers == null)
            return null;

        var relationship = new EditableObjectRelationship();
        foreach (var layer in layers)
        {
            relationship.layers.Add(layer);
        }
        return relationship;
    }

    private static IList<Layer>? getRelationship(object parent, object child, IList<Layer> layers)
    {
        // get all property that object type is editable object.
        foreach (var propertyInfo in getEditableProperties(parent))
        {
            // todo: deal with multiple layer
            var layer = createLayerIfMatch(parent, propertyInfo, child);
            if (layer != null)
            {
                layers.Add(layer);
                return layers;
            }
        }

        return null;

        static IEnumerable<PropertyInfo> getEditableProperties(object parent)
            => parent.GetType().GetProperties();

        static Layer? createLayerIfMatch(object parent, PropertyInfo propertyInfo, object target)
        {
            var propertyType = propertyInfo.PropertyType;

            // editable object in the property from the parent.
            if (propertyInfo.GetCustomAttribute<EditableObjectAttribute>() != null)
            {
                if(propertyInfo.GetValue(parent) == target)
                    return new PropertyLayer(propertyInfo.Name);
            }

            // editable object in the list property from the parent.
            if (propertyType.GetGenericTypeDefinition() == typeof(IList<>))
            {
                // check if the list contains editable object.
                var hasEditableObjectAttribute = propertyType.GetGenericArguments().Any(x => x.GetCustomAttribute<EditableObjectAttribute>() != null);
                if (hasEditableObjectAttribute)
                {
                    // check if the list contains editable object.
                    var list = (IList)propertyInfo.GetValue(parent);
                    for(int i=0; i<list.Count; i++)
                    {
                        if (list[i] == target)
                            return new ListLayer(propertyInfo.Name, i);
                    }
                }
            }

            // editable object in the dictionary property from the parent.
            if (propertyType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
            {
                // check if the list contains editable object.
                var hasEditableObjectAttribute = propertyType.GetGenericArguments().Any(x => x.GetCustomAttribute<EditableObjectAttribute>() != null);
                if (hasEditableObjectAttribute)
                {
                    // check if the list contains editable object.
                    var dictionary = (IDictionary)propertyInfo.GetValue(parent);
                    foreach (var key in dictionary.Keys)
                    {
                        if (dictionary[key] == target)
                            return new DictionaryValueLayer(propertyInfo.Name, key);
                    }
                }
            }

            return null;
        }
    }

    public static TObject GetPropertyFromRelationShip<TObject>(object parent, EditableObjectRelationship relationship)
    {
        object value = parent;

        foreach (var layer in relationship.layers)
        {
            var property = value.GetType().GetProperty(layer.PropertyName);
            if (property == null)
                throw new NullReferenceException($"Property {layer.PropertyName} not found in {parent.GetType()}");

            value = getValue(property.GetValue(value), layer);
        }

        return (TObject)value;

        static object getValue(object value, Layer layer)
        {
            return layer switch
            {
                PropertyLayer => layer,
                ListLayer listLayer => ((IList)value)[listLayer.Index],
                DictionaryKeyLayer dictionaryLayer => getKeyByValue((IDictionary)value, dictionaryLayer.Value),
                DictionaryValueLayer dictionaryLayer => ((IDictionary)value)[dictionaryLayer.Key],
                _ => value
            };
        }

        static object getKeyByValue(IDictionary dictionary, object value)
        {
            foreach (var key in dictionary.Keys)
            {
                if (dictionary[key] == value)
                    return (TObject)key;
            }

            throw new NullReferenceException($"Key not found in dictionary for value {value}");
        }
    }

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
        public int Index { get; set; }

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
        public object Value { get; set; }

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
        public object Key { get; set; }

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
