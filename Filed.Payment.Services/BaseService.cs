using Filed.Payments.Data.Models.Results;
using System;
using System.Collections.Generic;

namespace Filed.Payments.Services
{
    public class BaseService
    {
        /// <summary>
        /// Simulates retrieval of the current logged in user
        /// </summary>
        /// <returns></returns>
        public string GetLoggedInUser()
        {
            // Ideally should return current identity user
            return "Tim Udoma";
        }

        /// <summary>
        /// Creates a random payment result
        /// </summary>
        /// <returns></returns>
        public PaymentResult SimulateTransferResponse()
        {
            int[] responseArray = new int[] { 200, 400, 500 };
            var responses = new Dictionary<int, string>();
            responses.Add(200, "Payment is processed");
            responses.Add(400, "The request is invalid");
            responses.Add(500, "An error occurred");

            var rnd = new Random();
            int index = rnd.Next(0,3);

            string responseMessage;

            if (!responses.TryGetValue(responseArray[index], out responseMessage))
            {
                return new PaymentResult
                {
                    Message = responses[500],
                    StatusCode = responseArray[index]
                };
            }

            var response = new PaymentResult
            {
                Message = responseMessage,
                StatusCode = responseArray[index]
            };

            return response;
        }
    }
}
