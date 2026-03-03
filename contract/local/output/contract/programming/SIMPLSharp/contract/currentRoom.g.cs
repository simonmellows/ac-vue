using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace contract
{
    public interface IcurrentRoom
    {
        object UserObject { get; set; }

        event EventHandler<UIEventArgs> volumeRaise;
        event EventHandler<UIEventArgs> volumeLower;
        event EventHandler<UIEventArgs> volumeMute;
        event EventHandler<UIEventArgs> powerOff;
        event EventHandler<UIEventArgs> volumeLevel;

        void powerOnFb(currentRoomBoolInputSigDelegate callback);
        void volumeMuteFb(currentRoomBoolInputSigDelegate callback);
        void volumeLevelFb(currentRoomUShortInputSigDelegate callback);
        void volumeTypeFb(currentRoomUShortInputSigDelegate callback);
        void labelFb(currentRoomStringInputSigDelegate callback);

        contract.IroomLighting lighting { get; }
        contract.IroomShading shading { get; }
        contract.IroomClimate climate { get; }
        contract.IroomWatch watch { get; }
        contract.IroomListen listen { get; }
    }

    public delegate void currentRoomBoolInputSigDelegate(BoolInputSig boolInputSig, IcurrentRoom currentRoom);
    public delegate void currentRoomUShortInputSigDelegate(UShortInputSig uShortInputSig, IcurrentRoom currentRoom);
    public delegate void currentRoomStringInputSigDelegate(StringInputSig stringInputSig, IcurrentRoom currentRoom);

    internal class currentRoom : IcurrentRoom, IDisposable
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
                public const uint volumeRaise = 2;
                public const uint volumeLower = 3;
                public const uint volumeMute = 4;
                public const uint powerOff = 5;

                public const uint powerOnFb = 1;
                public const uint volumeMuteFb = 4;
            }
            internal static class Numerics
            {
                public const uint volumeLevel = 1;

                public const uint volumeLevelFb = 1;
                public const uint volumeTypeFb = 2;
            }
            internal static class Strings
            {

                public const uint labelFb = 1;
            }
        }

        #endregion

        #region Construction and Initialization

        internal currentRoom(ComponentMediator componentMediator, uint controlJoinId)
        {
            ComponentMediator = componentMediator;
            Initialize(controlJoinId);
        }

        internal static void ClearDictionaries()
        {
            contract.roomLighting.ClearDictionaries();
            contract.roomShading.ClearDictionaries();
            contract.roomClimate.ClearDictionaries();
            contract.roomWatch.ClearDictionaries();
            contract.roomListen.ClearDictionaries();
        }

        private void Initialize(uint controlJoinId)
        {
            ControlJoinId = controlJoinId; 
 
            _devices = new List<BasicTriListWithSmartObject>(); 
 
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.volumeRaise, onvolumeRaise);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.volumeLower, onvolumeLower);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.volumeMute, onvolumeMute);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.powerOff, onpowerOff);
            ComponentMediator.ConfigureNumericEvent(controlJoinId, Joins.Numerics.volumeLevel, onvolumeLevel);

            lighting = new contract.roomLighting(ComponentMediator, 13);

            shading = new contract.roomShading(ComponentMediator, 14);

            climate = new contract.roomClimate(ComponentMediator, 15);

            watch = new contract.roomWatch(ComponentMediator, 16);

            listen = new contract.roomListen(ComponentMediator, 17);

        }

        public void AddDevice(BasicTriListWithSmartObject device)
        {
            Devices.Add(device);
            ComponentMediator.HookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
            ((contract.roomLighting)lighting).AddDevice(device);
            ((contract.roomShading)shading).AddDevice(device);
            ((contract.roomClimate)climate).AddDevice(device);
            ((contract.roomWatch)watch).AddDevice(device);
            ((contract.roomListen)listen).AddDevice(device);
        }

        public void RemoveDevice(BasicTriListWithSmartObject device)
        {
            Devices.Remove(device);
            ComponentMediator.UnHookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
            ((contract.roomLighting)lighting).RemoveDevice(device);
            ((contract.roomShading)shading).RemoveDevice(device);
            ((contract.roomClimate)climate).RemoveDevice(device);
            ((contract.roomWatch)watch).RemoveDevice(device);
            ((contract.roomListen)listen).RemoveDevice(device);
        }

        #endregion

        #region CH5 Contract

        public contract.IroomLighting lighting { get; private set; }

        public contract.IroomShading shading { get; private set; }

        public contract.IroomClimate climate { get; private set; }

        public contract.IroomWatch watch { get; private set; }

        public contract.IroomListen listen { get; private set; }

        public event EventHandler<UIEventArgs> volumeRaise;
        private void onvolumeRaise(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = volumeRaise;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> volumeLower;
        private void onvolumeLower(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = volumeLower;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> volumeMute;
        private void onvolumeMute(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = volumeMute;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> powerOff;
        private void onpowerOff(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = powerOff;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void powerOnFb(currentRoomBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.powerOnFb], this);
            }
        }

        public void volumeMuteFb(currentRoomBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.volumeMuteFb], this);
            }
        }

        public event EventHandler<UIEventArgs> volumeLevel;
        private void onvolumeLevel(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = volumeLevel;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void volumeLevelFb(currentRoomUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.volumeLevelFb], this);
            }
        }

        public void volumeTypeFb(currentRoomUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.volumeTypeFb], this);
            }
        }


        public void labelFb(currentRoomStringInputSigDelegate callback)
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
            return string.Format("Contract: {0} Component: {1} HashCode: {2} {3}", "currentRoom", GetType().Name, GetHashCode(), UserObject != null ? "UserObject: " + UserObject : null);
        }

        #endregion

        #region IDisposable

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            ((contract.roomLighting)lighting).Dispose();
            ((contract.roomShading)shading).Dispose();
            ((contract.roomClimate)climate).Dispose();
            ((contract.roomWatch)watch).Dispose();
            ((contract.roomListen)listen).Dispose();

            volumeRaise = null;
            volumeLower = null;
            volumeMute = null;
            powerOff = null;
            volumeLevel = null;
        }

        #endregion

    }
}
