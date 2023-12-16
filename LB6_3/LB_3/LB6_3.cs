using System;
using System.Collections.Generic;

public class FunctionCache<TKey, TResult>
{
    private readonly Dictionary<TKey, CacheItem> cache = new Dictionary<TKey, CacheItem>();
    private readonly Func<TKey, TResult> function;

    public delegate TResult FuncDelegate(TKey key);

    public FunctionCache(Func<TKey, TResult> function)
    {
        this.function = function ?? throw new ArgumentNullException(nameof(function));
    }

    public TResult GetResult(TKey key, TimeSpan cacheDuration)
    {
        if (cache.TryGetValue(key, out var cacheItem) && IsCacheValid(cacheItem, cacheDuration))
        {
            return cacheItem.Result;
        }

        TResult result = function(key);
        cache[key] = new CacheItem(result, DateTime.UtcNow);
        return result;
    }

    private bool IsCacheValid(CacheItem cacheItem, TimeSpan cacheDuration)
    {
        return DateTime.UtcNow - cacheItem.Timestamp <= cacheDuration;
    }

    private class CacheItem
    {
        public TResult Result { get; }
        public DateTime Timestamp { get; }

        public CacheItem(TResult result, DateTime timestamp)
        {
            Result = result;
            Timestamp = timestamp;
        }
    }
}
class Program
{
    static void Main()
    {
        FunctionCache<string, int> cache = new FunctionCache<string, int>(StringLengthFunction);

        string input = "Hello!";
        TimeSpan cacheDuration = TimeSpan.FromMinutes(1);

        int result1 = cache.GetResult(input, cacheDuration);
        Console.WriteLine($"Result 1: {result1}");

        
        System.Threading.Thread.Sleep(2000);

        int result2 = cache.GetResult(input, cacheDuration);
        Console.WriteLine($"Result 2: {result2}");
    }

    static int StringLengthFunction(string input)
    {
        Console.WriteLine("Executing function...");
        return input.Length;
    }
}
