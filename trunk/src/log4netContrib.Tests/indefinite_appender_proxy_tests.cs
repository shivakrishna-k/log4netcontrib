﻿#region licence
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using log4net.Core;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using log4net.Appender;
using log4netContrib.Appender;

namespace log4netContrib.Tests
{
    [TestFixture]
    public class when_indefinite_appender_proxy_is_created
    {
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void given_appender_to_proxy_does_not_inherit_from_appender_skeleton_should_throw()
        {
            var sut = new IndefiniteAppenderProxy(
                        MockRepository.GenerateStub<IAppender>());
        }

        [Test]
        public void should_set_recording_error_handler_on_appender()
        {
            var appender = new StubbingAppender();
            var sut = new IndefiniteAppenderProxy(appender);
            Assert.That(appender.ErrorHandler, Iz.InstanceOfType(typeof(RecordingErrorHandler)));
        }
    }

    [TestFixture]
    public class when_safe_appender_proxy_asked_to_try_and_append_single_event
    {
        [Test]
        public void given_it_is_the_first_time_through_should_call_append_on_proxied_appender()
        {
            var appender = new StubbingAppender();
            var sut = new IndefiniteAppenderProxy(appender);
            sut.TryAppend(new LoggingEvent(
                            new LoggingEventData()));
            Assert.That(appender.AppendCalledCounter, Iz.EqualTo(1));
        }

        [Test]
        public void given_it_is_first_time_through_and_error_raised_should_return_false()
        {
            var appender = new StubbingAppender();
            var sut = new IndefiniteAppenderProxy(appender);
            appender.SetError("foo");
            var result = sut.TryAppend(new LoggingEvent(
                                        new LoggingEventData()));
            Assert.That(result, Iz.False);
        }

        [Test]
        public void given_second_time_through_and_appender_has_no_error_should_append()
        {
            var appender = new StubbingAppender();
            var sut = new IndefiniteAppenderProxy(appender);
            sut.TryAppend(new LoggingEvent(
                            new LoggingEventData()));
            sut.TryAppend(new LoggingEvent(
                            new LoggingEventData()));
            Assert.That(appender.AppendCalledCounter, Iz.EqualTo(2));
        }

        [Test]
        public void given_second_time_through_and_appender_has_error_should_not_append()
        {
            var appender = new StubbingAppender();
            var sut = new IndefiniteAppenderProxy(appender);
            sut.TryAppend(new LoggingEvent(
                            new LoggingEventData()));
            appender.SetError("foo");
            sut.TryAppend(new LoggingEvent(
                            new LoggingEventData()));
            Assert.That(appender.AppendCalledCounter, Iz.EqualTo(1));
        }

        [Test]
        public void given_second_time_through_and_appender_has_error_should_return_not_succeeded()
        {
            var appender = new StubbingAppender();
            var sut = new IndefiniteAppenderProxy(appender);
            sut.TryAppend(new LoggingEvent(
                            new LoggingEventData()));
            appender.SetError("foo");
            var result = sut.TryAppend(new LoggingEvent(
                            new LoggingEventData()));
            Assert.That(result, Iz.False);
        }
    }

    [TestFixture]
    public class when_indefinite_appender_proxy_asked_to_try_and_append_multiple_events
    {
        LoggingEvent[] loggingEvents = new LoggingEvent[] { 
            new LoggingEvent(new LoggingEventData()
            {
                Level = Level.Error
            }),
            new LoggingEvent(new LoggingEventData()
            {
                Level = Level.Error
            })};

        [Test]
        public void given_it_is_the_first_time_through_should_call_append_on_proxied_appender()
        {
            var appender = new StubbingAppender();
            var sut = new IndefiniteAppenderProxy(appender);
            sut.TryAppend(loggingEvents);
            Assert.That(appender.AppendCalledCounter, Iz.EqualTo(1));
        }

        [Test]
        public void given_it_is_first_time_through_and_error_raised_should_return_if_succeeded()
        {
            var appender = new StubbingAppender();
            var sut = new IndefiniteAppenderProxy(appender);
            appender.SetError("foo");
            var result = sut.TryAppend(loggingEvents);
            Assert.That(result, Iz.False);
        }

        [Test]
        public void given_second_time_through_and_appender_has_no_error_should_append()
        {
            var appender = new StubbingAppender();
            var sut = new IndefiniteAppenderProxy(appender);
            sut.TryAppend(loggingEvents);
            sut.TryAppend(loggingEvents);
            Assert.That(appender.AppendCalledCounter, Iz.EqualTo(2));
        }

        [Test]
        public void given_second_time_through_and_appender_has_error_should_not_append()
        {
            var appender = new StubbingAppender();
            var sut = new IndefiniteAppenderProxy(appender);
            sut.TryAppend(loggingEvents);
            appender.SetError("foo");
            sut.TryAppend(loggingEvents);
            Assert.That(appender.AppendCalledCounter, Iz.EqualTo(1));
        }

        [Test]
        public void given_second_time_through_and_appender_has_error_should_return_not_succeeded()
        {
            var appender = new StubbingAppender();
            var sut = new IndefiniteAppenderProxy(appender);
            sut.TryAppend(loggingEvents);
            appender.SetError("foo");
            var result = sut.TryAppend(loggingEvents);
            Assert.That(result, Iz.False);
        }
    }
}
