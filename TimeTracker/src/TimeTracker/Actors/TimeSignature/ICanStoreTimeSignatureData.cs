namespace TimeTracker.Actors.TimeSignature
{
    public interface ICanStoreTimeSignatureData
    {
        TimeSignatureDBO Store(DateTime datetime);
    }
}
