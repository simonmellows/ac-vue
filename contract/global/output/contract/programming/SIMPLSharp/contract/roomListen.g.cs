using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace contract
{
    public interface IroomListen
    {
        object UserObject { get; set; }

        event EventHandler<UIEventArgs> toggle;

        void visibleFb(roomListenBoolInputSigDelegate callback);
        void toggleFb(roomListenBoolInputSigDelegate callback);

        contract.IitemButton[] source { get; }
    }

    public delegate void roomListenBoolInputSigDelegate(BoolInputSig boolInputSig, IroomListen roomListen);

    internal class roomListen : IroomListen, IDisposable
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

        internal roomListen(ComponentMediator componentMediator, uint controlJoinId)
        {
            ComponentMediator = componentMediator;
            Initialize(controlJoinId);
        }

        private static readonly IDictionary<uint, List<uint>> SourceSmartObjectIdMappings = new Dictionary<uint, List<uint>> {
            { 7, new List<uint> { 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122 } }, 
            { 193, new List<uint> { 289, 290, 291, 292, 293, 294, 295, 296, 297, 298, 299, 300, 301, 302, 303, 304, 305, 306, 307, 308 } }, 
            { 379, new List<uint> { 475, 476, 477, 478, 479, 480, 481, 482, 483, 484, 485, 486, 487, 488, 489, 490, 491, 492, 493, 494 } }, 
            { 565, new List<uint> { 661, 662, 663, 664, 665, 666, 667, 668, 669, 670, 671, 672, 673, 674, 675, 676, 677, 678, 679, 680 } }, 
            { 751, new List<uint> { 847, 848, 849, 850, 851, 852, 853, 854, 855, 856, 857, 858, 859, 860, 861, 862, 863, 864, 865, 866 } }};

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


        public void visibleFb(roomListenBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.visibleFb], this);
            }
        }

        public void toggleFb(roomListenBoolInputSigDelegate callback)
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
            return string.Format("Contract: {0} Component: {1} HashCode: {2} {3}", "roomListen", GetType().Name, GetHashCode(), UserObject != null ? "UserObject: " + UserObject : null);
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
