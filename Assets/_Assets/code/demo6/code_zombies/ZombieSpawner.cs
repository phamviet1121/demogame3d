using UnityEngine;
using System.Collections;
using UnityEditor;

public class ZombieSpawner : MonoBehaviour
{
   
    public GameObject zombiePrefab; // Prefab của zombie
    public float radius;
    public int spawnQuantity;       // Số lượng zombie cần spawn
    public float spawnInterval;     // Khoảng thời gian giữa các lần spawn

    // Hiển thị vị trí spawn trong Scene bằng Gizmos
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
    private void OnDrawGizmosSelected()
    {
        Handles.color = new Color(1, 0, 0, 0.1f);
        Handles.DrawSolidDisc(transform.position, Vector3.up, radius);
    }    

#endif
    // Bắt đầu coroutine khi script khởi động
    private void Start() => StartCoroutine(SpawnZombiesByTime());

    // Coroutine để spawn zombie theo thời gian
    private IEnumerator SpawnZombiesByTime()
    {
        while (spawnQuantity > 0)
        {
            SpawnZombie();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Phương thức spawn một zombie
    private void SpawnZombie()
    {
        // TẠI 1 VI TRÍ

        Instantiate(zombiePrefab, transform.position, transform.rotation);
        spawnQuantity--;

        //TÀI VỊ TRÍ NGẪU NHIÊN ; TRONG BÁN KÍNH radius

        //  Vector3 randomPosition = transform.position + new Vector3(
        //    Random.Range(-radius, radius), 
        //    0,                             
        //    Random.Range(-radius, radius)  

        //);

        //  Instantiate(zombiePrefab, randomPosition, transform.rotation);
        //  spawnQuantity--;
    }
}
