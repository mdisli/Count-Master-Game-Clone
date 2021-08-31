using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;

public class LevelCreating : EditorWindow
{
    GameObject planePrefab, finishPrefab, multiplierPrefab;
    
    List<GameObject> obstaclePrefabs = new List<GameObject>();
    List<GameObject> multipliers = new List<GameObject>();
    List<String> multipliersVariables = new List<string>();

    int planeLength, obstacleCount, obstacleCountForSpawn, multiplierCount, multiplierCountForSpawn;

    private Vector3 startPos = new Vector3(0,0,0);
    private Transform groundHolder, multiplierHolder, obstacleHolder;
    
   [MenuItem("Window/Level Creater")]
   public static void ShowWindow()
   {
      GetWindow<LevelCreating>("Level Creater");
   }
   private void OnGUI()
   {
      //  PREFABS
      TakePrefab();
      
      // COUNTS 
      TakeCount();
      
      // BUILD 

      if (GUILayout.Button("Build Level"))
      {
         BuildLevel();
      }
      
      TakeMultipliersVariables();
      
      if (GUILayout.Button("Change Multiplier Trigger Names"))
      {
         ChangeMultiplierTriggerNames();
      }
      
   }

   void TakePrefab()
   {
      GUILayout.Label("----------------------------",EditorStyles.boldLabel);
      GUILayout.Label("PREFABS",EditorStyles.boldLabel);
      GUILayout.Label("----------------------------",EditorStyles.boldLabel);
      
      planePrefab = EditorGUILayout.ObjectField("Plane Prefab : ", planePrefab, typeof(GameObject),GUILayout.MaxWidth(350)) as GameObject;
      finishPrefab = EditorGUILayout.ObjectField("Finish Prefab : ", finishPrefab, typeof(GameObject), GUILayout.MaxWidth(350)) as GameObject;
      multiplierPrefab = EditorGUILayout.ObjectField("Multiplier Prefab : ", multiplierPrefab, typeof(GameObject), GUILayout.MaxWidth(350)) as GameObject;
      
      obstacleCount = Mathf.Max(0, EditorGUILayout.IntField("Obstacle Prefabs :", obstaclePrefabs.Count));
      
      // OBSTACLE PREFAB

      while (obstacleCount > obstaclePrefabs.Count)
      {
         obstaclePrefabs.Add(null);
      }

      while (obstacleCount < obstaclePrefabs.Count)
      {
         obstaclePrefabs.RemoveAt(obstaclePrefabs.Count - 1);
      }

      for (int i = 0; i < obstaclePrefabs.Count; i++)
      {
         obstaclePrefabs[i] = EditorGUILayout.ObjectField("Item " + i, obstaclePrefabs[i], typeof(GameObject),true) as GameObject;
      }
      
      
   }

   void TakeCount()
   {
      GUILayout.Label("----------------------------",EditorStyles.boldLabel);
      GUILayout.Label("COUNTS",EditorStyles.boldLabel);
      GUILayout.Label("----------------------------",EditorStyles.boldLabel);

      planeLength = EditorGUILayout.IntField("Plane Length: ", planeLength);
      obstacleCountForSpawn = EditorGUILayout.IntField("Obstacle Count: ", obstacleCountForSpawn);
      multiplierCountForSpawn = EditorGUILayout.IntField("Multiplicator Count: ", multiplierCountForSpawn);
   }

   void BuildLevel()
   {
      multipliers.Clear();
      
      // HOLDERS
      groundHolder = GameObject.Find("Ground Holder").transform;
      multiplierHolder = GameObject.Find("Multiplicator Holder").transform;
      obstacleHolder = GameObject.Find("Obstacle Holder").transform;
      
      // POSITIONS
      Vector3 groundPos = startPos;
      Vector3 obsPos = startPos + new Vector3(0, 2.4f, 15);
      Vector3 multiplierPos = startPos + new Vector3(0, 0, 15);
      
      GUILayout.Label("----------------------------",EditorStyles.boldLabel);

      // PLANE INSTANTIATING
      for (int i = 0; i < planeLength; i++)
      {
         Instantiate(planePrefab, groundPos, Quaternion.identity, groundHolder);
         groundPos += new Vector3(0, 0, 10);
      }
      
      // FINISH INSTANTIATING
      Instantiate(finishPrefab, groundPos, Quaternion.identity, groundHolder);
      
      // OBSTACLE INSTANTIATING
      for (int i = 0; i < obstacleCountForSpawn; i++)
      {
         int randomNum = Random.Range(0, obstacleCount);
         Instantiate(obstaclePrefabs[randomNum], obsPos + new Vector3(0,-2.14f,0), Quaternion.identity, obstacleHolder);
         obsPos += new Vector3(0, 0, 30);
      }

      // MULTIPLIER INSTANTIATING
      startPos = startPos + new Vector3(0, 2.4f, 15);
      for (int i = 0; i < multiplierCountForSpawn; i++)
      {
         GameObject multiplier = Instantiate(multiplierPrefab, startPos, Quaternion.identity, multiplierHolder);
         startPos += new Vector3(0, 0, 30);
         multipliers.Add(multiplier);
      }
   }
   void TakeMultipliersVariables()
   {
      multiplierCount = Mathf.Max(0, EditorGUILayout.IntField("Multiplier Variables :", multiplierCount));
      
      while (multiplierCount > multipliersVariables.Count)
      {
         multipliersVariables.Add(null);
      }

      while (multiplierCount < multipliersVariables.Count)
      {
         multipliersVariables.RemoveAt(multipliersVariables.Count - 1);
      }

      for (int i = 0; i < multipliersVariables.Count; i++)
      {
         multipliersVariables[i] = EditorGUILayout.TextField("Item " + i, multipliersVariables[i]) as string;
      }
   }
   static public GameObject getChildGameObject(GameObject fromGameObject, string withName) {
      //Author: Isaac Dart, June-13.
      Rigidbody[] ts = fromGameObject.GetComponentsInChildren<Rigidbody>();
      foreach (Rigidbody t in ts) if (t.gameObject.name == withName) return t.gameObject;
      return null;
   }

   void ChangeMultiplierTriggerNames()
   {
      int a = 0;
      for (int i = 0; i < multipliersVariables.Count; i++)
      {
         getChildGameObject(multipliers[i], "Trigger").transform.name = multipliersVariables[a];
         getChildGameObject(multipliers[i], "Trigger 1").transform.name = multipliersVariables[a+1];
         a += 2;
      }
   }

   
}
