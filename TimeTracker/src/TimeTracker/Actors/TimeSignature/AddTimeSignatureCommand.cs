namespace TimeTracker.Actors.TimeSignature
{
    public class AddTimeSignatureCommand
    {

        public AddTimeSignatureCommand(DateTime dateTime)
        {
            DateTime = dateTime;
        }

        public DateTime DateTime { get; }
    }
}
