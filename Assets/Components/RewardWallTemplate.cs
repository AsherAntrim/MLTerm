using UnityEngine;

public class RewardWallTemplate : MonoBehaviour {
    public GameObject rewardWall;

    void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawMesh(rewardWall.GetComponent<MeshFilter>().sharedMesh, transform.position, transform.rotation, rewardWall.transform.localScale);
    }
}
