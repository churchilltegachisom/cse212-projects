using System;
using System.Collections.Generic;

/// <summary>
/// Maintains a circular queue of people with a number of turns each.
/// - AddPerson(name, turns): adds a person with the given original turns.
///    * turns <= 0 is considered "infinite" turns (never consumed).
/// - GetNextPerson(): dequeues the next person and returns a Person object with the original Turns value.
///    * For finite-turn players, the remaining count is decremented and re-enqueued only if they still have remaining turns (> 0).
///    * For infinite-turn players (turns <= 0), they are always re-enqueued and their original Turns value is preserved.
/// - Length property returns current number of entries in the queue.
/// - If the queue is empty, GetNextPerson throws InvalidOperationException with message "No one in the queue."
/// </summary>
public class TakingTurnsQueue
{
    // Internal queue entry tracks original turns and remaining finite turns separately
    private class Entry
    {
        public string Name { get; }
        public int OriginalTurns { get; }
        // For finite players, Remaining is the number of times left to be returned.
        // For infinite players (OriginalTurns <= 0) this value is unused.
        public int Remaining;

        public bool IsInfinite => OriginalTurns <= 0;

        public Entry(string name, int originalTurns)
        {
            Name = name;
            OriginalTurns = originalTurns;
            Remaining = originalTurns > 0 ? originalTurns : 0;
        }
    }

    private readonly Queue<Entry> _queue;

    public TakingTurnsQueue()
    {
        _queue = new Queue<Entry>();
    }

    /// <summary>
    /// Number of entries currently in the queue.
    /// </summary>
    public int Length => _queue.Count;

    /// <summary>
    /// Adds a person to the end of the queue.
    /// </summary>
    /// <param name="name">Person's name</param>
    /// <param name="turns">Number of turns (0 or less means infinite)</param>
    public void AddPerson(string name, int turns)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));
        var entry = new Entry(name, turns);
        _queue.Enqueue(entry);
    }

    /// <summary>
    /// Dequeues and returns the next person. If the person still has remaining turns (or infinite),
    /// they will be re-enqueued per the requirements.
    /// Throws InvalidOperationException with message "No one in the queue." if there are no entries.
    /// </summary>
    public Person GetNextPerson()
    {
        if (_queue.Count == 0)
        {
            throw new InvalidOperationException("No one in the queue.");
        }

        Entry e = _queue.Dequeue();

        // Build and return a Person object with the original turns value preserved.
        Person returned = new Person(e.Name, e.OriginalTurns);

        if (e.IsInfinite)
        {
            // Re-enqueue unchanged for infinite-turn players
            _queue.Enqueue(e);
        }
        else
        {
            // Decrement remaining and re-enqueue only if still have > 0 remaining
            e.Remaining--;
            if (e.Remaining > 0)
            {
                _queue.Enqueue(e);
            }
            // else do not re-enqueue; person's finite quota exhausted
        }

        return returned;
    }
}
