using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace contract
{
    public interface IitemLightingCircuit
    {
        object UserObject { get; set; }

        event EventHandler<UIEventArgs> press;
        event EventHandler<UIEventArgs> switchOn;
        event EventHandler<UIEventArgs> switchOff;
        event EventHandler<UIEventArgs> toggle;
        event EventHandler<UIEventArgs> level;
        event EventHandler<UIEventArgs> red;
        event EventHandler<UIEventArgs> green;
        event EventHandler<UIEventArgs> blue;
        event EventHandler<UIEventArgs> white;
        event EventHandler<UIEventArgs> warmWhite;

        void visibleFb(itemLightingCircuitBoolInputSigDelegate callback);
        void pressFb(itemLightingCircuitBoolInputSigDelegate callback);
        void switchOnFb(itemLightingCircuitBoolInputSigDelegate callback);
        void switchOffFb(itemLightingCircuitBoolInputSigDelegate callback);
        void typeFb(itemLightingCircuitUShortInputSigDelegate callback);
        void levelFb(itemLightingCircuitUShortInputSigDelegate callback);
        void redFb(itemLightingCircuitUShortInputSigDelegate callback);
        void greenFb(itemLightingCircuitUShortInputSigDelegate callback);
        void blueFb(itemLightingCircuitUShortInputSigDelegate callback);
        void whiteFb(itemLightingCircuitUShortInputSigDelegate callback);
        void warmWhiteFb(itemLightingCircuitUShortInputSigDelegate callback);
        void labelFb(itemLightingCircuitStringInputSigDelegate callback);

    }

    public delegate void itemLightingCircuitBoolInputSigDelegate(BoolInputSig boolInputSig, IitemLightingCircuit itemLightingCircuit);
    public delegate void itemLightingCircuitUShortInputSigDelegate(UShortInputSig uShortInputSig, IitemLightingCircuit itemLightingCircuit);
    public delegate void itemLightingCircuitStringInputSigDelegate(StringInputSig stringInputSig, IitemLightingCircuit itemLightingCircuit);

    internal class itemLightingCircuit : IitemLightingCircuit, IDisposable
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
                public const uint press = 1;
                public const uint switchOn = 2;
                public const uint switchOff = 3;
                public const uint toggle = 4;

                public const uint visibleFb = 1;
                public const uint pressFb = 2;
                public const uint switchOnFb = 3;
                public const uint switchOffFb = 4;
            }
            internal static class Numerics
            {
                public const uint level = 2;
                public const uint red = 3;
                public const uint green = 4;
                public const uint blue = 5;
                public const uint white = 6;
                public const uint warmWhite = 7;

                public const uint typeFb = 1;
                public const uint levelFb = 2;
                public const uint redFb = 3;
                public const uint greenFb = 4;
                public const uint blueFb = 5;
                public const uint whiteFb = 6;
                public const uint warmWhiteFb = 7;
            }
            internal static class Strings
            {

                public const uint labelFb = 1;
            }
        }

        #endregion

        #region Construction and Initialization

        internal itemLightingCircuit(ComponentMediator componentMediator, uint controlJoinId)
        {
            ComponentMediator = componentMediator;
            Initialize(controlJoinId);
        }

        private void Initialize(uint controlJoinId)
        {
            ControlJoinId = controlJoinId; 
 
            _devices = new List<BasicTriListWithSmartObject>(); 
 
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.press, onpress);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.switchOn, onswitchOn);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.switchOff, onswitchOff);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.toggle, ontoggle);
            ComponentMediator.ConfigureNumericEvent(controlJoinId, Joins.Numerics.level, onlevel);
            ComponentMediator.ConfigureNumericEvent(controlJoinId, Joins.Numerics.red, onred);
            ComponentMediator.ConfigureNumericEvent(controlJoinId, Joins.Numerics.green, ongreen);
            ComponentMediator.ConfigureNumericEvent(controlJoinId, Joins.Numerics.blue, onblue);
            ComponentMediator.ConfigureNumericEvent(controlJoinId, Joins.Numerics.white, onwhite);
            ComponentMediator.ConfigureNumericEvent(controlJoinId, Joins.Numerics.warmWhite, onwarmWhite);

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

        public event EventHandler<UIEventArgs> switchOn;
        private void onswitchOn(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = switchOn;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> switchOff;
        private void onswitchOff(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = switchOff;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> toggle;
        private void ontoggle(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = toggle;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void visibleFb(itemLightingCircuitBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.visibleFb], this);
            }
        }

        public void pressFb(itemLightingCircuitBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.pressFb], this);
            }
        }

        public void switchOnFb(itemLightingCircuitBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.switchOnFb], this);
            }
        }

        public void switchOffFb(itemLightingCircuitBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.switchOffFb], this);
            }
        }

        public event EventHandler<UIEventArgs> level;
        private void onlevel(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = level;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> red;
        private void onred(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = red;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> green;
        private void ongreen(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = green;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> blue;
        private void onblue(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = blue;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> white;
        private void onwhite(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = white;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> warmWhite;
        private void onwarmWhite(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = warmWhite;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void typeFb(itemLightingCircuitUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.typeFb], this);
            }
        }

        public void levelFb(itemLightingCircuitUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.levelFb], this);
            }
        }

        public void redFb(itemLightingCircuitUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.redFb], this);
            }
        }

        public void greenFb(itemLightingCircuitUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.greenFb], this);
            }
        }

        public void blueFb(itemLightingCircuitUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.blueFb], this);
            }
        }

        public void whiteFb(itemLightingCircuitUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.whiteFb], this);
            }
        }

        public void warmWhiteFb(itemLightingCircuitUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.warmWhiteFb], this);
            }
        }


        public void labelFb(itemLightingCircuitStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.labelFb], this);
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
            return string.Format("Contract: {0} Component: {1} HashCode: {2} {3}", "itemLightingCircuit", GetType().Name, GetHashCode(), UserObject != null ? "UserObject: " + UserObject : null);
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
            switchOn = null;
            switchOff = null;
            toggle = null;
            level = null;
            red = null;
            green = null;
            blue = null;
            white = null;
            warmWhite = null;
        }

        #endregion

    }
}
