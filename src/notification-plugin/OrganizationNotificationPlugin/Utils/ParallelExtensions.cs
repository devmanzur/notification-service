using System.Threading.Tasks.Dataflow;

namespace OrganizationNotificationPlugin.Utils;

public static class ParallelExtensions
{

    public static Task ParallelForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> body,
        double maxDop = 0.75, TaskScheduler? scheduler = null)
    {
        var threadCount = Environment.ProcessorCount * 2.0;
        var options = new ExecutionDataflowBlockOptions
        {
            MaxDegreeOfParallelism = Convert.ToInt32(Math.Ceiling(threadCount * maxDop))
        };
        if (scheduler != null)
            options.TaskScheduler = scheduler;

        var block = new ActionBlock<T>(body, options);

        foreach (var item in source)
            block.Post(item);

        block.Complete();
        return block.Completion;
    }
}