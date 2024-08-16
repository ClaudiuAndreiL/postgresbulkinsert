using BulkInsertAPI.Data.DbContexts;
using BulkInsertAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BulkInsertAPI.Controllers
{
    [ApiController]
    //   [Route("[controller]")]
    [Route("bulk")]
    public class BulkTrialsController : ControllerBase
    {
       
        private readonly ILogger<BulkTrialsController> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IBulkInsertService _bulkInsertService;

        public BulkTrialsController(
            ILogger<BulkTrialsController> logger,
            ApplicationDbContext applicationDbContext,
            IBulkInsertService bulkInsertService)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _bulkInsertService = bulkInsertService;
        }

        [HttpPost]
        [Route("ef-add-range")]
        public async Task<IActionResult> EfAddRange(int no = 1000, int repeat = 10)
        {
            var times = new List<double>();

            for(int i = 0; i < repeat; i++)
            {
                var messages = MessageDataGenerator.GetMessages(no);
                messages.ForEach(x => x.SentAt = DateTime.UtcNow);

                var messagesCount = await _applicationDbContext.Message.CountAsync();
                Console.WriteLine("Initial: " + messagesCount.ToString());

                var sw = Stopwatch.StartNew();
                await _applicationDbContext.Message.AddRangeAsync(messages);
                await _applicationDbContext.SaveChangesAsync();
                times.Add(sw.Elapsed.TotalMilliseconds);

                messagesCount = await _applicationDbContext.Message.CountAsync();
                Console.WriteLine($"Inserted: {no}. After: " + messagesCount.ToString() + $" and took  {times.Last()} ms");
            }

            Console.WriteLine($"Average: {times.Average()}");
            return new OkResult();
        }

        [HttpPost]
        [Route("sql-copy-text")]
        public async Task<IActionResult> SqlCopyText(int no = 1000, int repeat = 10)
        {
            var times = new List<double>();
            for (int i = 0; i < repeat; i++)
            {
                var messages = MessageDataGenerator.GetMessages(no);

                var messagesCount = await _applicationDbContext.Message.CountAsync();
                Console.WriteLine("Initial: " + messagesCount.ToString());
                var sw = Stopwatch.StartNew();
                await _bulkInsertService.PerformBulkInsertTextAsync(messages);
                times.Add(sw.Elapsed.TotalMilliseconds);
                messagesCount = await _applicationDbContext.Message.CountAsync();
                Console.WriteLine($"Inserted: {no}. After: " + messagesCount.ToString() + $" and took  {times.Last()} ms");
            }

            Console.WriteLine($"Average: {times.Average()}");
            return new OkResult();
        }

        [HttpPost]
        [Route("sql-copy-binary")]
        public async Task<IActionResult> SqlCopyBinary(int no = 1000, int repeat = 10)
        {
            var times = new List<double>();
            for (int i = 0; i < repeat; i++)
            {
                var messages = MessageDataGenerator.GetMessages(no);
                var messagesCount = await _applicationDbContext.Message.CountAsync();
                Console.WriteLine("Initial: " + messagesCount.ToString());
                var sw = Stopwatch.StartNew();
                await _bulkInsertService.PerformBulkInsertBinaryAsync(messages);
                times.Add(sw.Elapsed.TotalMilliseconds);
                messagesCount = await _applicationDbContext.Message.CountAsync();
                Console.WriteLine($"Inserted: {no}. After: " + messagesCount.ToString() + $" and took  {times.Last()} ms");
            }

            Console.WriteLine($"Average: {times.Average()}");
            return new OkResult();
        }
    }
}
