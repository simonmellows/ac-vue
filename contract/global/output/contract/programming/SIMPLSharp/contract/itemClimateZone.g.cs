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

        contract.IitemButton[] mode { get; }
        contract.IitemButton[] fanSpeed { get; }
        contract.IitemButton[] toggle { get; }
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

        private static readonly IDictionary<uint, List<uint>> ModeSmartObjectIdMappings = new Dictionary<uint, List<uint>> {
            { 78, new List<uint> { 123, 124, 125, 126, 127 } }, { 79, new List<uint> { 136, 137, 138, 139, 140 } }, 
            { 80, new List<uint> { 149, 150, 151, 152, 153 } }, { 81, new List<uint> { 162, 163, 164, 165, 166 } }, 
            { 82, new List<uint> { 175, 176, 177, 178, 179 } }, { 264, new List<uint> { 309, 310, 311, 312, 313 } }, 
            { 265, new List<uint> { 322, 323, 324, 325, 326 } }, { 266, new List<uint> { 335, 336, 337, 338, 339 } }, 
            { 267, new List<uint> { 348, 349, 350, 351, 352 } }, { 268, new List<uint> { 361, 362, 363, 364, 365 } }, 
            { 450, new List<uint> { 495, 496, 497, 498, 499 } }, { 451, new List<uint> { 508, 509, 510, 511, 512 } }, 
            { 452, new List<uint> { 521, 522, 523, 524, 525 } }, { 453, new List<uint> { 534, 535, 536, 537, 538 } }, 
            { 454, new List<uint> { 547, 548, 549, 550, 551 } }, { 636, new List<uint> { 681, 682, 683, 684, 685 } }, 
            { 637, new List<uint> { 694, 695, 696, 697, 698 } }, { 638, new List<uint> { 707, 708, 709, 710, 711 } }, 
            { 639, new List<uint> { 720, 721, 722, 723, 724 } }, { 640, new List<uint> { 733, 734, 735, 736, 737 } }, 
            { 822, new List<uint> { 867, 868, 869, 870, 871 } }, { 823, new List<uint> { 880, 881, 882, 883, 884 } }, 
            { 824, new List<uint> { 893, 894, 895, 896, 897 } }, { 825, new List<uint> { 906, 907, 908, 909, 910 } }, 
            { 826, new List<uint> { 919, 920, 921, 922, 923 } }};
        private static readonly IDictionary<uint, List<uint>> FanSpeedSmartObjectIdMappings = new Dictionary<uint, List<uint>> {
            { 78, new List<uint> { 128, 129, 130, 131, 132 } }, { 79, new List<uint> { 141, 142, 143, 144, 145 } }, 
            { 80, new List<uint> { 154, 155, 156, 157, 158 } }, { 81, new List<uint> { 167, 168, 169, 170, 171 } }, 
            { 82, new List<uint> { 180, 181, 182, 183, 184 } }, { 264, new List<uint> { 314, 315, 316, 317, 318 } }, 
            { 265, new List<uint> { 327, 328, 329, 330, 331 } }, { 266, new List<uint> { 340, 341, 342, 343, 344 } }, 
            { 267, new List<uint> { 353, 354, 355, 356, 357 } }, { 268, new List<uint> { 366, 367, 368, 369, 370 } }, 
            { 450, new List<uint> { 500, 501, 502, 503, 504 } }, { 451, new List<uint> { 513, 514, 515, 516, 517 } }, 
            { 452, new List<uint> { 526, 527, 528, 529, 530 } }, { 453, new List<uint> { 539, 540, 541, 542, 543 } }, 
            { 454, new List<uint> { 552, 553, 554, 555, 556 } }, { 636, new List<uint> { 686, 687, 688, 689, 690 } }, 
            { 637, new List<uint> { 699, 700, 701, 702, 703 } }, { 638, new List<uint> { 712, 713, 714, 715, 716 } }, 
            { 639, new List<uint> { 725, 726, 727, 728, 729 } }, { 640, new List<uint> { 738, 739, 740, 741, 742 } }, 
            { 822, new List<uint> { 872, 873, 874, 875, 876 } }, { 823, new List<uint> { 885, 886, 887, 888, 889 } }, 
            { 824, new List<uint> { 898, 899, 900, 901, 902 } }, { 825, new List<uint> { 911, 912, 913, 914, 915 } }, 
            { 826, new List<uint> { 924, 925, 926, 927, 928 } }};
        private static readonly IDictionary<uint, List<uint>> ToggleSmartObjectIdMappings = new Dictionary<uint, List<uint>> {
            { 78, new List<uint> { 133, 134, 135 } }, { 79, new List<uint> { 146, 147, 148 } }, { 80, new List<uint> { 159, 160, 161 } }, 
            { 81, new List<uint> { 172, 173, 174 } }, { 82, new List<uint> { 185, 186, 187 } }, { 264, new List<uint> { 319, 320, 321 } }, 
            { 265, new List<uint> { 332, 333, 334 } }, { 266, new List<uint> { 345, 346, 347 } }, { 267, new List<uint> { 358, 359, 360 } }, 
            { 268, new List<uint> { 371, 372, 373 } }, { 450, new List<uint> { 505, 506, 507 } }, { 451, new List<uint> { 518, 519, 520 } }, 
            { 452, new List<uint> { 531, 532, 533 } }, { 453, new List<uint> { 544, 545, 546 } }, { 454, new List<uint> { 557, 558, 559 } }, 
            { 636, new List<uint> { 691, 692, 693 } }, { 637, new List<uint> { 704, 705, 706 } }, { 638, new List<uint> { 717, 718, 719 } }, 
            { 639, new List<uint> { 730, 731, 732 } }, { 640, new List<uint> { 743, 744, 745 } }, { 822, new List<uint> { 877, 878, 879 } }, 
            { 823, new List<uint> { 890, 891, 892 } }, { 824, new List<uint> { 903, 904, 905 } }, { 825, new List<uint> { 916, 917, 918 } }, 
            { 826, new List<uint> { 929, 930, 931 } }};

        internal static void ClearDictionaries()
        {
            ModeSmartObjectIdMappings.Clear();
            FanSpeedSmartObjectIdMappings.Clear();
            ToggleSmartObjectIdMappings.Clear();
        }

        private void Initialize(uint controlJoinId)
        {
            ControlJoinId = controlJoinId; 
 
            _devices = new List<BasicTriListWithSmartObject>(); 
 
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.setpointRaise, onsetpointRaise);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.setpointLower, onsetpointLower);
            ComponentMediator.ConfigureBooleanEvent(controlJoinId, Joins.Booleans.enable, onenable);

            List<uint> modeList = ModeSmartObjectIdMappings[controlJoinId];
            mode = new contract.IitemButton[modeList.Count];
            for (int index = 0; index < modeList.Count; index++)
            {
                mode[index] = new contract.itemButton(ComponentMediator, modeList[index]); 
            }

            List<uint> fanSpeedList = FanSpeedSmartObjectIdMappings[controlJoinId];
            fanSpeed = new contract.IitemButton[fanSpeedList.Count];
            for (int index = 0; index < fanSpeedList.Count; index++)
            {
                fanSpeed[index] = new contract.itemButton(ComponentMediator, fanSpeedList[index]); 
            }

            List<uint> toggleList = ToggleSmartObjectIdMappings[controlJoinId];
            toggle = new contract.IitemButton[toggleList.Count];
            for (int index = 0; index < toggleList.Count; index++)
            {
                toggle[index] = new contract.itemButton(ComponentMediator, toggleList[index]); 
            }

        }

        public void AddDevice(BasicTriListWithSmartObject device)
        {
            Devices.Add(device);
            ComponentMediator.HookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
            for (int index = 0; index < mode.Length; index++)
            {
                ((contract.itemButton)mode[index]).AddDevice(device);
            }
            for (int index = 0; index < fanSpeed.Length; index++)
            {
                ((contract.itemButton)fanSpeed[index]).AddDevice(device);
            }
            for (int index = 0; index < toggle.Length; index++)
            {
                ((contract.itemButton)toggle[index]).AddDevice(device);
            }
        }

        public void RemoveDevice(BasicTriListWithSmartObject device)
        {
            Devices.Remove(device);
            ComponentMediator.UnHookSmartObjectEvents(device.SmartObjects[ControlJoinId]);
            for (int index = 0; index < mode.Length; index++)
            {
                ((contract.itemButton)mode[index]).RemoveDevice(device);
            }
            for (int index = 0; index < fanSpeed.Length; index++)
            {
                ((contract.itemButton)fanSpeed[index]).RemoveDevice(device);
            }
            for (int index = 0; index < toggle.Length; index++)
            {
                ((contract.itemButton)toggle[index]).RemoveDevice(device);
            }
        }

        #endregion

        #region CH5 Contract

        public contract.IitemButton[] mode { get; private set; }

        public contract.IitemButton[] fanSpeed { get; private set; }

        public contract.IitemButton[] toggle { get; private set; }

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

            for (int index = 0; index < mode.Length; index++)
            {
                ((contract.itemButton)mode[index]).Dispose();
            }
            for (int index = 0; index < fanSpeed.Length; index++)
            {
                ((contract.itemButton)fanSpeed[index]).Dispose();
            }
            for (int index = 0; index < toggle.Length; index++)
            {
                ((contract.itemButton)toggle[index]).Dispose();
            }

            setpointRaise = null;
            setpointLower = null;
            enable = null;
        }

        #endregion

    }
}
