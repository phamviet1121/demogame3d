using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class rifle_ray_shoot : MonoBehaviour
{
    public GameObject vitriban;
    public GameObject filerifle;
    public Transform diembanban;
    public GameObject banPrefab;

    public float timeToEmptyMagazine = 60f; // Thời gian để bắn hết một băng đạn (giây)
    private float fireRate; // Khoảng thời gian giữa các lần bắn 
    private float nextFireTime = 0f; // Thời gian tiếp theo cho phép bắn

    public int soviendan;
    public int newsoviendan =40;

    public float thoigiannapdan = 3f;
    private bool isReloading = false;

    public TextMeshProUGUI soviendan_txt;
    public GameObject thaydan_txt;

    public Animator amin;
    // Start is called before the first frame update
    void Start()
    {
        
        fireRate = timeToEmptyMagazine / newsoviendan;
        soviendan = newsoviendan;
        thaydan_txt.SetActive(false);
        soviendan_txt.text = soviendan.ToString();
       Debug.Log($"{fireRate}");
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0)) // Nhấp chuột trái
        //{
        // ray_shoot();
        //}
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime&& soviendan>=1 && !isReloading) // 0 là chuột trái
        {
            soviendan -= 1;
            nextFireTime = Time.time + fireRate; // Đặt thời gian bắn tiếp theo
            amin.SetTrigger("shoot");
          //  ray_shoot();
            soviendan_txt.text = soviendan.ToString();
        }
        if (soviendan <= 0 && !isReloading)
        {
            StartReloading();
        }
        if (Input.GetKeyDown(KeyCode.U) && !isReloading)
        {
            StartReloading();
        }
    }

    void StartReloading()
    {
        amin.SetTrigger("reload");
        thaydan_txt.SetActive(true);
        isReloading = true; // Đặt trạng thái nạp đạn
       // Debug.Log("Đang nạp đạn...");
        // Invoke("thaydan", thoigiannapdan); // Bắt đầu quá trình nạp đạn
    }
    void thaydan()
    {

        soviendan = newsoviendan;
        isReloading = false;
        thaydan_txt.SetActive(false);
        soviendan_txt.text = soviendan.ToString();

    }
    void teoluakhiban()
    {
        // GameObject ban = Instantiate(banPrefab, throwPoint.position, throwPoint.rotation);
        GameObject ban = Instantiate(banPrefab, diembanban.position, diembanban.rotation, diembanban);
    }
    void ray_shoot()
    {
        // Lấy vị trí của đối tượng được truyền vào
        Vector3 targetPosition = vitriban.transform.position;

        // Tạo tia từ camera đến vị trí đối tượng
        Vector3 rayDirection = (targetPosition - Camera.main.transform.position).normalized; // Hướng từ camera đến target
        Ray ray = new Ray(Camera.main.transform.position, rayDirection); // Tạo tia

        RaycastHit hit; // Thông tin va chạm

        // Bắn tia với khoảng cách tối đa là 100 đơn vị
        if (Physics.Raycast(ray, out hit, 100f,~0, QueryTriggerInteraction.Ignore))
        {
          //  Debug.Log($"Hit: {hit.collider.gameObject.name}"); // In tên đối tượng bị va chạm
           GameObject file= Instantiate(filerifle, hit.point, Quaternion.LookRotation(hit.normal));
        }

        // Vẽ tia để kiểm tra trong Scene view
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f); // Tia màu đỏ trong 2 giây
    }

}
