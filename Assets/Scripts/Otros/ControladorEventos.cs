using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// Patrón delegado, contendrá las referencias a las funciones
/// </summary>
public delegate void FuncionDelegado(EventoBase message);

/// <summary>
/// Clase que representa el enlace entre las distintas capas para permitir sus comunicaciones
/// </summary>
public class ControladorEventos : MonoBehaviour {

    public static ControladorEventos Instancia { get; private set; }

    private Dictionary<string, List<FuncionDelegado>> eventListenerList = new Dictionary<string, List<FuncionDelegado>>();
    private Queue<EventoBase> eventQueue = new Queue<EventoBase>();
    private float maxQueueProcessingTime = 0.16667f;
    private bool isAlive = true;

    private void Awake()
    {
        Singleton();
    }

    private void Singleton()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public bool SubscribirseEvento(System.Type type, FuncionDelegado delegateFunction)
    {
        if (type == null)
        {
            Debug.Log("EVENTCONTROLLER --> Ha fallado al añadir un listener");
            return false;
        }

        string eventMessageName = type.Name;

        if (!eventListenerList.ContainsKey(eventMessageName))
        {
            eventListenerList.Add(eventMessageName, new List<FuncionDelegado>());
        }

        List<FuncionDelegado> listenerList = eventListenerList[eventMessageName];
        if (listenerList.Contains(delegateFunction))
        {
            return false; 
        }

        listenerList.Add(delegateFunction);
        return true;
    }

    public bool DesubscribirseEvento(System.Type type, FuncionDelegado delegateFunction)
    {
        if (type == null)
        {
            Debug.Log("EVENTCONTROLLER: DetachListener failed due to no message type specified");
            return false;
        }

        string eventMessageName = type.Name;

        if (!eventListenerList.ContainsKey(type.Name))
        {
            return false;
        }

        List<FuncionDelegado> listenerList = eventListenerList[eventMessageName];
        if (!listenerList.Contains(delegateFunction))
        {
            return false;
        }

        listenerList.Remove(delegateFunction);
        return true;
    }

    public bool QueueMessage(EventoBase eventMessage)
    {
        if (!eventListenerList.ContainsKey(eventMessage.nombreEvento))
        {
            return false;
        }
        eventQueue.Enqueue(eventMessage);
        return true;
    }

    void Update()
    {
        float timer = 0.0f;
        while (eventQueue.Count > 0)
        {
            if (maxQueueProcessingTime > 0.0f)
            {
                if (timer > maxQueueProcessingTime)
                    return;
            }

            EventoBase eventMessage = eventQueue.Dequeue();
            if (!LanzarEvento(eventMessage))
                Debug.Log("EVENTCONTROLLER --> " + eventMessage.nombreEvento);

            if (maxQueueProcessingTime > 0.0f)
                timer += Time.deltaTime;
        }
    }

    public bool LanzarEvento(EventoBase eventMessage)
    {
        string eventMessageName = eventMessage.nombreEvento;
        if (!eventListenerList.ContainsKey(eventMessageName))
        {
            Debug.Log("EVENTCONTROLLER: Message \"" + eventMessageName + "\" has no listeners!");
            return false; // no listeners for messae so ignore it
        }

        List<FuncionDelegado> listenerList = eventListenerList[eventMessageName];

        for (int i = 0; i < listenerList.Count; ++i)
        {
            listenerList[i](eventMessage);
        }

        return true;
    }

    public static bool IsAlive
    {
        get
        {
            if (Instancia == null)
                return false;
            return Instancia.isAlive;
        }
    }

    void OnDestroy()
    {
        isAlive = false;
    }

    void OnApplicationQuit()
    {
        isAlive = false;
    }



}
