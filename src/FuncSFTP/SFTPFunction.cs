using FuncSFTP.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Renci.SshNet;
using System.IO;
using System.Threading.Tasks;

namespace FuncSFTP
{
    public class SFTPFunction
    {
        private readonly ILogger<SFTPFunction> _log;
        private readonly SFTPOptions _options;

        public SFTPFunction(ILogger<SFTPFunction> log, IOptions<SFTPOptions> options)
        {
            _log = log;
            _options = options.Value;
        }

        [FunctionName("GetFileFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "file")] HttpRequest req)
        {
            _log.LogInformation("Get file using SFTP.");

            string json = await new StreamReader(req.Body).ReadToEndAsync();
            var getFileRequest = JsonConvert.DeserializeObject<GetFileRequest>(json);

            using var client = new SftpClient(_options.Host, _options.Username, _options.Password);
            client.Connect();

            // Note: FileStreamResult does close the underlying stream automatically.
            var memoryStream = new MemoryStream();
            client.DownloadFile(getFileRequest.Path, memoryStream);
            memoryStream.Position = 0;
            
            return new FileStreamResult(memoryStream, "application/octet-stream");
        }
    }
}

