using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

[TestClass]
public class PriorityQueue_Tests
{
    [TestMethod]
    // Scenario: Enqueue three items in order: A (1), B (2), C (1)
    // Expected Result: The internal order (ToString) should reflect the enqueue order:
    // "[A (Pri:1), B (Pri:2), C (Pri:1)]"
    // Defect(s) Found: None for Enqueue itself — items must be appended to the back. (This test verifies enqueue/back behavior.)
    public void TestEnqueue_MaintainsOrder()
    {
        var pq = new PriorityQueue();
        pq.Enqueue("A", 1);
        pq.Enqueue("B", 2);
        pq.Enqueue("C", 1);

        string expected = "[A (Pri:1), B (Pri:2), C (Pri:1)]";
        Assert.AreEqual(expected, pq.ToString());
    }

    [TestMethod]
    // Scenario: Enqueue A (1), B (3), C (2) and Dequeue once.
    // Expected Result: Dequeue should return "B" (highest priority) and the queue should then contain A and C in that order.
    // Defect(s) Found: 
    //  - Original code did not remove the item from the queue after returning it.
    //  - A scanning loop bug could omit the last element from consideration (fixed by using correct loop bounds).
    public void TestDequeue_RemovesHighestPriority()
    {
        var pq = new PriorityQueue();
        pq.Enqueue("A", 1);
        pq.Enqueue("B", 3);
        pq.Enqueue("C", 2);

        var removed = pq.Dequeue();
        Assert.AreEqual("B", removed);

        string expectedAfter = "[A (Pri:1), C (Pri:2)]";
        Assert.AreEqual(expectedAfter, pq.ToString());
    }

    [TestMethod]
    // Scenario: Enqueue items where two items share the highest priority: A (2), B (3), C (3), D (1)
    // Expected Result:
    //  - First Dequeue returns "B" (the earliest inserted item among the highest priority items).
    //  - Second Dequeue returns "C".
    // Defect(s) Found:
    //  - Original code used >= when selecting the high-priority item which favored the later item on ties. This breaks FIFO among equals.
    public void TestDequeue_FIFOAmongEqualPriorities()
    {
        var pq = new PriorityQueue();
        pq.Enqueue("A", 2);
        pq.Enqueue("B", 3);
        pq.Enqueue("C", 3);
        pq.Enqueue("D", 1);

        var first = pq.Dequeue();
        Assert.AreEqual("B", first); // B is first among highest-priority items

        var second = pq.Dequeue();
        Assert.AreEqual("C", second);
    }

    [TestMethod]
    // Scenario: Attempt to Dequeue from an empty queue.
    // Expected Result: InvalidOperationException thrown with message "The queue is empty."
    // Defect(s) Found: Original code threw an error if empty? The provided code already threw InvalidOperationException with the correct message.
    public void TestDequeue_EmptyThrows()
    {
        var pq = new PriorityQueue();
        try
        {
            pq.Dequeue();
            Assert.Fail("Expected InvalidOperationException was not thrown.");
        }
        catch (InvalidOperationException e)
        {
            Assert.AreEqual("The queue is empty.", e.Message);
        }
        catch (AssertFailedException)
        {
            throw;
        }
        catch (Exception e)
        {
            Assert.Fail($"Unexpected exception of type {e.GetType()} caught: {e.Message}");
        }
    }

    [TestMethod]
    // Scenario: Enqueue A(1), B(2), C(1), then Dequeue repeatedly to ensure the queue evolves correctly
    // Expected Result sequence of removed items: B, A, C
    // Defect(s) Found: This verifies both removal and order of remainders after removal.
    public void TestDequeue_SequenceAfterRemove()
    {
        var pq = new PriorityQueue();
        pq.Enqueue("A", 1);
        pq.Enqueue("B", 2);
        pq.Enqueue("C", 1);

        var first = pq.Dequeue();
        Assert.AreEqual("B", first);

        var second = pq.Dequeue();
        Assert.AreEqual("A", second);

        var third = pq.Dequeue();
        Assert.AreEqual("C", third);
    }
}
