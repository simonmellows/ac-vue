using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace contract
{
    public interface IroomShading
    {
        object UserObject { get; set; }

        event EventHandler<UIEventArgs> toggle;

        void visibleFb(roomShadingBoolInputSigDelegate callback);
        void toggleFb(roomShadingBoolInputSigDelegate callback);

        contract.IitemButton[] scene { get; }
    }

    public delegate void roomShadingBoolInputSigDelegate(BoolInputSig boolInputSig, IroomShading roomShading);

    internal class roomShading : IroomShading, IDisposable
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

        internal roomShading(ComponentMediator componentMediator, uint controlJoinId)
        {
            ComponentMediator = componentMediator;
            Initialize(controlJoinId);
        }

        private static readonly IDictionary<uint, List<uint>> SceneSmartObjectIdMappings = new Dictionary<uint, List<uint>> {
            { 4, new List<uint> { 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77 } }, 
            { 190, new List<uint> { 244, 245, 246, 247, 248, 249, 250, 251, 252, 253, 254, 255, 256, 257, 258, 259, 260, 261, 262, 263 } }, 
            { 376, new List<uint> { 430, 431, 432, 433, 434, 435, 436, 437, 438, 439, 440, 441, 442, 443, 444, 445, 446, 447, 448, 449 } }, 
            { 562, new List<uint> { 616, 617, 618, 619, 620, 621, 622, 623, 624, 625, 626, 627, 628, 629, 630, 631, 632, 633, 634, 635 } }, 
            { 748, new List<uint> { 802, 803, 804, 805, 806, 807, 808, 809, 810, 811, 812, 813, 814, 815, 816, 817, 818, 819, 820, 821 } }};

        internal static void ClearDictionaries()
        {
            SceneSmartObjectIdMappings.Clear();
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

        }

        public void AddDevice(BasicTriListWithSmartObject device)
        {
            Devices.Add(device);
            ComponentMediator.HookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
            for (int index = 0; index < scene.Length; index++)
            {
                ((contract.itemButton)scene[index]).AddDevice(device);
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
        }

        #endregion

        #region CH5 Contract

        public contract.IitemButton[] scene { get; private set; }

        public event EventHandler<UIEventArgs> toggle;
        private void ontoggle(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = toggle;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void visibleFb(roomShadingBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.visibleFb], this);
            }
        }

        public void toggleFb(roomShadingBoolInputSigDelegate callback)
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
            return string.Format("Contract: {0} Component: {1} HashCode: {2} {3}", "roomShading", GetType().Name, GetHashCode(), UserObject != null ? "UserObject: " + UserObject : null);
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

            toggle = null;
        }

        #endregion

    }
}
