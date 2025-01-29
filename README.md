
# EventManager

A flexible and simple C# event management system that enables easy registration, unregistration, and execution of events with delegates. It supports dynamic event invocation with parameters and allows multiple event handlers for each event type. This system is ideal for a wide range of C# applications, including games, utilities, and any system that requires event-driven programming.

## Features

- **Register event handlers**: Dynamically add handlers for specific events.
- **Unregister event handlers**: Remove individual or all handlers for a given event.
- **Execute events with arguments**: Trigger an event and pass arguments to all registered handlers.
- **Support for multiple handlers**: Attach multiple handlers for a single event type, enabling complex event-driven workflows.
- **Dynamic method invocation**: Uses `DynamicInvoke` to call handlers with the necessary arguments at runtime.
- **Error Handling**: Handles missing event types and exceptions in event handlers.

## Table of Contents

1. [Installation](#installation)
2. [Getting Started](#getting-started)
   - [Registering an Event Handler](#registering-an-event-handler)
   - [Unregistering an Event Handler](#unregistering-an-event-handler)
   - [Executing an Event](#executing-an-event)
   - [Event Handling Example](#event-handling-example)
3. [Use Cases](#use-cases)
   - [Game Development](#game-development)
   - [Utility Applications](#utility-applications)
4. [Best Practices](#best-practices)
5. [Common Pitfalls](#common-pitfalls)
6. [License](#license)

## Installation

No installation is needed. Just include the `EventManager` class in your project, and you're ready to start using it. This is a lightweight system with no external dependencies, making it easy to integrate into any C# project.

```csharp
public static class EventManager
{
    private static Dictionary<string, Delegate> _eventDictionary = new();

    public static void Register(string eventType, Delegate eventHandler)
    {
        if (!_eventDictionary.ContainsKey(eventType))
        {
            _eventDictionary[eventType] = eventHandler;
        }
        else
        {
            _eventDictionary[eventType] = Delegate.Combine(_eventDictionary[eventType], eventHandler);
        }
    }

    public static void Unregister(string eventType, Delegate eventHandler)
    {
        if (_eventDictionary.ContainsKey(eventType))
        {
            var currentDelegate = _eventDictionary[eventType];
            currentDelegate = Delegate.Remove(currentDelegate, eventHandler);

            if (currentDelegate == null)
            {
                _eventDictionary.Remove(eventType);
            }
            else
            {
                _eventDictionary[eventType] = currentDelegate;
            }
        }
    }

    public static void Execute(string eventType, params object[] args)
    {
        if (_eventDictionary.ContainsKey(eventType))
        {
            _eventDictionary[eventType].DynamicInvoke(args);
        }
    }
}
```

## Getting Started

### Registering an Event Handler

To register an event handler for a particular event type, use the `Register` method. The first argument is a string that identifies the event type, and the second argument is a delegate representing the handler.

Example:

```csharp
EventManager.Register("OnPlayerDeath", OnPlayerDeathHandler);
```

This registers a handler `OnPlayerDeathHandler` for the event type `OnPlayerDeath`.

### Unregistering an Event Handler

To unregister a previously registered event handler, use the `Unregister` method. You need to pass the event type and the handler delegate that you want to remove.

Example:

```csharp
EventManager.Unregister("OnPlayerDeath", OnPlayerDeathHandler);
```

If there are no more handlers registered for the event, the event type will be removed from the `_eventDictionary`.

### Executing an Event

To execute an event and invoke all the registered handlers, use the `Execute` method. This method also supports passing parameters to event handlers, making it dynamic and flexible.

Example:

```csharp
EventManager.Execute("OnPlayerDeath", "Player1", 100);
```

This will invoke all the handlers registered for the `"OnPlayerDeath"` event, passing `"Player1"` and `100` as parameters.

### Event Handling Example

```csharp
using System;

public class Game
{
    // Event handler method
    public void OnPlayerDeathHandler(string playerName, int score)
    {
        Console.WriteLine($"Player {playerName} died with a score of {score}.");
    }

    public void ExampleUsage()
    {
        // Register the event handler
        EventManager.Register("OnPlayerDeath", OnPlayerDeathHandler);

        // Simulate player death and execute event
        Console.WriteLine("Simulating player death...");
        EventManager.Execute("OnPlayerDeath", "Player1", 100);

        // Unregister the event handler
        EventManager.Unregister("OnPlayerDeath", OnPlayerDeathHandler);
    }
}
```

In the above example, the `OnPlayerDeathHandler` will be executed when `Execute` is called, outputting the message: `"Player1 died with a score of 100."`

## Use Cases

### Game Development

Event-driven programming is especially useful in games where many things are happening simultaneously, and different components need to respond to these events. Here's a list of possible scenarios where `EventManager` can be applied:

- **Player death**: Trigger events when a player dies and let various systems (UI, achievements, score tracking) react.
- **Item pickup**: An item pickup event can notify the game inventory, UI, and other systems about the player’s action.
- **Game over**: When the game is over, trigger an event to handle game over logic like showing scores or loading a new scene.

### Utility Applications

`EventManager` can also be applied in utility applications that need to manage events across different components, like:

- **Logging system**: Register event handlers to log messages to files, consoles, or other systems.
- **Network events**: Handle connection status, data reception, or error events.
- **User interface updates**: Use events to notify UI components about changes in application state.

## Best Practices

- **Delegate Matching**: Always ensure that the handler delegate signature matches the arguments being passed by the event.
- **Event Cleanup**: Always unregister events when they are no longer needed, to avoid potential memory leaks.
- **Use Descriptive Event Names**: Event names like `"OnPlayerDeath"` or `"OnScoreChanged"` are much more descriptive than generic names like `"Event1"`.
- **Limit DynamicInvoke Usage**: While `DynamicInvoke` is convenient, consider defining specific delegate types for performance-critical code.

## Common Pitfalls

- **Missing Event Handlers**: If you try to execute an event that has no registered handlers, it will fail silently. Consider adding checks or logging to identify when an event has no handlers.
  
```csharp
if (!_eventDictionary.ContainsKey(eventType) || _eventDictionary[eventType] == null)
{
    Console.WriteLine($"No handlers for event: {eventType}");
}
```

- **Unregistering Handlers**: Always make sure you unregister event handlers when they are no longer required to prevent memory leaks. It's especially important in long-running applications like games.

- **Performance**: When using multiple handlers, ensure that you’re not unintentionally slowing down your event execution by chaining too many handlers for events that get triggered frequently.
