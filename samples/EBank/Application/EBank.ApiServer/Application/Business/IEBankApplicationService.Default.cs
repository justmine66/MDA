﻿using EBank.ApiServer.Infrastructure.Exceptions;
using EBank.Application.Commands.Accounts;
using EBank.Application.Commands.Depositing;
using EBank.Application.Commands.Transferring;
using EBank.Application.Commands.Withdrawing;
using MDA.Application.Commands;
using MDA.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace EBank.ApiServer.Application.Business
{
    public class DefaultEBankApplicationService : IEBankApplicationService
    {
        private readonly IApplicationCommandService _commandService;

        public DefaultEBankApplicationService(IApplicationCommandService commandService)
                => _commandService = commandService;

        /// <summary>
        /// 开户
        /// </summary>
        /// <param name="command"></param>
        /// <param name="token"></param>
        public async Task<ApplicationCommandResult> OpenAccountAsync(OpenBankAccountApplicationCommand command, CancellationToken token = default)
        {
            return await _commandService.ExecuteCommandAsync(command, token);
        }

        /// <summary>
        /// 变更账户名
        /// </summary>
        /// <param name="command"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task ChangeAccountNameAsync(ChangeAccountNameApplicationCommand command, CancellationToken token = default)
        {
            var result = await _commandService.ExecuteCommandAsync(command, ApplicationCommandReplySchemes.OnDomainEventHandled, token);
            if (!result.Succeed())
            {
                throw new ApiDomainException(result);
            }
        }

        /// <summary>
        /// 存款
        /// </summary>
        /// <param name="command"></param>
        /// <param name="token"></param>
        public async Task DepositedFundsAsync(StartDepositApplicationCommand command, CancellationToken token = default)
        {
            var result = await _commandService.ExecuteCommandAsync(command, token);
            if (!result.Succeed())
            {
                throw new ApiDomainException(result);
            }
        }

        /// <summary>
        /// 取款
        /// </summary>
        /// <param name="command"></param>
        /// <param name="token"></param>
        public async Task WithdrawFundsAsync(StartWithdrawApplicationCommand command, CancellationToken token = default)
        {
            var result = await _commandService.ExecuteCommandAsync(command, token);
            if (!result.Succeed())
            {
                throw new ApiDomainException(result);
            }
        }

        /// <summary>
        /// 转账
        /// </summary>
        /// <param name="command"></param>
        /// <param name="token"></param>
        public async Task TransferFundsAsync(StartTransferApplicationCommand command, CancellationToken token = default)
        {
            await _commandService.PublishAsync(command, token);
        }
    }
}
