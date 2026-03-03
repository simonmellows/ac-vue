using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace contract
{
    public interface IpropertySecurity
    {
        object UserObject { get; set; }

        void visibleFb(propertySecurityBoolInputSigDelegate callback);
        void showCameras(propertySecurityBoolInputSigDelegate callback);
        void showTexecom(propertySecurityBoolInputSigDelegate callback);

        contract.IitemCamera[] cameras { get; }
    }

    public delegate void propertySecurityBoolInputSigDelegate(BoolInputSig boolInputSig, IpropertySecurity propertySecurity);

    internal class propertySecurity : IpropertySecurity, IDisposable
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

                public const uint visibleFb = 1;
                public const uint showCameras = 2;
                public const uint showTexecom = 3;
            }
        }

        #endregion

        #region Construction and Initialization

        internal propertySecurity(ComponentMediator componentMediator, uint controlJoinId)
        {
            ComponentMediator = componentMediator;
            Initialize(controlJoinId);
        }

        private static readonly IDictionary<uint, List<uint>> CamerasSmartObjectIdMappings = new Dictionary<uint, List<uint>> {

            { 202, new List<uint> { 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232 } }};

        internal static void ClearDictionaries()
        {
            CamerasSmartObjectIdMappings.Clear();
        }

        private void Initialize(uint controlJoinId)
        {
            ControlJoinId = controlJoinId; 
 
            _devices = new List<BasicTriListWithSmartObject>(); 
 
            List<uint> camerasList = CamerasSmartObjectIdMappings[controlJoinId];
            cameras = new contract.IitemCamera[camerasList.Count];
            for (int index = 0; index < camerasList.Count; index++)
            {
                cameras[index] = new contract.itemCamera(ComponentMediator, camerasList[index]); 
            }

        }

        public void AddDevice(BasicTriListWithSmartObject device)
        {
            Devices.Add(device);
            ComponentMediator.HookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
            for (int index = 0; index < cameras.Length; index++)
            {
                ((contract.itemCamera)cameras[index]).AddDevice(device);
            }
        }

        public void RemoveDevice(BasicTriListWithSmartObject device)
        {
            Devices.Remove(device);
            ComponentMediator.UnHookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
            for (int index = 0; index < cameras.Length; index++)
            {
                ((contract.itemCamera)cameras[index]).RemoveDevice(device);
            }
        }

        #endregion

        #region CH5 Contract

        public contract.IitemCamera[] cameras { get; private set; }


        public void visibleFb(propertySecurityBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.visibleFb], this);
            }
        }

        public void showCameras(propertySecurityBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.showCameras], this);
            }
        }

        public void showTexecom(propertySecurityBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.showTexecom], this);
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
            return string.Format("Contract: {0} Component: {1} HashCode: {2} {3}", "propertySecurity", GetType().Name, GetHashCode(), UserObject != null ? "UserObject: " + UserObject : null);
        }

        #endregion

        #region IDisposable

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            for (int index = 0; index < cameras.Length; index++)
            {
                ((contract.itemCamera)cameras[index]).Dispose();
            }

        }

        #endregion

    }
}
