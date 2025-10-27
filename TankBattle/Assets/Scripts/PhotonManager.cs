using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonManager : Photon.MonoBehaviour
{
    public void OnConnectedToPhoton()
    {
        Debug.Log("OnConnectedToPhoton");
    }
    public void OnDisconnectedFromPhoton()
    {
        Debug.Log("OnDisconnectedFromPhoton");
    }
    public void OnConnectionFail()
    {
        Debug.Log("OnConnectionFail");
    }
    public void OnFailedToConnectToPhoton(object parameters)
    {
        Debug.Log("OnFailedToConnectToPhoton");
    }
    public void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");

    }
    public void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
    }
    public void OnReceivedRoomListUpdate()
    {
        Debug.Log("OnReceivedRoomListUpdate");

        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        Debug.Log(string.Format("room count is {0}", rooms.Length));
        foreach (RoomInfo infor in rooms)
        {
            Debug.Log("Room: " + infor.name);
        }
    }
    public void OnLeftLobby()
    {
        Debug.Log("OnLeftLobby");
    }
    public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");

    }
    public void OnPhotonCreateRoomFailed()
    {
        Debug.Log("OnPhotonCreateRoomFailed");
    }



    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        Debug.Log(string.Format("Name:{0}", PhotonNetwork.room.name));
        Debug.Log("OnJoinedRoom");
    }



    public void OnPhotonJoinRoomFailed(object[] cause)
    {
        Debug.Log("OnPhotonJoinRoomFailed");
    }


    public void OnPhotonRandomJoinFailed()
    {
        Debug.Log("OnPhotonRandomJoinFailed");
        CreateRoom();
    }


    public void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
    }
    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("OnPhotonPlayerConnected");
    }
    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        Debug.Log("OnPhotonPlayerDisconnected");
    }
    public void OnMasterClientSwitched(PhotonPlayer player)
    {
        Debug.Log("OnMasterClientSwitched");
    }
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings("1.0");

        PhotonNetwork.sendRate = 60;
        PhotonNetwork.sendRateOnSerialize = 60;

        PhotonNetwork.automaticallySyncScene = true;
    }

    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(null, roomOptions, null);
    }
}
