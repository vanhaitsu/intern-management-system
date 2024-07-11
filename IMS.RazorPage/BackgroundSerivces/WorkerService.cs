using System.Net.Mail;
using System.Net;
using IMS.Repositories.Entities;
using IMS.Services.Interfaces;
using IMS.Repositories.Models.InternModel;
using AutoMapper;

public class WorkerService : BackgroundService
{
    private readonly ILogger<WorkerService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;

    public WorkerService(IConfiguration configuration, ILogger<WorkerService> logger, IServiceScopeFactory scopeFactory)
    {
        _configuration = configuration;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ExecuteAsync started.");

        string MailServer = _configuration["EmailSettings:MailServer"];
        string FromEmail = _configuration["EmailSettings:FromEmail"];
        string Password = _configuration["EmailSettings:Password"];
        int Port = int.Parse(_configuration["EmailSettings:MailPort"]);
        string Subject = "Verify mail";

        _logger.LogInformation("Email configuration loaded. MailServer: {MailServer}, FromEmail: {FromEmail}, Port: {Port}",
            MailServer, FromEmail, Port);

        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var internService = scope.ServiceProvider.GetRequiredService<IInternService>();
                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                try
                {
                    _logger.LogInformation("Checking for registered users and emails to send.");

                    // Simulating a delay for the periodic task
                    await Task.Delay(TimeSpan.FromMinutes(0.3), stoppingToken);

                    List<Intern> registerIntern = await internService.GetRegisterCustomer();
                    _logger.LogInformation("Retrieved {UserCount} registered users.", registerIntern.Count);

                    foreach (var intern in registerIntern)
                    {
                        if (intern.ExpiredCode == null)
                        {
                            try
                            {
                                using var client = new SmtpClient(MailServer, Port)
                                {
                                    Credentials = new NetworkCredential(FromEmail, Password),
                                    EnableSsl = true,
                                };

                                var mailMessage = new MailMessage(FromEmail, intern.Email, Subject, intern.EmailVerifyCode.ToString())
                                {
                                    IsBodyHtml = true
                                };
                                await client.SendMailAsync(mailMessage);
                                _logger.LogInformation("Email sent to: {Email}", intern.Email);
                            }
                            catch (SmtpException smtpEx)
                            {
                                _logger.LogError(smtpEx, "SMTP error occurred while sending email to: {Email}", intern.Email);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "General error occurred while sending email to: {Email}", intern.Email);
                            }
                            intern.ExpiredCode = DateTime.Now.AddMinutes(3);
                            InternUpdateModel internUpdateModel = mapper.Map<InternUpdateModel>(intern);
                            await internService.Update(intern.Id, internUpdateModel);
                        }

                        if (intern.ExpiredCode < DateTime.Now)
                        {
                            internService.Delete(intern.Id);
                            _logger.LogInformation("Deleted user: {Email}", intern.Email);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred in ExecuteAsync.");
                }
            }
        }
        _logger.LogInformation("ExecuteAsync stopping.");
    }
}
