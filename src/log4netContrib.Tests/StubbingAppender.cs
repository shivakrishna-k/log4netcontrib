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
using log4net.Appender;
using log4net.Core;

namespace log4netContrib.Tests
{
    public class StubbingAppender : AppenderSkeleton
    {
        protected int appendCalledCounter = 0;

        public Action<LoggingEvent> SingleEventAppendAction = (x) => { };
        public Action<LoggingEvent[]> MultipleEventAppendAction = (x) => { };

        protected override void Append(LoggingEvent loggingEvent)
        {
            appendCalledCounter++;
            SingleEventAppendAction(loggingEvent);
        }

        protected override void Append(log4net.Core.LoggingEvent[] loggingEvents)
        {
            appendCalledCounter++;
            MultipleEventAppendAction(loggingEvents);
        }

        public void SetError(string message)
        {
            ErrorHandler.Error(message);
        }

        public int AppendCalledCounter
        {
            get { return appendCalledCounter; }
        }
    }
}
