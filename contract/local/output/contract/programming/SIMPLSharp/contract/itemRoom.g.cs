using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace contract
{
    public interface IitemRoom
    {
        object UserObject { get; set; }

        event EventHandler<UIEventArgs> favourite;
        event EventHandler<UIEventArgs> press;
        event EventHandler<UIEventArgs> roomOff;
        event EventHandler<UIEventArgs> watchOff;
        event EventHandler<UIEventArgs> listenOff;

        void visibleFb(itemRoomBoolInputSigDelegate callback);
        void favouriteFb(itemRoomBoolInputSigDelegate callback);
        void pressFb(itemRoomBoolInputSigDelegate callback);
        void roomGroupFb(itemRoomUShortInputSigDelegate callback);
        void sourceWatchFb(itemRoomUShortInputSigDelegate callback);
        void sourceListenFb(itemRoomUShortInputSigDelegate callback);
        void labelFb(itemRoomStringInputSigDelegate callback);

    }

    public delegate void itemRoomBoolInputSigDelegate(BoolInputSig boolInputSig, IitemRoom itemRoom);
    public delegate void itemRoomUShortInputSigDelegate(UShortInputSig uShortInputSig, IitemRoom itemRoom);
    public delegate void itemRoomStringInputSigDelegate(StringInputSig stringInputSig, IitemRoom itemRoom);

    /// <summary>
    /// Component for room list items
    /// </summary>
    internal class itemRoom : IitemRoom, IDisposable
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
                public const uint favourite = 2;
                public const uint press = 3;
                public const uint roomOff = 4;
                public const uint watchOff = 5;
                public const uint listenOff = 6;

                public const uint visibleFb = 1;
                public const uint favouriteFb = 2;
                public const uint pressFb = 3;
            }
            internal static class Numerics
            {

                public const uint roomGroupFb = 1;
                public const uint sourceWatchFb = 2;
                public const uint sourceListenFb = 3;
            }
            internal static class Strings
            {

                public const uint labelFb = 1;
            }
        }

        #endregion

        #region Construction and Initialization

        internal itemRoom(ComponentMediator componentMediator, uint controlJoinId)
        {
            ComponentMediator = componentMediator;
            Initialize(controlJoinId);
        }

        private void Initialize(uint controlJoinId)
        {
            ControlJoinId = controlJoinId; 
 
            _devices = new List<BasicTriListWithSmartObject>(); 
 
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.favourite, onfavourite);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.press, onpress);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.roomOff, onroomOff);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.watchOff, onwatchOff);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.listenOff, onlistenOff);

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

        public event EventHandler<UIEventArgs> favourite;
        private void onfavourite(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = favourite;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> press;
        private void onpress(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = press;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> roomOff;
        private void onroomOff(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = roomOff;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> watchOff;
        private void onwatchOff(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = watchOff;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> listenOff;
        private void onlistenOff(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = listenOff;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void visibleFb(itemRoomBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.visibleFb], this);
            }
        }

        public void favouriteFb(itemRoomBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.favouriteFb], this);
            }
        }

        public void pressFb(itemRoomBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.pressFb], this);
            }
        }


        public void roomGroupFb(itemRoomUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.roomGroupFb], this);
            }
        }

        public void sourceWatchFb(itemRoomUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.sourceWatchFb], this);
            }
        }

        public void sourceListenFb(itemRoomUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.sourceListenFb], this);
            }
        }


        public void labelFb(itemRoomStringInputSigDelegate callback)
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
            return string.Format("Contract: {0} Component: {1} HashCode: {2} {3}", "itemRoom", GetType().Name, GetHashCode(), UserObject != null ? "UserObject: " + UserObject : null);
        }

        #endregion

        #region IDisposable

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            favourite = null;
            press = null;
            roomOff = null;
            watchOff = null;
            listenOff = null;
        }

        #endregion

    }
}
