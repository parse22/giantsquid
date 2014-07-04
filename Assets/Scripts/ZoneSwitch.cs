using UnityEngine;
using System.Collections;

public class ZoneSwitch : MonoBehaviour {
    public int m_RegionNumber;
    void OnTriggerEnter(Collider other)
    {
        other.transform.gameObject.layer = LayerMask.NameToLayer("Fishy" + m_RegionNumber);
    }

    void OnTriggerExit(Collider other)
    {
        other.transform.gameObject.layer = LayerMask.NameToLayer("FishyOutside");
    }
}
