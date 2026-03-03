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
        event EventHandler<UIEventArgs> power;
        event EventHandler<UIEventArgs> volumeRaise;
        event EventHandler<UIEventArgs> volumeLower;
        event EventHandler<UIEventArgs> volumeMute;
        event EventHandler<UIEventArgs> volume;

        void visibleFb(itemRoomBoolInputSigDelegate callback);
        void favouriteFb(itemRoomBoolInputSigDelegate callback);
        void powerOnFb(itemRoomBoolInputSigDelegate callback);
        void volumeMuteFb(itemRoomBoolInputSigDelegate callback);
        void roomGroupFb(itemRoomUShortInputSigDelegate callback);
        void volumeFb(itemRoomUShortInputSigDelegate callback);
        void volumeTypeFb(itemRoomUShortInputSigDelegate callback);
        void labelFb(itemRoomStringInputSigDelegate callback);

        contract.IroomLighting lighting { get; }
        contract.IroomShading shading { get; }
        contract.IroomClimate climate { get; }
        contract.IroomWatch watch { get; }
        contract.IroomListen listen { get; }
    }

    public delegate void itemRoomBoolInputSigDelegate(BoolInputSig boolInputSig, IitemRoom itemRoom);
    public delegate void itemRoomUShortInputSigDelegate(UShortInputSig uShortInputSig, IitemRoom itemRoom);
    public delegate void itemRoomStringInputSigDelegate(StringInputSig stringInputSig, IitemRoom itemRoom);

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
                public const uint power = 3;
                public const uint volumeRaise = 4;
                public const uint volumeLower = 5;
                public const uint volumeMute = 6;

                public const uint visibleFb = 1;
                public const uint favouriteFb = 2;
                public const uint powerOnFb = 3;
                public const uint volumeMuteFb = 6;
            }
            internal static class Numerics
            {
                public const uint volume = 2;

                public const uint roomGroupFb = 1;
                public const uint volumeFb = 2;
                public const uint volumeTypeFb = 3;
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

        private static readonly IDictionary<uint, List<uint>> LightingSmartObjectIdMappings = new Dictionary<uint, List<uint>> {
            { 2, new List<uint> { 3 } }, { 188, new List<uint> { 189 } }, { 374, new List<uint> { 375 } }, { 560, new List<uint> { 561 } }, 
            { 746, new List<uint> { 747 } }};
        private static readonly IDictionary<uint, List<uint>> ShadingSmartObjectIdMappings = new Dictionary<uint, List<uint>> {
            { 2, new List<uint> { 4 } }, { 188, new List<uint> { 190 } }, { 374, new List<uint> { 376 } }, { 560, new List<uint> { 562 } }, 
            { 746, new List<uint> { 748 } }};
        private static readonly IDictionary<uint, List<uint>> ClimateSmartObjectIdMappings = new Dictionary<uint, List<uint>> {
            { 2, new List<uint> { 5 } }, { 188, new List<uint> { 191 } }, { 374, new List<uint> { 377 } }, { 560, new List<uint> { 563 } }, 
            { 746, new List<uint> { 749 } }};
        private static readonly IDictionary<uint, List<uint>> WatchSmartObjectIdMappings = new Dictionary<uint, List<uint>> {
            { 2, new List<uint> { 6 } }, { 188, new List<uint> { 192 } }, { 374, new List<uint> { 378 } }, { 560, new List<uint> { 564 } }, 
            { 746, new List<uint> { 750 } }};
        private static readonly IDictionary<uint, List<uint>> ListenSmartObjectIdMappings = new Dictionary<uint, List<uint>> {
            { 2, new List<uint> { 7 } }, { 188, new List<uint> { 193 } }, { 374, new List<uint> { 379 } }, { 560, new List<uint> { 565 } }, 
            { 746, new List<uint> { 751 } }};

        internal static void ClearDictionaries()
        {
            LightingSmartObjectIdMappings.Clear();
            ShadingSmartObjectIdMappings.Clear();
            ClimateSmartObjectIdMappings.Clear();
            WatchSmartObjectIdMappings.Clear();
            ListenSmartObjectIdMappings.Clear();

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
 
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.favourite, onfavourite);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.power, onpower);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.volumeRaise, onvolumeRaise);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.volumeLower, onvolumeLower);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.volumeMute, onvolumeMute);
            ComponentMediator.ConfigureNumericEvent(controlJoinId, Joins.Numerics.volume, onvolume);

            lighting = new contract.roomLighting(ComponentMediator, LightingSmartObjectIdMappings[controlJoinId][0]);

            shading = new contract.roomShading(ComponentMediator, ShadingSmartObjectIdMappings[controlJoinId][0]);

            climate = new contract.roomClimate(ComponentMediator, ClimateSmartObjectIdMappings[controlJoinId][0]);

            watch = new contract.roomWatch(ComponentMediator, WatchSmartObjectIdMappings[controlJoinId][0]);

            listen = new contract.roomListen(ComponentMediator, ListenSmartObjectIdMappings[controlJoinId][0]);

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

        public event EventHandler<UIEventArgs> favourite;
        private void onfavourite(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = favourite;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> power;
        private void onpower(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = power;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

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

        public void powerOnFb(itemRoomBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.powerOnFb], this);
            }
        }

        public void volumeMuteFb(itemRoomBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.volumeMuteFb], this);
            }
        }

        public event EventHandler<UIEventArgs> volume;
        private void onvolume(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = volume;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void roomGroupFb(itemRoomUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.roomGroupFb], this);
            }
        }

        public void volumeFb(itemRoomUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.volumeFb], this);
            }
        }

        public void volumeTypeFb(itemRoomUShortInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].UShortInput[Joins.Numerics.volumeTypeFb], this);
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

            ((contract.roomLighting)lighting).Dispose();
            ((contract.roomShading)shading).Dispose();
            ((contract.roomClimate)climate).Dispose();
            ((contract.roomWatch)watch).Dispose();
            ((contract.roomListen)listen).Dispose();

            favourite = null;
            power = null;
            volumeRaise = null;
            volumeLower = null;
            volumeMute = null;
            volume = null;
        }

        #endregion

    }
}
