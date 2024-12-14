namespace YahooFinance.Extensions
{
	public static class QueueExtensions
	{
        public static async Task<TResult> ExecuteWithQueueAsync<T, TResult>(this Queue<T> queue,
            Func<T, Task<TResult>> action)
        {
            var first = queue.Dequeue();
            try
            {
                return await action(first);
            }
            finally
            {
                queue.Enqueue(first);
            }
        }
    }
}