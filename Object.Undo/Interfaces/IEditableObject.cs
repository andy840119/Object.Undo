// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace Object.Undo.Interfaces;

/// <summary>
/// All editable object should implement this interface.
/// For able to identify the object.
/// </summary>
public interface IEditableObject
{
    Guid Id { get; }
}
