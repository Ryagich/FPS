using UnityEngine;
using UnityEngine.UI;

public class DeathUIHolder : MonoBehaviour
{
   public static DeathUIHolder Instance;
   
   [field: SerializeField] public Transform DeadScreen{ get; private set; }
   [field: SerializeField] public Button ContinueButton{ get; private set; }
   
   private void Awake()
   {
      Instance = this;
   }
}
