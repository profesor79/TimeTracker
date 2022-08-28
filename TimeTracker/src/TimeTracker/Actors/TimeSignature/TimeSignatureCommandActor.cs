using Akka.Actor;

namespace TimeTracker.Actors.TimeSignature
{
    public class TimeSignatureCommandActor : ReceiveActor
    {
        private readonly ICanStoreTimeSignatureData canStoreTimeSignatureData;

        public TimeSignatureCommandActor(ICanStoreTimeSignatureData canStoreTimeSignatureData)
        {
            this.canStoreTimeSignatureData = canStoreTimeSignatureData;
            Receive<AddTimeSignatureCommand>(SaveData);

        }

        private void SaveData(AddTimeSignatureCommand cmd)
        {
            var response = canStoreTimeSignatureData.Store(cmd.DateTime);
            Sender.Tell(response);
        }
    }
}
