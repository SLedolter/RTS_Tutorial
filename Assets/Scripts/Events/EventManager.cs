using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour {
  private Dictionary<string, UnityEvent> _events;
  private Dictionary<string, CustomEvent> _typedEvents;
  private static EventManager _eventManager;

  public static EventManager instance {
    get {
      if (!_eventManager) {
        _eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

        if (!_eventManager) {
          Debug.LogError("There needs to be one active EventManager script on a GameObject in the scene!");
        } else {
          _eventManager.Init();
        }
      }
      return _eventManager;
    }
  }

  void Init() {
    if (_events == null) {
      _events = new Dictionary<string, UnityEvent>();
      _typedEvents = new Dictionary<string, CustomEvent>();
    }
  }

  public static void AddListener(string eventName, UnityAction listener) {
    UnityEvent evt = null;
    if (instance._events.TryGetValue(eventName, out evt)) {
      evt.AddListener(listener);
    } else {
      evt = new UnityEvent();
      evt.AddListener(listener);
      instance._events.Add(eventName, evt);
    }
  }

  public static void RemoveListener(string eventName, UnityAction listener) {
    if (_eventManager == null) return;
    UnityEvent evt = null;
    if (instance._events.TryGetValue(eventName, out evt)) {
      evt.RemoveListener(listener);
    }
  }

  public static void TriggerEvent(string eventName) {
    UnityEvent evt = null;
    if(instance._events.TryGetValue(eventName,out evt)) {
      evt.Invoke();
    }
  }

  public static void AddTypedListener(string eventName, UnityAction<CustomEvent> listener) {
    CustomEvent evt = null;
    if(instance._typedEvents.TryGetValue(eventName, out evt)) {
      evt.AddListener(listener);
    } else {
      evt = new CustomEvent();
      evt.AddListener(listener);
      instance._typedEvents.Add(eventName, evt);
    }
  }

  public static void RemoveTypeListener(string eventName, UnityAction<CustomEvent> listener) { 
    if(_eventManager == null) return;
    CustomEvent evt = null;
    if(instance._typedEvents.TryGetValue(eventName,out evt)) {
      evt.RemoveListener(listener);
    }
  }

  public static void TriggerTypeEvent(string eventName, CustomEvent data) {
    CustomEvent evt = null;
    if(instance._typedEvents.TryGetValue(eventName, out evt)) {
      evt.Invoke(data);
    }
  }

  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }
}
