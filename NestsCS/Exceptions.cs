// ReSharper disable once CheckNamespace

namespace NestsCS.Exceptions;

public class MissingKeyException : Exception
{
	// ReSharper disable once MemberCanBePrivate.Global
	public readonly string Key;

	internal MissingKeyException(object key) => Key = key.ToString() ?? "<stringified to NULL>";

	public override string Message => $"Tried to get key `{Key}` that does not exist in this nest";
}

public class NotANestException : Exception
{
	internal NotANestException() { }

	public override string Message => "Not a nest";
}