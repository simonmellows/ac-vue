using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace contract
{
    public interface IroomWatch
    {
        object UserObject { get; set; }

        event EventHandler<UIEventArgs> toggle;

        void visibleFb(roomWatchBoolInputSigDelegate callback);
        void toggleFb(roomWatchBoolInputSigDelegate callback);

        contract.IitemButton[] source { get; }
    }

    public delegate void roomWatchBoolInputSigDelegate(BoolInputSig boolInputSig, IroomWatch roomWatch);

    internal class roomWatch : IroomWatch, IDisposable
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

        internal roomWatch(ComponentMediator componentMediator, uint controlJoinId)
        {
            ComponentMediator = componentMediator;
            Initialize(controlJoinId);
        }

        private static readonly IDictionary<uint, List<uint>> SourceSmartObjectIdMappings = new Dictionary<uint, List<uint>> {
            { 6, new List<uint> { 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102 } }, 
            { 192, new List<uint> { 269, 270, 271, 272, 273, 274, 275, 276, 277, 278, 279, 280, 281, 282, 283, 284, 285, 286, 287, 288 } }, 
            { 378, new List<uint> { 455, 456, 457, 458, 459, 460, 461, 462, 463, 464, 465, 466, 467, 468, 469, 470, 471, 472, 473, 474 } }, 
            { 564, new List<uint> { 641, 642, 643, 644, 645, 646, 647, 648, 649, 650, 651, 652, 653, 654, 655, 656, 657, 658, 659, 660 } }, 
            { 750, new List<uint> { 827, 828, 829, 830, 831, 832, 833, 834, 835, 836, 837, 838, 839, 840, 841, 842, 843, 844, 845, 846 } }};

        internal static void ClearDictionaries()
        {
            SourceSmartObjectIdMappings.Clear();
        }

        private void Initialize(uint controlJoinId)
        {
            ControlJoinId = controlJoinId; 
 
            _devices = new List<BasicTriListWithSmartObject>(); 
 
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.toggle, ontoggle);

            List<uint> sourceList = SourceSmartObjectIdMappings[controlJoinId];
            source = new contract.IitemButton[sourceList.Count];
            for (int index = 0; index < sourceList.Count; index++)
            {
                source[index] = new contract.itemButton(ComponentMediator, sourceList[index]); 
            }

        }

        public void AddDevice(BasicTriListWithSmartObject device)
        {
            Devices.Add(device);
            ComponentMediator.HookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
            for (int index = 0; index < source.Length; index++)
            {
                ((contract.itemButton)source[index]).AddDevice(device);
            }
        }

        public void RemoveDevice(BasicTriListWithSmartObject device)
        {
            Devices.Remove(device);
            ComponentMediator.UnHookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
            for (int index = 0; index < source.Length; index++)
            {
                ((contract.itemButton)source[index]).RemoveDevice(device);
            }
        }

        #endregion

        #region CH5 Contract

        public contract.IitemButton[] source { get; private set; }

        public event EventHandler<UIEventArgs> toggle;
        private void ontoggle(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = toggle;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void visibleFb(roomWatchBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.visibleFb], this);
            }
        }

        public void toggleFb(roomWatchBoolInputSigDelegate callback)
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
            return string.Format("Contract: {0} Component: {1} HashCode: {2} {3}", "roomWatch", GetType().Name, GetHashCode(), UserObject != null ? "UserObject: " + UserObject : null);
        }

        #endregion

        #region IDisposable

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            for (int index = 0; index < source.Length; index++)
            {
                ((contract.itemButton)source[index]).Dispose();
            }

            toggle = null;
        }

        #endregion

    }
}
