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

        public contract.IitemRoom[] room { get { return Internalroom.Cast<contract.IitemRoom>().ToArray(); } }
        private contract.itemRoom[] Internalroom { get; set; }

        public contract.IitemRoomGroup[] roomGroup { get { return InternalroomGroup.Cast<contract.IitemRoomGroup>().ToArray(); } }
        private contract.itemRoomGroup[] InternalroomGroup { get; set; }

        #endregion

        #region Construction and Initialization

        private static readonly IDictionary<int, uint> RoomSmartObjectIdMappings = new Dictionary<int, uint>{
            { 0, 2 }, { 1, 188 }, { 2, 374 }, { 3, 560 }, { 4, 746 }};
        private static readonly IDictionary<int, uint> RoomGroupSmartObjectIdMappings = new Dictionary<int, uint>{
            { 0, 932 }, { 1, 933 }, { 2, 934 }, { 3, 935 }, { 4, 936 }};

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
            Internalroom = new contract.itemRoom[RoomSmartObjectIdMappings.Count];
            for (int index = 0; index < RoomSmartObjectIdMappings.Count; index++)
            {
                Internalroom[index] = new contract.itemRoom(ComponentMediator, RoomSmartObjectIdMappings[index]);
            }
            InternalroomGroup = new contract.itemRoomGroup[RoomGroupSmartObjectIdMappings.Count];
            for (int index = 0; index < RoomGroupSmartObjectIdMappings.Count; index++)
            {
                InternalroomGroup[index] = new contract.itemRoomGroup(ComponentMediator, RoomGroupSmartObjectIdMappings[index]);
            }

            for (int index = 0; index < devices.Length; index++)
            {
                AddDevice(devices[index]);
            }
        }

        public static void ClearDictionaries()
        {
            RoomSmartObjectIdMappings.Clear();
            RoomGroupSmartObjectIdMappings.Clear();

            contract.itemRoom.ClearDictionaries();
        }

        #endregion

        #region Standard Contract Members

        public object UserObject { get; set; }

        public void AddDevice(BasicTriListWithSmartObject device)
        {
            Internalui.AddDevice(device);
            for (int index = 0; index < 5; index++)
            {
                Internalroom[index].AddDevice(device);
            }
            for (int index = 0; index < 5; index++)
            {
                InternalroomGroup[index].AddDevice(device);
            }
        }

        public void RemoveDevice(BasicTriListWithSmartObject device)
        {
            Internalui.RemoveDevice(device);
            for (int index = 0; index < 5; index++)
            {
                Internalroom[index].RemoveDevice(device);
            }
            for (int index = 0; index < 5; index++)
            {
                InternalroomGroup[index].RemoveDevice(device);
            }
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
                Internalroom[index].Dispose();
            }
            for (int index = 0; index < 5; index++)
            {
                InternalroomGroup[index].Dispose();
            }
            ComponentMediator.Dispose(); 
        }

        #endregion

    }
}
