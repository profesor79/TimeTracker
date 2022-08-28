using Akka.Hosting;
using Akka.Actor;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Actors.TimeSignature;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSignatureController : ControllerBase
    {

        // GET: api/<TimeSignatureController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TimeSignatureController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TimeSignatureController>
        [HttpPost]
        public async Task<TimeSignatureDBO> PostAsync([FromBody] AddTimeSignatureCommand dateTime)
        {

            var reporter = ActorSystemRefernce.System.ActorOf(Props.Create(() => new TimeSignatureCommandActor(new CanStoreTimeSignatureDataAslist())));
            var resp = await reporter.Ask<TimeSignatureDBO>(new AddTimeSignatureCommand(dateTime.DateTime));
            reporter.Tell(PoisonPill.Instance);
            return resp;
        }

        // PUT api/<TimeSignatureController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TimeSignatureController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
