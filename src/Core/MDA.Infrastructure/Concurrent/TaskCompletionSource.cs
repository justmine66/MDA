using System.Threading.Tasks;

namespace MDA.Infrastructure.Concurrent
{
    public sealed class TaskCompletionSource : TaskCompletionSource<int>
    {
        public static readonly TaskCompletionSource Void = CreateVoidTcs();

        public TaskCompletionSource(object state)
            : base(state)
        {
        }

        public TaskCompletionSource()
        {
        }

        public bool TryComplete() => TrySetResult(0);

        public void Complete() => SetResult(0);

        // todo: support cancellation token where used
        public bool SetUnCancellable() => true;

        public override string ToString() => "TaskCompletionSource[status: " + Task.Status + "]";

        static TaskCompletionSource CreateVoidTcs()
        {
            var tcs = new TaskCompletionSource();
            tcs.TryComplete();
            return tcs;
        }
    }
}
