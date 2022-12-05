using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using System;

public class kinematics : MonoBehaviour
{
    private MqttClient client; 
    public GameObject[] Links;
    public Slider[] sliders;
    public Vector3 link1;
   
    // Start is called before the first frame update
    void Start()
    {
        Links[1].transform.eulerAngles = new Vector3(0f, -90f, -90f);
        Links[2].transform.eulerAngles = new Vector3(0f, 90f, 0f);
        client = new MqttClient("broker.emqx.io", 1883, false, null);
        
        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId);

        if (client.IsConnected)
        {
            Debug.Log("Connected to MQTT Broker");
        }
        else
        {
            Debug.Log("Failed to connect");
        }

        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
        // subscribe to the topic "/home/temperature" with QoS 2 
        client.Subscribe(new string[] { "hello/world" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Llegada de datos de MQTT: 
    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message));
    }

    /// <summary>
    /// Base
    /// </summary>
    public void slider_val1()
    {
        Debug.Log(sliders[0].value);
        //angulos.y = sliders[0].value;
        Links[0].transform.eulerAngles = new Vector3(0f, sliders[0].value, 0f); 
    }

    public void slider_val2()
    {
        Debug.Log(sliders[1].value);
        Links[1].transform.localEulerAngles = new Vector3(0, -90, sliders[1].value - 90);
        string msg = sliders[1].value.ToString();
        client.Publish("Mot1", System.Text.Encoding.UTF8.GetBytes(msg));
    }

    public void slider_val3()
    {
        Debug.Log(sliders[2].value);
        Links[2].transform.localEulerAngles = new Vector3(0f, 180f, sliders[2].value);
    }
}
