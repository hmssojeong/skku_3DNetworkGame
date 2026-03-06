using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class UI_Room : MonoBehaviourPunCallbacks
{
    private List<UI_RoomItem> _roomItems;
    private Dictionary<string, RoomInfo> _rooms = new();

    private void Awake()
    {
        _roomItems = GetComponentsInChildren<UI_RoomItem>().ToList();

        HideAllRoomUI();
    }
    private void HideAllRoomUI()
    {
        foreach(UI_RoomItem roomItem in _roomItems)
        {
            roomItem.gameObject.SetActive(false);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        HideAllRoomUI();

        foreach (var room in roomList)
        {
            if(room.RemovedFromList)
            {
                _rooms.Remove(room.Name);
            }
            else
            {
                _rooms[room.Name] = room;
            }
        }


        int roomCount = _rooms.Count;
        List<RoomInfo> rooms = _rooms.Values.ToList();

        for (int i = 0; i < roomCount; i++)
        {
            _roomItems[i].Init(rooms[i]);
            _roomItems[i].gameObject.SetActive(true);
        }
    }

}
