using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xunit;

namespace TekConf.Mobile.Core.Tests.Extensions
{
    public class NotifyExpectation<T> where T : INotifyPropertyChanged
    {
        private readonly T owner;
        private readonly string propertyName;
        private readonly bool eventExpected;

        public NotifyExpectation(T owner, string propertyName, bool eventExpected)
        {
            this.owner = owner;
            this.propertyName = propertyName;
            this.eventExpected = eventExpected;
        }

        //public async void When(Task task)
        //{
        //    bool eventWasRaised = false;
        //    this.owner.PropertyChanged += (sender, e) =>
        //    {
        //        if (e.PropertyName == this.propertyName)
        //        {
        //            eventWasRaised = true;
        //        }
        //    };
        //    task.Wait();
        //    //await task(owner);

        //    Assert.Equal(this.eventExpected, eventWasRaised);
        //    //Assert.AreEqual<bool>(this.eventExpected,
        //    //    eventWasRaised,
        //    //    "PropertyChanged on {0}", this.propertyName);
        //}
        public void When(Action<T> action)
        {
            bool eventWasRaised = false;
            this.owner.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == this.propertyName)
                {
                    eventWasRaised = true;
                }
            };

            action(this.owner);

            Assert.Equal(this.eventExpected, eventWasRaised);
        }
    }
}