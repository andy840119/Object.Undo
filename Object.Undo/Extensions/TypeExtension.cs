// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;

namespace Object.Undo.Extensions;

public static class TypeExtension
{
    public static bool InheritInterface<T>(this Type type)
        => type.GetInterfaces().Contains(typeof(T));
}
