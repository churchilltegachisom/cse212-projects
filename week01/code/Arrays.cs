using System;
using System.Collections.Generic;

public static class Arrays
{
    /*
     * PART 1: MultiplesOf
     * -------------------
     * Plan (step-by-step comments as required):
     * 1. Validate inputs: count must be non-negative.
     * 2. If count == 0, return an empty array.
     * 3. Create a result array of size count.
     * 4. Loop i from 0 to count - 1:
     *      - Compute start * (i + 1).
     *      - Store it in result[i].
     * 5. Return the result array.
     */
    public static double[] MultiplesOf(double start, int count)
    {
        if (count < 0)
        {
            throw new ArgumentException("count must be non-negative", nameof(count));
        }

        if (count == 0)
        {
            return new double[0];
        }

        double[] result = new double[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = start * (i + 1);
        }

        return result;
    }

    /*
     * PART 2: RotateListRight
     * -----------------------
     * Plan (step-by-step comments as required):
     * 1. Validate inputs: data must not be null.
     * 2. If the list is empty or has 1 element, no rotation needed.
     * 3. Normalize the amount: r = amount % n, ensure r >= 0.
     * 4. If r == 0, no rotation needed.
     * 5. Split the list into two slices:
     *      - tail = last r elements
     *      - head = first (n - r) elements
     * 6. Clear the list and add tail followed by head.
     */
    public static void RotateListRight<T>(List<T> data, int amount)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        int n = data.Count;
        if (n <= 1)
        {
            return;
        }

        int r = amount % n;
        if (r < 0) r += n;
        if (r == 0) return;

        int split = n - r;
        List<T> tail = data.GetRange(split, r);
        List<T> head = data.GetRange(0, split);

        data.Clear();
        data.AddRange(tail);
        data.AddRange(head);
    }
}
