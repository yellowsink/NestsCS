using System.Diagnostics.CodeAnalysis;

namespace NestsCS;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum EventType
{
	BULK,
	SET,
	DELETE
}