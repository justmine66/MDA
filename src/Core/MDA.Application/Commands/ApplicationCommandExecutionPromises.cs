using System.Threading;
using System.Threading.Tasks;

namespace MDA.Application.Commands
{
    public class ApplicationCommandExecutionPromise
    {
        private readonly TaskCompletionSource<ApplicationCommandResult> _completionSource;

        public ApplicationCommandExecutionPromise(IApplicationCommand command)
        {
            ApplicationCommand = command;
            _completionSource = new TaskCompletionSource<ApplicationCommandResult>();
        }

        public IApplicationCommand ApplicationCommand { get; }

        public Task<ApplicationCommandResult> Future => _completionSource.Task;

        public bool TrySetResult(ApplicationCommandResult result) => _completionSource.TrySetResult(result);

        public bool TrySetCanceled(CancellationToken token) => _completionSource.TrySetCanceled(token);
    }

    public class ApplicationCommandExecutionPromise<TPayload> : ApplicationCommandExecutionPromise
    {
        private readonly TaskCompletionSource<ApplicationCommandResult<TPayload>> _completionSource;

        public ApplicationCommandExecutionPromise(IApplicationCommand command) : base(command)
        {
            _completionSource = new TaskCompletionSource<ApplicationCommandResult<TPayload>>();
        }


        public new Task<ApplicationCommandResult<TPayload>> Future => _completionSource.Task;

        public bool TrySetResult(ApplicationCommandResult<TPayload> result) => _completionSource.TrySetResult(result);
    }
}