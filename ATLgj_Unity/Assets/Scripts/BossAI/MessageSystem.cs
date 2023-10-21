using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MessageType {
    DAMAGED,
    DEAD,
    RESPAWN,
}

public interface IMessageReceiver {
    void OnRecieveMessage(MessageType type, object sender, object msg);
}

