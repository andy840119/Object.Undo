// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Reflection;

namespace Object.Undo.Properties;

/// <summary>
/// Should add this attribute to the class or struct if this object is support to be edited.
/// Means the property in the class can be undo/redo.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class EditableObjectAttribute : Attribute
{
    /// <summary>
    /// Get the <see cref="EditableObjectAttribute"/> in the class.
    /// Also note that on object should only have one <see cref="EditableObjectAttribute"/>.
    /// </summary>
    /// <param name="editableObject"></param>
    /// <returns></returns>
    public static EditableObjectAttribute? GetEditableObjectAttribute(object editableObject)
        => editableObject.GetType().GetCustomAttributes<EditableObjectAttribute>().SingleOrDefault();
}
