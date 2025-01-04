using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;
using static Lean.Pool.LeanGameObjectPool;

public class ZombieSpawner : MonoBehaviour
{

    public GameObject zombiePrefab; // Prefab của zombie
    public float radius;
    public int spawnQuantity;       // Số lượng zombie cần spawn
    public int _spawnQuantity;       // Số lượng zombie cần spawn
    public float spawnInterval;     // Khoảng thời gian giữa các lần spawn
    public Transform player;

    private List<GameObject> spawnedZombies = new List<GameObject>();  // Danh sách lưu trữ các zombie đã spawn

    public TMP_Text ZombieCount_txt;
    public TMP_Text kill_ZombieCount_txt;
    public UnityEvent youWin;
    private int killedZombieCount;
    bool attackPlayer =false;
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

        // Kiểm tra nếu player nằm trong vòng tròn
        if (player != null && Vector3.Distance(player.position, transform.position) <= radius)
        {
            // Bắt đầu spawn zombie nếu player vào vòng tròn
            if (spawnQuantity > 0&&!attackPlayer)
            {
                StartCoroutine(SpawnZombiesByTime());
                attackPlayer=true;
            }
        }
    }

#endif
    // Bắt đầu coroutine khi script khởi động
    private void Start()
    {
        _spawnQuantity = spawnQuantity;
        ZombieCount_txt.text = GetZombieCount().ToString();
        // StartCoroutine(SpawnZombiesByTime());
    }
    private void Update()
    {
        ZombieCount_txt.text = GetZombieCount().ToString();
        GetZombieCount();
        kill_ZombieCount_txt.text = GetKilledZombieCount().ToString();
        if (GetKilledZombieCount() == _spawnQuantity)
        {
            // Debug.Log($"{GetKilledZombieCount()} : {_spawnQuantity}");
            Invoke("OnWin", 2f);
            //  youWin.Invoke();
        }
    }

    // Coroutine để spawn zombie theo thời gian
    private IEnumerator SpawnZombiesByTime()
    {
        while (spawnQuantity > 0)
        {   
             yield return new WaitForSeconds(spawnInterval);
          
           //ZombieCount_txt.text = GetZombieCount().ToString();
              SpawnZombie();
          
           
          
        }
    }

    // Phương thức spawn một zombie
    private void SpawnZombie()
    {
        // TẠI 1 VI TRÍ

        GameObject zombie = Instantiate(zombiePrefab, transform.position, transform.rotation);
        spawnedZombies.Add(zombie);
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

    public int GetZombieCount()
    {
        foreach (var zombie in spawnedZombies)
        {
            if (zombie.GetComponent<Health>().healthPoint <= 0)
            {
                killedZombieCount++; // Tăng số lượng zombie bị hạ gục
            }
        }


        spawnedZombies.RemoveAll(z => z.GetComponent<Health>().healthPoint <= 0);




        return spawnedZombies.Count;
    }
    public int GetKilledZombieCount()
    {
        return killedZombieCount; // Trả về số lượng zombie đã bị hạ gục
    }
    public void OnWin()
    {
        youWin.Invoke();
    }
}
