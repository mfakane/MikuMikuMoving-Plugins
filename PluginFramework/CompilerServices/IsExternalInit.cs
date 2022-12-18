#if !NET5_0_OR_GREATER

using System.ComponentModel;

namespace System.Runtime.CompilerServices;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class IsExternalInit
{
}

#endif