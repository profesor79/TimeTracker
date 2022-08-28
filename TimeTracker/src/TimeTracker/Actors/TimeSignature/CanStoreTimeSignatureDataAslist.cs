﻿namespace TimeTracker.Actors.TimeSignature
{
    public class CanStoreTimeSignatureDataAslist : ICanStoreTimeSignatureData
    {
        public TimeSignatureDBO Store(DateTime datetime)
        {
            return new TimeSignatureDBO(datetime, Guid.NewGuid());
        }
    }
}
