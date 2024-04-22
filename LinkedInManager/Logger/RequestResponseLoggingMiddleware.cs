using LinkedInManager.Helper;
using System.Text;

namespace LinkedInManager.Logger
{
    public class RequestResponseLoggingMiddleware
    {
        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            Next = next;
            Logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
        }

        private RequestDelegate Next { get; }
        private ILogger Logger { get; }

        public async Task Invoke(HttpContext context)
        {
            if (Logger.IsEnabled(LogLevel.Debug))
            {
                context.Request.EnableBuffering();

                var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
                await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
                context.Request.Body.Seek(0, SeekOrigin.Begin);
                var requestBody = Encoding.UTF8.GetString(buffer);

                var sBuilder = new StringBuilder("");
                sBuilder.AppendLine();
                sBuilder.AppendLine("-------------------------------------------------------------");
                sBuilder.AppendLine(context.Request.Path + context.Request.QueryString);
                sBuilder.AppendLine();
                sBuilder.AppendLine($"Method: {context.Request.Method}");
                sBuilder.AppendLine($"Authorization: {context.Request.Headers.FirstOrDefault(c => c.Key == "Authorization").Value}");
                sBuilder.AppendLine();
                sBuilder.AppendLine("Body: ");
                sBuilder.AppendLine(requestBody.IsNullOrWhiteSpace()
                    ? ""
                    : requestBody.FormatJsonOrSelf().TruncateLongString(10000));

                var originalBodyStream = context.Response.Body;

                try
                {
                    using var responseBody = new MemoryStream();
                    context.Response.Body = responseBody;
                    await Next(context);

                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    var response = await new StreamReader(context.Response.Body).ReadToEndAsync();
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);

                    sBuilder.AppendLine();
                    sBuilder.AppendLine($"Response result '{context.Response.StatusCode}' and body:");
                    sBuilder.AppendLine(
                        response
                            .FormatJsonOrSelf()
                            .TruncateLongString(5000));

                    Logger.LogDebug(sBuilder.ToString());
                }
                catch (Exception exc)
                {
                    context.Response.Body = originalBodyStream;
                    var innerMessages = exc.UnfoldSingle(p => p.InnerException!)
                        .Select(p => p.Message)
                        .Join(Environment.NewLine);

                    sBuilder.AppendLine($"{exc.Message}{Environment.NewLine}Inner exception(s):{Environment.NewLine}{innerMessages}");
                    Logger.LogDebug(sBuilder.ToString());
                    throw;
                }
            }
            else
            {
                await Next(context);
            }
        }
    }
}
