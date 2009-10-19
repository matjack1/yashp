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
        private ArrayList _argumentsInfos;
        private int _functionID;

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
        public ArrayList ArgumentsInfos
        {
            get { return _argumentsInfos; }
            set { _argumentsInfos = value; }
        }
        public int FunctionID
        {
            get { return _functionID; }
            set { _functionID = value; }
        }
    }

    class LogEvent
    {
        private DateTime _timestamp;

        public DateTime timestamp
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
        private bool _isStatic;
        private ArrayList _argumentsObjectID;
        private int _instanceObjectID;

        public bool IsStatic
        {
            get { return _instanceObjectID == 0; }
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
        public int InstanceObjectID
        {
            get { return _instanceObjectID; }
            set { _instanceObjectID = value; }
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
