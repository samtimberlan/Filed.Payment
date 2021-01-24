using Filed.Payment.Infrastructure.Config;
using Filed.Payment.Interfaces;
using Filed.Payment.Services.Mappings;
using Filed.Payments.Data;
using Filed.Payments.Data.Entities;
using Filed.Payments.Data.Models;
using Filed.Payments.Data.Models.Results;
using Filed.Payments.Infrastructure.Response;
using Filed.Payments.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Filed.Payments.Services
{
    public class PaymentService : BaseService, IPaymentService
    {
        private readonly ResponseFactory _responseFactory;
        private readonly ILogger<PaymentService> _logger;
        private readonly ICheapPaymentGateway _cheapPaymentGateway;
        private readonly IExpensivePaymentGateway _expensivePaymentGateway;
        private readonly ICommandRepository<Data.Entities.Payment> _commandRepostory;
        private readonly ICommandRepository<PaymentState> _paymentStateCommandRepostory;
        private readonly AppConfig _config;

        public PaymentService(ResponseFactory responseFactory, ILogger<PaymentService> logger, ICheapPaymentGateway cheapPaymentGateway, IExpensivePaymentGateway expensivePaymentGateway, IOptions<AppConfig> config, ICommandRepository<Payments.Data.Entities.Payment> commandRepostory, ICommandRepository<PaymentState> paymentStateCommandRepostory)
        {
            _responseFactory = responseFactory;
            _logger = logger;
            _cheapPaymentGateway = cheapPaymentGateway;
            _expensivePaymentGateway = expensivePaymentGateway;
            _commandRepostory = commandRepostory;
            _paymentStateCommandRepostory = paymentStateCommandRepostory;
            _config = config.Value;
        }

        /// <summary>
        /// Processes a payment request
        /// </summary>
        /// <param name="paymentModel"></param>
        /// <returns></returns>
        public async Task<IExecutionResponse<PaymentResult>> ProcessPaymentAsync(PaymentModel paymentModel)
        {
            _logger.LogInformation($"Processing payment...");
            PaymentResult paymentResult;
            string userName = GetLoggedInUser();

            if (paymentModel.Amount < 0)
            {
                _logger.LogInformation("Cannot process payment for amounts less than 0");
                paymentResult = PaymentResultMapping.CreateEntity(400, "An error occured");

                return _responseFactory.ExecutionResponse<PaymentResult>("Cannot process payment for amounts less than 0", paymentResult, statusCode: 400);
            }

            int thirdPartyResponseCode = 500;

                if (paymentModel.Amount < 20)
                {
                    // Use ICheapPaymentGateway. No retry
                    thirdPartyResponseCode = _cheapPaymentGateway.BankTransfer(paymentModel).StatusCode;
                }
                else if (paymentModel.Amount >= 21 && paymentModel.Amount <= 500)
                {
                    // Use IExpensivePaymentGateway and retry once with ICheapPaymentGateway
                    thirdPartyResponseCode = RetryWithCheapPaymentGateway(paymentModel);
                }
                else if (paymentModel.Amount > 500)
                {
                    // Use PremiumPaymentService and retry 3 times
                    thirdPartyResponseCode = RetryThriceWithPremiumGateway(paymentModel);
                }
                
            var payment = PaymentMapping.CreateEntity(paymentModel, userName, userName);

            // Map response from third party
            var paymentState = UpdatePaymentStateFromThirdParty(payment, thirdPartyResponseCode);

            // Save to DB
            _commandRepostory.Add(payment);
            _paymentStateCommandRepostory.Add(paymentState);
            await _commandRepostory.SaveAsync();

            _logger.LogInformation("Payment processed successfully.");
            paymentResult = PaymentResultMapping.CreateEntity(200, "Payment is processed");
            return _responseFactory.ExecutionResponse<PaymentResult>("Payment processed successfully.", paymentResult, status: true);
        }

        
        /// <summary>
        /// Helper method to retry payment with cheap gateway, if model matches business requirements
        /// </summary>
        /// <param name="paymentModel"></param>
        /// <returns></returns>
        private int RetryWithCheapPaymentGateway(PaymentModel paymentModel)
        {
            var expensiveGatewayResponseCode = _expensivePaymentGateway.BankTransfer(paymentModel).StatusCode;

            if (expensiveGatewayResponseCode == _config.ThirdPartyErrorResponse)
            {
                // Service is unavailable. Retry with cheap gateway
                int thirdPartyResponseCode = _cheapPaymentGateway.BankTransfer(paymentModel).StatusCode;

                return thirdPartyResponseCode;
            }
            else
            {
                return expensiveGatewayResponseCode;
            }

        }

        /// <summary>
        /// Helper method to retry payment with premium service, if model matches business requirements
        /// </summary>
        /// <param name="paymentModel"></param>
        /// <returns></returns>
        private int RetryThriceWithPremiumGateway(PaymentModel paymentModel)
        {
            int premiumGatewayResponseCode = BankTransfer(paymentModel).StatusCode;

            // Keep calling BankTransfer if response is error

            int maxRetries = 3;
            for (int i = 1; i <= maxRetries; i++)
            {
                if (premiumGatewayResponseCode != _config.ThirdPartyErrorResponse)
                {
                    return premiumGatewayResponseCode;
                }
                else
                {
                    premiumGatewayResponseCode = BankTransfer(paymentModel).StatusCode;
                }
            }
            return premiumGatewayResponseCode;
        }

        /// <summary>
        /// Simulates a bank transfer with a specified payment model
        /// </summary>
        /// <param name="paymentModel"></param>
        /// <returns></returns>
        private PaymentResult BankTransfer(PaymentModel paymentModel)
        {
            return SimulateTransferResponse();
        }

        /// <summary>
        /// Updates payment state after making a call to payment services
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="thirdPartyResponseCode"></param>
        /// <returns></returns>
        private PaymentState UpdatePaymentStateFromThirdParty(Data.Entities.Payment payment, int thirdPartyResponseCode)
        {
            string userName = GetLoggedInUser();

            // Map third party response to internal response, per business requirement
            if (thirdPartyResponseCode == _config.ThirdPartyProcessedResponse)
            {
                payment.PaymentState = PaymentStateMapping.CreateEntity(payment, PaymentStatus.Processed, userName, userName);
            }
            else if (thirdPartyResponseCode == _config.ThirdPartyInvalidResponse)
            {
                payment.PaymentState = PaymentStateMapping.CreateEntity(payment, PaymentStatus.Invalid, userName, userName);
            }
            else
            {
                payment.PaymentState = PaymentStateMapping.CreateEntity(payment, PaymentStatus.Error, userName, userName);
            }
            return payment.PaymentState;
        }
    }
}
