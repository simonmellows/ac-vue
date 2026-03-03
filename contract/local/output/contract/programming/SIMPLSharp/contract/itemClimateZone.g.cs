using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace contract
{
    public interface IitemClimateZone
    {
        object UserObject { get; set; }

        event EventHandler<UIEventArgs> setpointRaise;
        event EventHandler<UIEventArgs> setpointLower;
        event EventHandler<UIEventArgs> enable;

        void visibleFb(itemClimateZoneBoolInputSigDelegate callback);
        void hasEnableFb(itemClimateZoneBoolInputSigDelegate callback);
        void enableFb(itemClimateZoneBoolInputSigDelegate callback);
        void hasScheduleFb(itemClimateZoneBoolInputSigDelegate callback);
        void labelFb(itemClimateZoneStringInputSigDelegate callback);
        void setpointFb(itemClimateZoneStringInputSigDelegate callback);
        void temperatureFb(itemClimateZoneStringInputSigDelegate callback);

        contract.IitemButton[] modes { get; }
        contract.IitemButton[] fanSpeeds { get; }
        contract.IitemButton[] toggles { get; }
    }

    public delegate void itemClimateZoneBoolInputSigDelegate(BoolInputSig boolInputSig, IitemClimateZone itemClimateZone);
    public delegate void itemClimateZoneStringInputSigDelegate(StringInputSig stringInputSig, IitemClimateZone itemClimateZone);

    internal class itemClimateZone : IitemClimateZone, IDisposable
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
                public const uint setpointRaise = 2;
                public const uint setpointLower = 3;
                public const uint enable = 6;

                public const uint visibleFb = 1;
                public const uint hasEnableFb = 2;
                public const uint enableFb = 3;
                public const uint hasScheduleFb = 6;
            }
            internal static class Strings
            {

                public const uint labelFb = 1;
                public const uint setpointFb = 2;
                public const uint temperatureFb = 3;
            }
        }

        #endregion

        #region Construction and Initialization

        internal itemClimateZone(ComponentMediator componentMediator, uint controlJoinId)
        {
            ComponentMediator = componentMediator;
            Initialize(controlJoinId);
        }

        private static readonly IDictionary<uint, List<uint>> ModesSmartObjectIdMappings = new Dictionary<uint, List<uint>> {
            { 88, new List<uint> { 133, 134, 135, 136, 137 } }, { 89, new List<uint> { 146, 147, 148, 149, 150 } }, 
            { 90, new List<uint> { 159, 160, 161, 162, 163 } }, { 91, new List<uint> { 172, 173, 174, 175, 176 } }, 
            { 92, new List<uint> { 185, 186, 187, 188, 189 } }};
        private static readonly IDictionary<uint, List<uint>> FanSpeedsSmartObjectIdMappings = new Dictionary<uint, List<uint>> {
            { 88, new List<uint> { 138, 139, 140, 141, 142 } }, { 89, new List<uint> { 151, 152, 153, 154, 155 } }, 
            { 90, new List<uint> { 164, 165, 166, 167, 168 } }, { 91, new List<uint> { 177, 178, 179, 180, 181 } }, 
            { 92, new List<uint> { 190, 191, 192, 193, 194 } }};
        private static readonly IDictionary<uint, List<uint>> TogglesSmartObjectIdMappings = new Dictionary<uint, List<uint>> {
            { 88, new List<uint> { 143, 144, 145 } }, { 89, new List<uint> { 156, 157, 158 } }, { 90, new List<uint> { 169, 170, 171 } }, 
            { 91, new List<uint> { 182, 183, 184 } }, { 92, new List<uint> { 195, 196, 197 } }};

        internal static void ClearDictionaries()
        {
            ModesSmartObjectIdMappings.Clear();
            FanSpeedsSmartObjectIdMappings.Clear();
            TogglesSmartObjectIdMappings.Clear();
        }

        private void Initialize(uint controlJoinId)
        {
            ControlJoinId = controlJoinId; 
 
            _devices = new List<BasicTriListWithSmartObject>(); 
 
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.setpointRaise, onsetpointRaise);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.setpointLower, onsetpointLower);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.enable, onenable);

            List<uint> modesList = ModesSmartObjectIdMappings[controlJoinId];
            modes = new contract.IitemButton[modesList.Count];
            for (int index = 0; index < modesList.Count; index++)
            {
                modes[index] = new contract.itemButton(ComponentMediator, modesList[index]); 
            }

            List<uint> fanSpeedsList = FanSpeedsSmartObjectIdMappings[controlJoinId];
            fanSpeeds = new contract.IitemButton[fanSpeedsList.Count];
            for (int index = 0; index < fanSpeedsList.Count; index++)
            {
                fanSpeeds[index] = new contract.itemButton(ComponentMediator, fanSpeedsList[index]); 
            }

            List<uint> togglesList = TogglesSmartObjectIdMappings[controlJoinId];
            toggles = new contract.IitemButton[togglesList.Count];
            for (int index = 0; index < togglesList.Count; index++)
            {
                toggles[index] = new contract.itemButton(ComponentMediator, togglesList[index]); 
            }

        }

        public void AddDevice(BasicTriListWithSmartObject device)
        {
            Devices.Add(device);
            ComponentMediator.HookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
            for (int index = 0; index < modes.Length; index++)
            {
                ((contract.itemButton)modes[index]).AddDevice(device);
            }
            for (int index = 0; index < fanSpeeds.Length; index++)
            {
                ((contract.itemButton)fanSpeeds[index]).AddDevice(device);
            }
            for (int index = 0; index < toggles.Length; index++)
            {
                ((contract.itemButton)toggles[index]).AddDevice(device);
            }
        }

        public void RemoveDevice(BasicTriListWithSmartObject device)
        {
            Devices.Remove(device);
            ComponentMediator.UnHookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
            for (int index = 0; index < modes.Length; index++)
            {
                ((contract.itemButton)modes[index]).RemoveDevice(device);
            }
            for (int index = 0; index < fanSpeeds.Length; index++)
            {
                ((contract.itemButton)fanSpeeds[index]).RemoveDevice(device);
            }
            for (int index = 0; index < toggles.Length; index++)
            {
                ((contract.itemButton)toggles[index]).RemoveDevice(device);
            }
        }

        #endregion

        #region CH5 Contract

        public contract.IitemButton[] modes { get; private set; }

        public contract.IitemButton[] fanSpeeds { get; private set; }

        public contract.IitemButton[] toggles { get; private set; }

        public event EventHandler<UIEventArgs> setpointRaise;
        private void onsetpointRaise(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = setpointRaise;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> setpointLower;
        private void onsetpointLower(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = setpointLower;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }

        public event EventHandler<UIEventArgs> enable;
        private void onenable(SmartObjectEventArgs eventArgs)
        {
            EventHandler<UIEventArgs> handler = enable;
            if (handler != null)
                handler(this, UIEventArgs.CreateEventArgs(eventArgs));
        }


        public void visibleFb(itemClimateZoneBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.visibleFb], this);
            }
        }

        public void hasEnableFb(itemClimateZoneBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.hasEnableFb], this);
            }
        }

        public void enableFb(itemClimateZoneBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.enableFb], this);
            }
        }

        public void hasScheduleFb(itemClimateZoneBoolInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].BooleanInput[Joins.Booleans.hasScheduleFb], this);
            }
        }


        public void labelFb(itemClimateZoneStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.labelFb], this);
            }
        }

        public void setpointFb(itemClimateZoneStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.setpointFb], this);
            }
        }

        public void temperatureFb(itemClimateZoneStringInputSigDelegate callback)
        {
            for (int index = 0; index < Devices.Count; index++)
            {
                callback(Devices[index].SmartObjects[ControlJoinId].StringInput[Joins.Strings.temperatureFb], this);
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
            return string.Format("Contract: {0} Component: {1} HashCode: {2} {3}", "itemClimateZone", GetType().Name, GetHashCode(), UserObject != null ? "UserObject: " + UserObject : null);
        }

        #endregion

        #region IDisposable

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            for (int index = 0; index < modes.Length; index++)
            {
                ((contract.itemButton)modes[index]).Dispose();
            }
            for (int index = 0; index < fanSpeeds.Length; index++)
            {
                ((contract.itemButton)fanSpeeds[index]).Dispose();
            }
            for (int index = 0; index < toggles.Length; index++)
            {
                ((contract.itemButton)toggles[index]).Dispose();
            }

            setpointRaise = null;
            setpointLower = null;
            enable = null;
        }

        #endregion

    }
}
