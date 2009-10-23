using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Demo
{
    class MethodInfo
    {
        private String _className;
        private String _methodName;
        private String _returnType;
        private String _functionID;
        private bool _isStatic;

        public bool IsStatic
        {
            get { return _isStatic; }
            set { _isStatic = value; }
        }
        public String ClassName
        {
            get { return _className; }
            set { _className = value; }
        }
        public String MethodName
        {
            get { return _methodName; }
            set { _methodName = value; }
        }
        public String ReturnType
        {
            get { return _returnType; }
            set { _returnType = value; }
        }
        public String FunctionID
        {
            get { return _functionID; }
            set { _functionID = value; }
        }
    }

    class LogEvent
    {
        private double _timestamp;

        public double timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }
    }

    class MethodEvent : LogEvent
    {
        public enum EventTypeEnum
        {
            EnterEvent = 0x01,
            LeaveEvent = 0x02,
            TailCallEvent = 0x04,
        }

        private EventTypeEnum _eventType;
        private MethodInfo _methodInfo;
        private String _instanceObjectID;
        private String _threadID;

        public String ThreadID
        {
            get { return _threadID; }
            set { _threadID = value; }
        }
        public EventTypeEnum EventType
        {
            get { return _eventType; }
            set { _eventType = value; }
        }
        public MethodInfo MethodInfo
        {
            get { return _methodInfo; }
            set { _methodInfo = value; }
        }
        public String InstanceObjectID
        {
            get { return _instanceObjectID; }
            set { _instanceObjectID = value; }
        }

        public String getHash() {
            String hash;

            if (!this.MethodInfo.IsStatic)
                hash = this.InstanceObjectID.ToString();
            else
                hash = this.MethodInfo.ClassName;

            return hash;
        }
    }

    class ThreadEvent : LogEvent
    {

    }

    class ExceptionEvent : LogEvent
    {

    }

    class GarbageCollectionEvent : LogEvent
    {

    }
}
