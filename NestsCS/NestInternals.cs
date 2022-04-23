using System.Dynamic;
using NestsCS.Exceptions;

namespace NestsCS;

public class NestInternals : DynamicObject
{
	internal NestInternals(object init)
	{
		foreach (var prop in init.GetType().GetProperties())
			Store[prop] = prop.GetValue(init);
	}

	internal readonly Dictionary<object, object?> Store = new();

	internal readonly List<(HashSet<NestEvent>, Action<(NestEvent type, object key, object? value)>)> EventHandlers
		= new();

	internal bool FireEvents = true;

	private object? InternalGet(object key)
	{
		if (!Store.ContainsKey(key)) throw new MissingKeyException(key);

		return Store[key];
	}

	private void InternalSet(object key, object? value)
	{
		Store[key] = value;

		if (FireEvents)
			foreach (var handler in from h in EventHandlers where h.Item1.Contains(NestEvent.SET) select h.Item2)
				handler((NestEvent.SET, key, value));
	}

	internal void InternalDelete(object key)
	{
		var oldValue = InternalGet(key);
		Store.Remove(key);

		if (FireEvents)
			foreach (var handler in from h in EventHandlers where h.Item1.Contains(NestEvent.DELETE) select h.Item2)
				handler((NestEvent.DELETE, key, oldValue));
	}

	// dynamic nest.x style gets
	public override bool TryGetMember(GetMemberBinder binder, out object? result)
	{
		result = InternalGet(binder.Name);
		return true;
	}

	// dynamic nest.x style sets
	public override bool TrySetMember(SetMemberBinder binder, object? value)
	{
		InternalSet(binder.Name, value);
		return true;
	}

	// standard nest["x"] style gets/sets. Can be done via dynamic binding but this is WAY cleaner.
	// ReSharper disable once UnusedMember.Global
	public object? this[object k]
	{
		get => InternalGet(k);
		set => InternalSet(k, value);
	}
}