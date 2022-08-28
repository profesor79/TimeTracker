using Akka.Configuration;
using Akka.Actor.Dsl;
using Akka.TestKit.Xunit2;
using Xunit;
using Xunit.Abstractions;
using Akka.Actor;
using TimeTracker.Actors.TimeSignature;
using System;
using Moq;

namespace TimeTracker.Tests.TimeTracker
{
    public class TimeTrackerCommandActorTests : TestKit
    {
        private IActorRef _sut;

        [Fact]
        public void WhenReceiveTimeSignatureADBOObjectIsReturned()
        {
            // arrange/given
            var dt = DateTime.UtcNow;
            var guid = Guid.NewGuid();

            //mock
            var mock = new Mock<ICanStoreTimeSignatureData>();
            mock.Setup(writer => writer.Store(dt))
                .Returns(new TimeSignatureDBO(dt, guid));

            ICanStoreTimeSignatureData writer = mock.Object;
            var props = Props.Create(() => new TimeSignatureCommandActor(writer));
            _sut = Sys.ActorOf(props);


            // when
            var command = new AddTimeSignatureCommand(dt);
            _sut.Tell(command);


            //then
            ExpectMsg<TimeSignatureDBO>(a =>
           {
               Assert.True(a.DatabaseId == guid);
           }, TimeSpan.FromSeconds(180));

            // Verify that the given method was indeed called with the expected value at most once
            mock.Verify(library => library.Store(dt), Times.AtMostOnce());

        }

    }
}
