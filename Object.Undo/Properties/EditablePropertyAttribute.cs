// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using Object.Undo.Interfaces;

namespace Object.Undo.Properties;

/// <summary>
/// Should add this attribute to the property that able to be edited.
/// Means the property in the class can be undo/redo.
/// Note that the class should add the <see cref="IEditableObject"/>
/// </summary>
[AttributeUsage(AttributeTargets.Property)]

public class EditablePropertyAttribute: Attribute
{
}
