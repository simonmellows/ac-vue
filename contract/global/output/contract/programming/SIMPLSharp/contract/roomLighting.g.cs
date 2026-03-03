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

        contract.IitemButton[] scene { get; }
        contract.IitemLightingCircuit[] circuit { get; }
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

        private static readonly IDictionary<uint, List<uint>> SceneSmartObjectIdMappings = new Dictionary<uint, List<uint>> {
            { 3, new List<uint> { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27 } }, 
            { 189, new List<uint> { 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212, 213 } }, 
            { 375, new List<uint> { 380, 381, 382, 383, 384, 385, 386, 387, 388, 389, 390, 391, 392, 393, 394, 395, 396, 397, 398, 399 } }, 
            { 561, new List<uint> { 566, 567, 568, 569, 570, 571, 572, 573, 574, 575, 576, 577, 578, 579, 580, 581, 582, 583, 584, 585 } }, 
            { 747, new List<uint> { 752, 753, 754, 755, 756, 757, 758, 759, 760, 761, 762, 763, 764, 765, 766, 767, 768, 769, 770, 771 } }};
        private static readonly IDictionary<uint, List<uint>> CircuitSmartObjectIdMappings = new Dictionary<uint, List<uint>> {

            { 3, new List<uint> { 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57 } }, 
            { 189, new List<uint> { 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243 } }, 
            { 375, new List<uint> { 400, 401, 402, 403, 404, 405, 406, 407, 408, 409, 410, 411, 412, 413, 414, 415, 416, 417, 418, 419, 420, 421, 422, 423, 424, 425, 426, 427, 428, 429 } }, 
            { 561, new List<uint> { 586, 587, 588, 589, 590, 591, 592, 593, 594, 595, 596, 597, 598, 599, 600, 601, 602, 603, 604, 605, 606, 607, 608, 609, 610, 611, 612, 613, 614, 615 } }, 
            { 747, new List<uint> { 772, 773, 774, 775, 776, 777, 778, 779, 780, 781, 782, 783, 784, 785, 786, 787, 788, 789, 790, 791, 792, 793, 794, 795, 796, 797, 798, 799, 800, 801 } }};

        internal static void ClearDictionaries()
        {
            SceneSmartObjectIdMappings.Clear();
            CircuitSmartObjectIdMappings.Clear();
        }

        private void Initialize(uint controlJoinId)
        {
            ControlJoinId = controlJoinId; 
 
            _devices = new List<BasicTriListWithSmartObject>(); 
 
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.toggle, ontoggle);

            List<uint> sceneList = SceneSmartObjectIdMappings[controlJoinId];
            scene = new contract.IitemButton[sceneList.Count];
            for (int index = 0; index < sceneList.Count; index++)
            {
                scene[index] = new contract.itemButton(ComponentMediator, sceneList[index]); 
            }

            List<uint> circuitList = CircuitSmartObjectIdMappings[controlJoinId];
            circuit = new contract.IitemLightingCircuit[circuitList.Count];
            for (int index = 0; index < circuitList.Count; index++)
            {
                circuit[index] = new contract.itemLightingCircuit(ComponentMediator, circuitList[index]); 
            }

        }

        public void AddDevice(BasicTriListWithSmartObject device)
        {
            Devices.Add(device);
            ComponentMediator.HookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
            for (int index = 0; index < scene.Length; index++)
            {
                ((contract.itemButton)scene[index]).AddDevice(device);
            }
            for (int index = 0; index < circuit.Length; index++)
            {
                ((contract.itemLightingCircuit)circuit[index]).AddDevice(device);
            }
        }

        public void RemoveDevice(BasicTriListWithSmartObject device)
        {
            Devices.Remove(device);
            ComponentMediator.UnHookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
            for (int index = 0; index < scene.Length; index++)
            {
                ((contract.itemButton)scene[index]).RemoveDevice(device);
            }
            for (int index = 0; index < circuit.Length; index++)
            {
                ((contract.itemLightingCircuit)circuit[index]).RemoveDevice(device);
            }
        }

        #endregion

        #region CH5 Contract

        public contract.IitemButton[] scene { get; private set; }

        public contract.IitemLightingCircuit[] circuit { get; private set; }

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

            for (int index = 0; index < scene.Length; index++)
            {
                ((contract.itemButton)scene[index]).Dispose();
            }
            for (int index = 0; index < circuit.Length; index++)
            {
                ((contract.itemLightingCircuit)circuit[index]).Dispose();
            }

            toggle = null;
        }

        #endregion

    }
}
