using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace contract
{
    /// <summary>
    /// Name of the house
    /// </summary>
    public interface Iproperty
    {
        object UserObject { get; set; }

        void labelFb(propertyStringInputSigDelegate callback);

        contract.IpropertyClimate climate { get; }
        contract.IpropertySecurity security { get; }
    }

    public delegate void propertyStringInputSigDelegate(StringInputSig stringInputSig, Iproperty property);

    internal class property : Iproperty, IDisposable
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
            internal static class Strings
            {
                public const uint labelFb = 1;
            }
        }

        #endregion

        #region Construction and Initialization

        internal property(ComponentMediator componentMediator, uint controlJoinId)
        {
            ComponentMediator = componentMediator;
            Initialize(controlJoinId);
        }

        internal static void ClearDictionaries()
        {
            contract.propertySecurity.ClearDictionaries();
        }

        private void Initialize(uint controlJoinId)
        {
            ControlJoinId = controlJoinId; 
 
            _devices = new List<BasicTriListWithSmartObject>(); 
 
            climate = new contract.propertyClimate(ComponentMediator, 201);

            security = new contract.propertySecurity(ComponentMediator, 202);

        }

        public void AddDevice(BasicTriListWithSmartObject device)
        {
            Devices.Add(device);
            ComponentMediator.HookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
            ((contract.propertyClimate)climate).AddDevice(device);
            ((contract.propertySecurity)security).AddDevice(device);
        }

        public void RemoveDevice(BasicTriListWithSmartObject device)
        {
            Devices.Remove(device);
            ComponentMediator.UnHookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
            ((contract.propertyClimate)climate).RemoveDevice(device);
            ((contract.propertySecurity)security).RemoveDevice(device);
        }

        #endregion

        #region CH5 Contract

        public contract.IpropertyClimate climate { get; private set; }

        public contract.IpropertySecurity security { get; private set; }

        public void labelFb(propertyStringInputSigDelegate callback)
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
            return string.Format("Contract: {0} Component: {1} HashCode: {2} {3}", "property", GetType().Name, GetHashCode(), UserObject != null ? "UserObject: " + UserObject : null);
        }

        #endregion

        #region IDisposable

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            ((contract.propertyClimate)climate).Dispose();
            ((contract.propertySecurity)security).Dispose();
        }

        #endregion

    }
}
