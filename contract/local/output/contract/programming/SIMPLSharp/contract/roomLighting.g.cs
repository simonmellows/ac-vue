using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace contract
{
    public interface IroomLighting
    {
        object UserObject { get; set; }

        event EventHandler<UIEventArgs> toggle;

        void visibleFb(roomLightingBoolInputSigDelegate callback);
        void toggleFb(roomLightingBoolInputSigDelegate callback);

        contract.IitemButton[] scenes { get; }
        contract.IitemLightingCircuit[] circuits { get; }
    }

    public delegate void roomLightingBoolInputSigDelegate(BoolInputSig boolInputSig, IroomLighting roomLighting);

    internal class roomLighting : IroomLighting, IDisposable
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

        internal roomLighting(ComponentMediator componentMediator, uint controlJoinId)
        {
            ComponentMediator = componentMediator;
            Initialize(controlJoinId);
        }

        private static readonly IDictionary<uint, List<uint>> ScenesSmartObjectIdMappings = new Dictionary<uint, List<uint>> {
            { 13, new List<uint> { 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37 } }};
        private static readonly IDictionary<uint, List<uint>> CircuitsSmartObjectIdMappings = new Dictionary<uint, List<uint>> {

            { 13, new List<uint> { 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67 } }};

        internal static void ClearDictionaries()
        {
            ScenesSmartObjectIdMappings.Clear();
            CircuitsSmartObjectIdMappings.Clear();
        }

        private void Initialize(uint controlJoinId)
        {
            ControlJoinId = controlJoinId; 
 
            _devices = new List<BasicTriListWithSmartObject>(); 
 
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.toggle, ontoggle);

            List<uint> scenesList = ScenesSmartObjectIdMappings[controlJoinId];
            scenes = new contract.IitemButton[scenesList.Count];
            for (int index = 0; index < scenesList.Count; index++)
            {
                scenes[index] = new contract.itemButton(ComponentMediator, scenesList[index]); 
            }

            List<uint> circuitsList = CircuitsSmartObjectIdMappings[controlJoinId];
            circuits = new contract.IitemLightingCircuit[circuitsList.Count];
            for (int index = 0; index < circuitsList.Count; index++)
            {
                circuits[index] = new contract.itemLightingCircuit(ComponentMediator, circuitsList[index]); 
            }

        }

        public void AddDevice(BasicTriListWithSmartObject device)
        {
            Devices.Add(device);
            ComponentMediator.HookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
            for (int index = 0; index < scenes.Length; index++)
            {
                ((contract.itemButton)scenes[index]).AddDevice(device);
            }
            for (int index = 0; index < circuits.Length; index++)
            {
                ((contract.itemLightingCircuit)circuits[index]).AddDevice(device);
            }
        }

        public void RemoveDevice(BasicTriListWithSmartObject device)
        {
            Devices.Remove(device);
            ComponentMediator.UnHookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
            for (int index = 0; index < scenes.Length; index++)
            {
                ((contract.itemButton)scenes[index]).RemoveDevice(device);
            }
            for (int index = 0; index < circuits.Length; index++)
            {
                ((contract.itemLightingCircuit)circuits[index]).RemoveDevice(device);
            }
        }

        #endregion

        #region CH5 Contract

        public contract.IitemButton[] scenes { get; private set; }

        public contract.IitemLightingCircuit[] circuits { get; private set; }

        public event EventHandler<UIEventArgs> toggle;
        private void ontoggle(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = toggle;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void visibleFb(roomLightingBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.visibleFb], this);
            }
        }

        public void toggleFb(roomLightingBoolInputSigDelegate callback)
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
            return string.Format("Contract: {0} Component: {1} HashCode: {2} {3}", "roomLighting", GetType().Name, GetHashCode(), UserObject != null ? "UserObject: " + UserObject : null);
        }

        #endregion

        #region IDisposable

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            for (int index = 0; index < scenes.Length; index++)
            {
                ((contract.itemButton)scenes[index]).Dispose();
            }
            for (int index = 0; index < circuits.Length; index++)
            {
                ((contract.itemLightingCircuit)circuits[index]).Dispose();
            }

            toggle = null;
        }

        #endregion

    }
}
