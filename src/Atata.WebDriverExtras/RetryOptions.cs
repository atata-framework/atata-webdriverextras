﻿using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace Atata
{
    public class RetryOptions
    {
        private static readonly Type StaleElementReferenceExceptionType = typeof(StaleElementReferenceException);

        private TimeSpan? timeout;

        private TimeSpan? interval;

        public TimeSpan Timeout
        {
            get => timeout ?? RetrySettings.Timeout;
            set => timeout = value;
        }

        public TimeSpan Interval
        {
            get => interval ?? RetrySettings.Interval;
            set => interval = value;
        }

        public List<Type> IgnoredExceptionTypes { get; set; } = new List<Type>();

        public RetryOptions WithTimeout(TimeSpan timeout)
        {
            Timeout = timeout;
            return this;
        }

        public RetryOptions WithInterval(TimeSpan interval)
        {
            Interval = interval;
            return this;
        }

        public RetryOptions IgnoringExceptionType(Type exceptionType)
        {
            IgnoredExceptionTypes.Add(exceptionType);
            return this;
        }

        public RetryOptions IgnoringStaleElementReferenceException()
        {
            IgnoredExceptionTypes.Add(StaleElementReferenceExceptionType);
            return this;
        }
    }
}