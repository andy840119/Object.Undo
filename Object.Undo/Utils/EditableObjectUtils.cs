// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using Object.Undo.Interfaces;

namespace Object.Undo.Utils;

public static class EditableObjectUtils
{
    public static bool IsSameInstance(IEditableObject obj1, IEditableObject obj2)
    {
        if (obj1.Id == Guid.Empty || obj2.Id == Guid.Empty)
            throw new ArgumentException(
                "The ID of the object is empty. Editable object must have the random Guid.");

        // technically Guid will not duplicated, but it's better to check is the same instance.
        return obj1.Equals(obj2) && obj1.Id == obj2.Id;
    }
}
