namespace NestsCS;

public static class Util
{
	public static bool SetsEqual<T>(ICollection<T> hs1, ICollection<T> hs2)
		=> hs1.Count == hs2.Count && hs1.OrderBy(e => e).SequenceEqual(hs2.OrderBy(e => e));
}