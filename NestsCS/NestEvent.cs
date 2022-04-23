using System.Diagnostics.CodeAnalysis;

namespace NestsCS;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum NestEvent
{
	BULK,
	SET,
	DELETE
}