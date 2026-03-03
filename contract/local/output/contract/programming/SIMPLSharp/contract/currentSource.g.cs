using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace contract
{
    public interface IcurrentSource
    {
        object UserObject { get; set; }

        event EventHandler<UIEventArgs> play;
        event EventHandler<UIEventArgs> stop;
        event EventHandler<UIEventArgs> pause;

        void playFb(currentSourceBoolInputSigDelegate callback);
        void stopFb(currentSourceBoolInputSigDelegate callback);
        void pauseFb(currentSourceBoolInputSigDelegate callback);
        void labelFb(currentSourceStringInputSigDelegate callback);
        void typeFb(currentSourceStringInputSigDelegate callback);
        void iconFb(currentSourceStringInputSigDelegate callback);

    }

    public delegate void currentSourceBoolInputSigDelegate(BoolInputSig boolInputSig, IcurrentSource currentSource);
    public delegate void currentSourceStringInputSigDelegate(StringInputSig stringInputSig, IcurrentSource currentSource);

    internal class currentSource : IcurrentSource, IDisposable
    {
        #region Standard CH5 Component members

        private ComponentMediator ComponentMediator { get; set; }

        public object UserObject { get; set; }

        public uint ControlJoinId { get; private set; }

        private IList<BasicTriListWithSmartObject> _devices;
        public IList<BasicTriListWithSmartObject> Devices { get { return _devices; } }

        #endregion

        #region Joins

        private static class Joins
        {
            internal static class Booleans
            {
                public const uint play = 1;
                public const uint stop = 2;
                public const uint pause = 3;

                public const uint playFb = 1;
                public const uint stopFb = 2;
                public const uint pauseFb = 3;
            }
            internal static class Strings
            {

                public const uint labelFb = 1;
                public const uint typeFb = 2;
                public const uint iconFb = 3;
            }
        }

        #endregion

        #region Construction and Initialization

        internal currentSource(ComponentMediator componentMediator, uint controlJoinId)
        {
            ComponentMediator = componentMediator;
            Initialize(controlJoinId);
        }

        private void Initialize(uint controlJoinId)
        {
            ControlJoinId = controlJoinId; 
 
            _devices = new List<BasicTriListWithSmartObject>(); 
 
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.play, onplay);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.stop, onstop);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.pause, onpause);

        }

        public void AddDevice(BasicTriListWithSmartObject device)
        {
            Devices.Add(device);
            ComponentMediator.HookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
        }

        public void RemoveDevice(BasicTriListWithSmartObject device)
        {
            Devices.Remove(device);
            ComponentMediator.UnHookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
        }

        #endregion

        #region CH5 Contract

        public event EventHandler<UIEventArgs> play;
        private void onplay(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = play;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> stop;
        private void onstop(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = stop;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> pause;
        private void onpause(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = pause;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void playFb(currentSourceBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.playFb], this);
            }
        }

        public void stopFb(currentSourceBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.stopFb], this);
            }
        }

        public void pauseFb(currentSourceBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.pauseFb], this);
            }
        }


        public void labelFb(currentSourceStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.labelFb], this);
            }
        }

        public void typeFb(currentSourceStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.typeFb], this);
            }
        }

        public void iconFb(currentSourceStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.iconFb], this);
            }
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return (int)ControlJoinId;
        }

        public override string ToString()
        {
            return string.Format("Contract: {0} Component: {1} HashCode: {2} {3}", "currentSource", GetType().Name, GetHashCode(), UserObject != null ? "UserObject: " + UserObject : null);
        }

        #endregion

        #region IDisposable

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            play = null;
            stop = null;
            pause = null;
        }

        #endregion

    }
}
