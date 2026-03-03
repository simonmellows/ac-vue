using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace contract
{
    public interface IroomClimate
    {
        object UserObject { get; set; }

        event EventHandler<UIEventArgs> toggle;

        void visibleFb(roomClimateBoolInputSigDelegate callback);
        void toggleFb(roomClimateBoolInputSigDelegate callback);

        contract.IitemClimateZone[] zone { get; }
    }

    public delegate void roomClimateBoolInputSigDelegate(BoolInputSig boolInputSig, IroomClimate roomClimate);

    internal class roomClimate : IroomClimate, IDisposable
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
                public const uint toggle = 2;

                public const uint visibleFb = 1;
                public const uint toggleFb = 2;
            }
        }

        #endregion

        #region Construction and Initialization

        internal roomClimate(ComponentMediator componentMediator, uint controlJoinId)
        {
            ComponentMediator = componentMediator;
            Initialize(controlJoinId);
        }

        private static readonly IDictionary<uint, List<uint>> ZoneSmartObjectIdMappings = new Dictionary<uint, List<uint>> {
            { 5, new List<uint> { 78, 79, 80, 81, 82 } }, { 191, new List<uint> { 264, 265, 266, 267, 268 } }, 
            { 377, new List<uint> { 450, 451, 452, 453, 454 } }, { 563, new List<uint> { 636, 637, 638, 639, 640 } }, 
            { 749, new List<uint> { 822, 823, 824, 825, 826 } }};

        internal static void ClearDictionaries()
        {
            ZoneSmartObjectIdMappings.Clear();

            contract.itemClimateZone.ClearDictionaries();
        }

        private void Initialize(uint controlJoinId)
        {
            ControlJoinId = controlJoinId; 
 
            _devices = new List<BasicTriListWithSmartObject>(); 
 
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.toggle, ontoggle);

            List<uint> zoneList = ZoneSmartObjectIdMappings[controlJoinId];
            zone = new contract.IitemClimateZone[zoneList.Count];
            for (int index = 0; index < zoneList.Count; index++)
            {
                zone[index] = new contract.itemClimateZone(ComponentMediator, zoneList[index]); 
            }

        }

        public void AddDevice(BasicTriListWithSmartObject device)
        {
            Devices.Add(device);
            ComponentMediator.HookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
            for (int index = 0; index < zone.Length; index++)
            {
                ((contract.itemClimateZone)zone[index]).AddDevice(device);
            }
        }

        public void RemoveDevice(BasicTriListWithSmartObject device)
        {
            Devices.Remove(device);
            ComponentMediator.UnHookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
            for (int index = 0; index < zone.Length; index++)
            {
                ((contract.itemClimateZone)zone[index]).RemoveDevice(device);
            }
        }

        #endregion

        #region CH5 Contract

        public contract.IitemClimateZone[] zone { get; private set; }

        public event EventHandler<UIEventArgs> toggle;
        private void ontoggle(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = toggle;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void visibleFb(roomClimateBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.visibleFb], this);
            }
        }

        public void toggleFb(roomClimateBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.toggleFb], this);
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
            return string.Format("Contract: {0} Component: {1} HashCode: {2} {3}", "roomClimate", GetType().Name, GetHashCode(), UserObject != null ? "UserObject: " + UserObject : null);
        }

        #endregion

        #region IDisposable

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            for (int index = 0; index < zone.Length; index++)
            {
                ((contract.itemClimateZone)zone[index]).Dispose();
            }

            toggle = null;
        }

        #endregion

    }
}
