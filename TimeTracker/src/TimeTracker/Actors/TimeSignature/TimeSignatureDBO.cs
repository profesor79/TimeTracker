namespace TimeTracker.Actors.TimeSignature
{
    public class TimeSignatureDBO
    {

        public TimeSignatureDBO(DateTime dateTime, Guid databaseId)
        {
            DateTime = dateTime;
            DatabaseId = databaseId;
        }

        public DateTime DateTime { get; }
        public Guid DatabaseId { get; }
    }
}
