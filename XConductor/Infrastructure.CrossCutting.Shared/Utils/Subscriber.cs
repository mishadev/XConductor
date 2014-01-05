using System;
using System.Collections.Generic;
using XConductor.Infrastructure.CrossCutting.Seedwork.Utils;

namespace XConductor.Infrastructure.CrossCutting.Shared.Utils
{
    public class Subscriber<T> : ISubscriber<T>
    {
        private List<ISubscribtion<T>> m_subscribtions = new List<ISubscribtion<T>>();

        public ISubscribtion<T> Add(ISubscribtion<T> subscriber)
        {
            this.m_subscribtions.Add(subscriber);

            return subscriber;
        }

        public void OnNext(T value)
        { 
            this.CleanUp();

            foreach (var subscriber in this.m_subscribtions)
	        {
                subscriber.OnNext(value);
	        } 
        }

        public void OnError(Exception exception)
        {
            this.CleanUp();

            foreach (var subscriber in this.m_subscribtions)
            {
                subscriber.OnError(exception);
            } 
        }

        public void OnCompleted()
        {
            this.CleanUp();

            foreach (var subscriber in this.m_subscribtions)
            {
                subscriber.OnCompleted();
            } 
        }

        protected void CleanUp()
        {
            this.m_subscribtions.RemoveAll(sub => sub.Disposed);
        }
    }
}
