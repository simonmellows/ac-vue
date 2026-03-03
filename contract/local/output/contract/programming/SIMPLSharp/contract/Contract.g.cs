using System;
using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro;

namespace contract
{
    /// <summary>
    /// Common Interface for Root Contracts.
    /// </summary>
    public interface IContract
    {
        object UserObject { get; set; }
        void AddDevice(BasicTriListWithSmartObject device);
        void RemoveDevice(BasicTriListWithSmartObject device);
    }

    public class Contract : IContract, IDisposable
    {
        #region Components

        private ComponentMediator ComponentMediator { get; set; }

        public contract.Iui ui { get { return (contract.Iui)Internalui; } }
        private contract.ui Internalui { get; set; }

        public contract.IitemRoom[] rooms { get { return Internalrooms.Cast<contract.IitemRoom>().ToArray(); } }
        private contract.itemRoom[] Internalrooms { get; set; }

        public contract.IitemRoomGroup[] roomGroups { get { return InternalroomGroups.Cast<contract.IitemRoomGroup>().ToArray(); } }
        private contract.itemRoomGroup[] InternalroomGroups { get; set; }

        public contract.IcurrentRoom currentRoom { get { return (contract.IcurrentRoom)InternalcurrentRoom; } }
        private contract.currentRoom InternalcurrentRoom { get; set; }

        public contract.IcurrentSource currentSourceWatch { get { return (contract.IcurrentSource)InternalcurrentSourceWatch; } }
        private contract.currentSource InternalcurrentSourceWatch { get; set; }

        public contract.IcurrentSource currentSourceListen { get { return (contract.IcurrentSource)InternalcurrentSourceListen; } }
        private contract.currentSource InternalcurrentSourceListen { get; set; }

        public contract.Iproperty property { get { return (contract.Iproperty)Internalproperty; } }
        private contract.property Internalproperty { get; set; }

        #endregion

        #region Construction and Initialization

        private static readonly IDictionary<int, uint> RoomsSmartObjectIdMappings = new Dictionary<int, uint>{
            { 0, 2 }, { 1, 3 }, { 2, 4 }, { 3, 5 }, { 4, 6 }};
        private static readonly IDictionary<int, uint> RoomGroupsSmartObjectIdMappings = new Dictionary<int, uint>{
            { 0, 7 }, { 1, 8 }, { 2, 9 }, { 3, 10 }, { 4, 11 }};

        public Contract()
            : this(new List<BasicTriListWithSmartObject>().ToArray())
        {
        }

        public Contract(BasicTriListWithSmartObject device)
            : this(new [] { device })
        {
        }

        public Contract(BasicTriListWithSmartObject[] devices)
        {
            if (devices == null)
                throw new ArgumentNullException("Devices is null");

            ComponentMediator = new ComponentMediator();

            Internalui = new contract.ui(ComponentMediator, 1);
            Internalrooms = new contract.itemRoom[RoomsSmartObjectIdMappings.Count];
            for (int index = 0; index < RoomsSmartObjectIdMappings.Count; index++)
            {
                Internalrooms[index] = new contract.itemRoom(ComponentMediator, RoomsSmartObjectIdMappings[index]);
            }
            InternalroomGroups = new contract.itemRoomGroup[RoomGroupsSmartObjectIdMappings.Count];
            for (int index = 0; index < RoomGroupsSmartObjectIdMappings.Count; index++)
            {
                InternalroomGroups[index] = new contract.itemRoomGroup(ComponentMediator, RoomGroupsSmartObjectIdMappings[index]);
            }
            InternalcurrentRoom = new contract.currentRoom(ComponentMediator, 12);
            InternalcurrentSourceWatch = new contract.currentSource(ComponentMediator, 198);
            InternalcurrentSourceListen = new contract.currentSource(ComponentMediator, 199);
            Internalproperty = new contract.property(ComponentMediator, 200);

            for (int index = 0; index < devices.Length; index++)
            {
                AddDevice(devices[index]);
            }
        }

        public static void ClearDictionaries()
        {
            RoomsSmartObjectIdMappings.Clear();
            RoomGroupsSmartObjectIdMappings.Clear();

            contract.currentRoom.ClearDictionaries();
            contract.property.ClearDictionaries();
        }

        #endregion

        #region Standard Contract Members

        public object UserObject { get; set; }

        public void AddDevice(BasicTriListWithSmartObject device)
        {
            Internalui.AddDevice(device);
            for (int index = 0; index < 5; index++)
            {
                Internalrooms[index].AddDevice(device);
            }
            for (int index = 0; index < 5; index++)
            {
                InternalroomGroups[index].AddDevice(device);
            }
            InternalcurrentRoom.AddDevice(device);
            InternalcurrentSourceWatch.AddDevice(device);
            InternalcurrentSourceListen.AddDevice(device);
            Internalproperty.AddDevice(device);
        }

        public void RemoveDevice(BasicTriListWithSmartObject device)
        {
            Internalui.RemoveDevice(device);
            for (int index = 0; index < 5; index++)
            {
                Internalrooms[index].RemoveDevice(device);
            }
            for (int index = 0; index < 5; index++)
            {
                InternalroomGroups[index].RemoveDevice(device);
            }
            InternalcurrentRoom.RemoveDevice(device);
            InternalcurrentSourceWatch.RemoveDevice(device);
            InternalcurrentSourceListen.RemoveDevice(device);
            Internalproperty.RemoveDevice(device);
        }

        #endregion

        #region IDisposable

        public bool IsDisposed { get; set; }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            Internalui.Dispose();
            for (int index = 0; index < 5; index++)
            {
                Internalrooms[index].Dispose();
            }
            for (int index = 0; index < 5; index++)
            {
                InternalroomGroups[index].Dispose();
            }
            InternalcurrentRoom.Dispose();
            InternalcurrentSourceWatch.Dispose();
            InternalcurrentSourceListen.Dispose();
            Internalproperty.Dispose();
            ComponentMediator.Dispose(); 
        }

        #endregion

    }
}
