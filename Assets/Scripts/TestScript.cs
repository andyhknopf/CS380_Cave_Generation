using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEditor.Analytics;

 // Allows you to run scripts while the game in
 // [ExecuteInEditMode] 
public class TestScript : MonoBehaviour
{
  // Hash tables in C#
  private Dictionary<int, float> hashtable;

  // std::vector<int> in C#
  private List<int> stdVector;

  // Unity editor tools
  public GameObject testGameObject;
  public Light testLight;
  public float testValue1 = 3f;
  [SerializeField] float testValue2;
  // Enumerators in C#
  enum TestEnum
  {
    ENUM1,
    ENUM2
  }

  enum TestEnumType : int
  {
    INVALID = -1,
    OKAY = 0,
    GREAT = 1
  }

  // Extra return values using keyword 'out' in function signature
  void ChangeValue(out int valueToChange)
  {
    valueToChange = 5;
  }



  // A Unity-specific constructor, executes as soon as GameObject is enabled
  private void Awake()
  {
    GetLightReference();
  }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
        
  }

  
  // Update is called once per frame
  void Update()
  {
    // Basic input handling
    if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown((int)MouseButton.Left))
    {
      // Basic debug printing
      Debug.Log("TESTING");
    }

    // Pauses the editor if this condition is met
    bool fileIsInvalid = false;
    if (fileIsInvalid)
      Debug.Break();

    // Will throw an error in the Unity console
    Debug.Assert(false);
      
    // Using function arguments as return values
    int value = 0;
    Debug.Log(value);
    ChangeValue(out value);
    Debug.Log(value); // Value should be 5 by here
  }

  // Good for physics equations and mathematically deterministic functions
  private void FixedUpdate()
  {
    
  }

  void SpawnGameObject()
  {
    // Spawning a copy of a game object
    GameObject newObject = GameObject.Instantiate(testGameObject);
  }

  void GetLightReference()
  {
    testLight = testGameObject.GetComponent<Light>();
  }
}
