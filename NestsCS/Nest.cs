using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using NestsCS.Exceptions;

namespace NestsCS;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class Nest
{
	public static dynamic Make(object init) => new NestInternals(init);

	/*
	public static void Shallow(dynamic nest) => throw new NotImplementedException();
	public static void Deep(dynamic    nest) => throw new NotImplementedException();
	*/

	public static void Silent(dynamic nest)
	{
		if (nest is not NestInternals realNest) throw new NotANestException();
		realNest.FireEvents = false;
	}

	public static void Loud(dynamic nest)
	{
		if (nest is not NestInternals realNest) throw new NotANestException();
		realNest.FireEvents = false;
	}

	public static void Bulk(dynamic nest) => throw new NotImplementedException();
	public static void Copy(dynamic nest) => throw new NotImplementedException();

	public static void On(dynamic nest, NestEvent[] types, Action<NestEvent, object, object?> handler)
	{
		if (nest is not NestInternals realNest) throw new NotANestException();
		realNest.EventHandlers.Add((new(types), handler));
	}

	public static void Once(dynamic nest, NestEvent[] types, Action<NestEvent, object, object?> handler)
	{
		if (nest is not NestInternals realNest) throw new NotANestException();

		void PatchedHandler(NestEvent e, object k, object? v)
		{
			handler(e, k, v);
			Off(nest, types, (Action<NestEvent, object, object?>) PatchedHandler);
		}

		realNest.EventHandlers.Add((new(types), PatchedHandler));
	}

	public static void Off(dynamic nest, NestEvent[] types, Action<NestEvent, object, object?> handler)
	{
		if (nest is not NestInternals realNest) throw new NotANestException();
		realNest.EventHandlers.RemoveAll(e => Util.SetsEqual(e.Item1, types) && e.Item2 == handler);
	}

	public static void On(dynamic nest, NestEvent type, Action<NestEvent, object, object?> handler)
		=> On(nest, new[] { type }, handler);

	public static void Once(dynamic nest, NestEvent type, Action<NestEvent, object, object?> handler)
		=> Once(nest, new[] { type }, handler);

	public static void Off(dynamic nest, NestEvent type, Action<NestEvent, object, object?> handler)
		=> Off(nest, new[] { type }, handler);

	//public static void Track(dynamic nest) => throw new NotImplementedException();

	public static ExpandoObject Target(dynamic nest)
	{
		if (nest is not NestInternals realNest)
			throw new NotANestException();

		dynamic expo = new ExpandoObject();

		foreach (var pair in realNest.Store)
			expo[pair.Key.ToString()] = pair.Value;

		return expo;
	}

	/*
	public static void Options(dynamic nest) => throw new NotImplementedException();
	public static void Path(dynamic    nest) => throw new NotImplementedException();
	*/

	public static bool IsNest(dynamic maybeNest) => maybeNest is NestInternals;

	// these are not part of the original nests api
	public static void Delete(dynamic nest, object key)
	{
		if (nest is not NestInternals realNest) throw new NotANestException();
		realNest.InternalDelete(key);
	}

	public static object[] Keys(dynamic nest)
	{
		if (nest is not NestInternals realNest) throw new NotANestException();
		return realNest.Store.Keys.ToArray();
	}

	public static object?[] Values(dynamic nest)
	{
		if (nest is not NestInternals realNest) throw new NotANestException();
		return realNest.Store.Values.ToArray();
	}

	public static (object, object?)[] Entries(dynamic nest)
	{
		if (nest is not NestInternals realNest) throw new NotANestException();
		return realNest.Store.Select(p => (p.Key, p.Value)).ToArray();
	}
}