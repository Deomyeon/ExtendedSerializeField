SerializeField Extension
=========================

<br>

## Available Types
#### - [System.Collections.Stack](https://learn.microsoft.com/ko-kr/dotnet/api/system.collections.stack?view=net-8.0)
#### - [System.Collections.Queue](https://learn.microsoft.com/ko-kr/dotnet/api/system.collections.queue?view=net-8.0)
#### - [System.Collections.Generic.Stack<>](https://learn.microsoft.com/ko-kr/dotnet/api/system.collections.generic.stack-1?view=net-8.0)
#### - [System.Collections.Generic.Queue<>](https://learn.microsoft.com/ko-kr/dotnet/api/system.collections.generic.queue-1?view=net-8.0)
#### - [System.Collections.ArrayList](https://learn.microsoft.com/ko-kr/dotnet/api/system.collections.arraylist?view=net-8.0)
#### - [System.Collections.Generic.LinkedList<>](https://learn.microsoft.com/ko-kr/dotnet/api/system.collections.generic.linkedlist-1?view=net-8.0)
#### - [System.Collections.Generic.HashSet<>](https://learn.microsoft.com/ko-kr/dotnet/api/system.collections.generic.hashset-1?view=net-8.0)
#### - [System.Collections.Hashtable](https://learn.microsoft.com/ko-kr/dotnet/api/system.collections.hashtable?view=net-8.0)
#### - [System.Collections.Generic.Dictionary<,>](https://learn.microsoft.com/ko-kr/dotnet/api/system.collections.generic.dictionary-2?view=net-8.0)

<br>

## Available Element Types
#### - int
#### - long
#### - string
#### - float
#### - double
#### - Vector2
#### - Vector3
#### - Vector4
#### - Color
#### - Rect
#### - RectInt
#### - Unity.Object castable type  (MonoBehaviour, GameObject, Transform, Button, Scene, ...)

<br><br>

##### Property is not supported. Only support Field data.


<br><br><br>

## Usage Example

<br>

```cs

[ExSerializeField]
public Stack stack1; // Valid

[ExSerializeField]
private Stack stack2; // Valid

[ExSerializeField]
public Queue queue1; // Valid

[ExSerializeField]
private Stack<int> stackGeneric1; // Valid

[ExSerializeField]
Queue<string> queueGeneric1; // Valid

[ExSerializeField]
LinkedList<MonoBehaviour> linkedList1; // Now Valid

[ExSerializeField]
CustomCollection<string> customCollection; // InValid

[ExSerializeField]
Dictionary<string, Vector2> dictionary1; // Valid

```
