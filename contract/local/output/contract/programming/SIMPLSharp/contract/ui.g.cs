using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace contract
{
    public interface Iui
    {
        object UserObject { get; set; }

        void hideFloorplanView(uiBoolInputSigDelegate callback);
        void hideRoomGroupAll(uiBoolInputSigDelegate callback);
        void hideRoomGroupFavourites(uiBoolInputSigDelegate callback);
        void readyFb(uiBoolInputSigDelegate callback);
        void defaultRoomFb(uiUShortInputSigDelegate callback);
        void defaultRoomGroupFb(uiUShortInputSigDelegate callback);
        void labelFb(uiStringInputSigDelegate callback);

    }

    public delegate void uiBoolInputSigDelegate(BoolInputSig boolInputSig, Iui ui);
    public delegate void uiUShortInputSigDelegate(UShortInputSig uShortInputSig, Iui ui);
    public delegate void uiStringInputSigDelegate(StringInputSig stringInputSig, Iui ui);

    internal class ui : Iui, IDisposable
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

                public const uint hideFloorplanView = 1;
                public const uint hideRoomGroupAll = 2;
                public const uint hideRoomGroupFavourites = 3;
                public const uint readyFb = 4;
            }
            internal static class Numerics
            {

                public const uint defaultRoomFb = 1;
                public const uint defaultRoomGroupFb = 2;
            }
            internal static class Strings
            {

                public const uint labelFb = 1;
            }
        }

        #endregion

        #region Construction and Initialization

        internal ui(ComponentMediator componentMediator, uint controlJoinId)
        {
            ComponentMediator = componentMediator;
            Initialize(controlJoinId);
        }

        private void Initialize(uint controlJoinId)
        {
            ControlJoinId = controlJoinId; 
 
            _devices = new List<BasicTriListWithSmartObject>(); 
 
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


        public void hideFloorplanView(uiBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.hideFloorplanView], this);
            }
        }

        public void hideRoomGroupAll(uiBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.hideRoomGroupAll], this);
            }
        }

        public void hideRoomGroupFavourites(uiBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.hideRoomGroupFavourites], this);
            }
        }

        public void readyFb(uiBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.readyFb], this);
            }
        }


        public void defaultRoomFb(uiUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.defaultRoomFb], this);
            }
        }

        public void defaultRoomGroupFb(uiUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.defaultRoomGroupFb], this);
            }
        }


        public void labelFb(uiStringInputSigDelegate callback)
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
            return string.Format("Contract: {0} Component: {1} HashCode: {2} {3}", "ui", GetType().Name, GetHashCode(), UserObject != null ? "UserObject: " + UserObject : null);
        }

        #endregion

        #region IDisposable

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

        }

        #endregion

    }
}
