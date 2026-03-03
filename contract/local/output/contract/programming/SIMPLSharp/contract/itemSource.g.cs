using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace contract
{
    public interface IitemSource
    {
        object UserObject { get; set; }

        event EventHandler<UIEventArgs> press;

        void visibleFb(itemSourceBoolInputSigDelegate callback);
        void enableFb(itemSourceBoolInputSigDelegate callback);
        void pressFb(itemSourceBoolInputSigDelegate callback);
        void inUseFb(itemSourceBoolInputSigDelegate callback);
        void labelFb(itemSourceStringInputSigDelegate callback);
        void iconFb(itemSourceStringInputSigDelegate callback);

    }

    public delegate void itemSourceBoolInputSigDelegate(BoolInputSig boolInputSig, IitemSource itemSource);
    public delegate void itemSourceStringInputSigDelegate(StringInputSig stringInputSig, IitemSource itemSource);

    internal class itemSource : IitemSource, IDisposable
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
                public const uint press = 3;

                public const uint visibleFb = 1;
                public const uint enableFb = 2;
                public const uint pressFb = 3;
                public const uint inUseFb = 4;
            }
            internal static class Strings
            {

                public const uint labelFb = 1;
                public const uint iconFb = 2;
            }
        }

        #endregion

        #region Construction and Initialization

        internal itemSource(ComponentMediator componentMediator, uint controlJoinId)
        {
            ComponentMediator = componentMediator;
            Initialize(controlJoinId);
        }

        private void Initialize(uint controlJoinId)
        {
            ControlJoinId = controlJoinId; 
 
            _devices = new List<BasicTriListWithSmartObject>(); 
 
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.press, onpress);

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

        public event EventHandler<UIEventArgs> press;
        private void onpress(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = press;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void visibleFb(itemSourceBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.visibleFb], this);
            }
        }

        public void enableFb(itemSourceBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.enableFb], this);
            }
        }

        public void pressFb(itemSourceBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.pressFb], this);
            }
        }

        public void inUseFb(itemSourceBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.inUseFb], this);
            }
        }


        public void labelFb(itemSourceStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.labelFb], this);
            }
        }

        public void iconFb(itemSourceStringInputSigDelegate callback)
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
            return string.Format("Contract: {0} Component: {1} HashCode: {2} {3}", "itemSource", GetType().Name, GetHashCode(), UserObject != null ? "UserObject: " + UserObject : null);
        }

        #endregion

        #region IDisposable

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            press = null;
        }

        #endregion

    }
}
