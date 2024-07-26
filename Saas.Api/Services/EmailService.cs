using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace Saas.Api.Services
{
    public class EmailService(ILogger<EmailService> _logger) : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string fromEmail, string subject, string body)
        {
            try
            {
                var region = RegionEndpoint.USEast2;
                var client = new AmazonSimpleEmailServiceClient(region);

                var sendRequest = new SendEmailRequest
                {
                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { toEmail }
                    },
                    Message = new Message
                    {
                        Body = new Body
                        {
                            Text = new Content
                            {
                                Data = body
                            }
                        },
                        Subject = new Content
                        {
                            Data = subject
                        }
                    },
                    Source = fromEmail
                };

                await client.SendEmailAsync(sendRequest);
            }
            catch (AmazonSimpleEmailServiceException ex)
            {
                _logger.LogError($"SendEmailAsync : {ex.Message + ex.InnerException?.Message}");
                throw;
            }
        }
    }    
}