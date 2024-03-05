using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecificationHolder : MonoBehaviour
{
[field: SerializeField] public TMP_Text Name { get; private set; }
[field: SerializeField] public List<Image> Updates { get; private set; } = new();
[field: SerializeField] public TMP_Text History { get; private set; }
[field: SerializeField] public TMP_Text Health { get; private set; }
[field: SerializeField] public TMP_Text Armour { get; private set; }
[field: SerializeField] public TMP_Text Speed { get; private set; }
[field: SerializeField] public TMP_Text Ammo { get; private set; }

[field: SerializeField] public TMP_Text FirstSkillName { get; private set; }
[field: SerializeField] public TMP_Text FirstSkillCooldown { get; private set; }
[field: SerializeField] public TMP_Text FirstSkillDescription { get; private set; }

[field: SerializeField] public TMP_Text SecondSkillName { get; private set; }
[field: SerializeField] public TMP_Text SecondSkillCount { get; private set; }
[field: SerializeField] public TMP_Text SecondSkillDescription { get; private set; }
}
