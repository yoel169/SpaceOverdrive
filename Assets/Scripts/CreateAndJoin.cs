using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateAndJoin : MonoBehaviourPunCallbacks
{

    [SerializeField] public TMP_InputField createInput;
    [SerializeField] public TMP_InputField joinInput;

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
        
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Main Menu");
    }

    public override void OnJoinedRoom()
    {
        //PhotonNetwork.LoadLevel("M Game");
        SceneManager.LoadScene("Pick Ship");
    }
}

