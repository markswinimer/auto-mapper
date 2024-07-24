// using System.Collections.Generic;
// using UnityEngine;

// public class RoomPrefabManager : MonoBehaviour
// {
//     public static RoomPrefabManager Instance { get; private set; }

//     private Dictionary<RoomType, string> prefabDictionary;

//     void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//             InitializeDictionary();
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     private void InitializeDictionary()
//     {
//         prefabDictionary = new Dictionary<RoomType, string>()
//         {
//             { RoomType.Shop, "shop_desert01" },
//             { RoomType.Battle, "battle_desert01" },
//             { RoomType.Event, "event_desert01" }
//         };
//     }

//     public string GetPrefabName(RoomType type)
//     {
//         if (prefabDictionary.TryGetValue(type, out string prefabName))
//         {
//             return prefabName;
//         }
//         else
//         {
//             Debug.LogError("Prefab not found for type: " + type.ToString());
//             return null;
//         }
//     }
// }
