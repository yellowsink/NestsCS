# NestsCS

A port of [kyza/nests v4](https://github.com/kyza/nests/tree/v4) to C#, released before nests v4 as well because why not.

## What is nests

Nests are data stores that not only hold data like a Dictionary, but pretend to be objects via dynamic binding.

They can have properties freely added and deleted, and accept event handlers.

All interaction is done via the `NestsCS.Nest` static class members.

## Docs

// TODO

## Example

```cs
using NestsCS;


var myNest = Nest.make(new { foo: "Hello, World!", bar: 5, baz: false });
Nest.on(myNest, NestEvent.SET,
    ((NestEvent type, object key, object? value)) => {
    if (key == "foo")
        Console.WriteLine($"Foo was written to: {value}");
    }
);

myNest.foo = "Goodbye, World!";
// [console] Foo was written to: Goodbye, World!
Nest.Silent(myNest);
myNest.foo = "cheese";
// no log
Nest.Loud(myNest);
myNest.bar = true; // datatypes need not be consistent
// handler called but no log
```


