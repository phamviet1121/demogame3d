//using UnityEngine;

//public class Shooter : MonoBehaviour
//{
//    public GameObject grenadePrefab;    // Prefab của lựu đạn
//    public Transform throwPoint;       // Điểm ném (vị trí xuất phát của lựu đạn)
//    public float throwForce = 10f;     // Lực ném
//    public float throwAngle;     // Góc ném (theo độ)
//    public GameObject gocnem;
//    // Update được gọi mỗi frame
//    void Update()
//    {
//         throwAngle = gocnem.transform.eulerAngles.x;
//        // Chuyển đổi góc về khoảng -180 đến 180
//        if (throwAngle > 180)
//        {
//            throwAngle -= 360;
//        }

//        // Kiểm tra xem người chơi có nhấn phím "J" không
//        if (Input.GetKeyDown(KeyCode.J))
//        {
//            ThrowGrenade();
//        }
//    }

//    void ThrowGrenade()
//    {
//        // Tạo lựu đạn tại vị trí ném
//        GameObject grenade = Instantiate(grenadePrefab, throwPoint.position, throwPoint.rotation);

//        // Lấy Rigidbody của lựu đạn
//        Rigidbody rb = grenade.GetComponent<Rigidbody>();

//        if (rb != null)
//        {
//            // Tính toán vận tốc theo góc ném
//            float angleInRadians = (throwAngle * Mathf.Deg2Rad)-2f; // Chuyển góc sang radian
//            Debug.Log($"{angleInRadians}");
//            // Tính toán hướng ném
//            // Tính toán hướng ném
//            Vector3 forward = throwPoint.forward;  // Hướng chính (XZ)
//            Vector3 upward = throwPoint.up;        // Hướng lên (Y)
//            Vector3 throwDirection = (forward * Mathf.Cos(angleInRadians)) + (upward * Mathf.Sin(-angleInRadians));
//                            // Hướng phía trước
//            //Vector3 throwDirection = Quaternion.Euler(-throwAngle, 0, 0) * forward; // Tạo hướng theo góc ném

//            // Áp dụng vận tốc ban đầu cho lựu đạn
//            //Vector3 velocity = throwDirection.normalized * throwForce;
//            //rb.velocity = velocity;
//            rb.velocity = throwDirection.normalized * throwForce;
//        }
//    }
//}
using UnityEngine;
using TMPro;
public class Shooter : MonoBehaviour
{
    public GameObject grenadePrefab;    // Prefab của lựu đạn
    public Transform throwPoint;       // Điểm ném (vị trí xuất phát của lựu đạn)
    public float throwForce = 500f;    // Lực ném (được áp dụng trực tiếp)
    //public float throwAngle;           // Góc ném (theo độ)
    //public GameObject gocnem;          // Đối tượng để lấy góc ném
    //public float a;

    public int soviendan;
    public int newsoviendan = 6;
    public float thoigiannapdan = 3f;
    private bool isReloading = false;
    public TextMeshProUGUI soviendan_txt;
    public GameObject thaydan_txt;
    public float khoangthoigianbandan;
    public float newkhoangthoigianbandan=0.3f;

    private void Start()
    {
        soviendan = newsoviendan;
        thaydan_txt.SetActive(false);
        soviendan_txt.text = soviendan.ToString();

    }
    void Update()
    {
        // Lấy góc ném từ trục X của gocnem
        // throwAngle = gocnem.transform.eulerAngles.x;

        //// Chuyển đổi góc về khoảng - 180 đến 180
        // if (throwAngle > 180)
        // {
        //     throwAngle -= 360;
        // }

        khoangthoigianbandan-=Time.deltaTime;
        // Kiểm tra xem người chơi có nhấn phím "J" không
        if (Input.GetKeyDown(KeyCode.J)&&soviendan>=1&& !isReloading&& khoangthoigianbandan<=0)
        {
            soviendan -= 1;
            ThrowGrenade();
            Debug.Log($"{soviendan}");
            soviendan_txt.text= soviendan.ToString();
            khoangthoigianbandan = newkhoangthoigianbandan;
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
        thaydan_txt.SetActive(true);
        isReloading = true; // Đặt trạng thái nạp đạn
        Debug.Log("Đang nạp đạn...");
        Invoke("thaydan", thoigiannapdan); // Bắt đầu quá trình nạp đạn
    }
    void thaydan()
    {
        soviendan = newsoviendan;
        isReloading = false;
        thaydan_txt.SetActive(false);
        soviendan_txt.text = soviendan.ToString();

    }    

    void ThrowGrenade()
    {
        // Tạo lựu đạn tại vị trí ném
        GameObject grenade = Instantiate(grenadePrefab, throwPoint.position, throwPoint.rotation);

        // Lấy Rigidbody của lựu đạn
         //Rigidbody rb = grenade.GetComponent<Rigidbody>();

        //if (rb != null)
        //{
           // float adjustedAngle = throwAngle + a;
            // Tính toán hướng ném dựa trên góc ném và hướng của throwPoint
           // float angleInRadians = adjustedAngle * Mathf.Deg2Rad;

            // Tính toán hướng ném
           // Vector3 forward = throwPoint.forward;  // Hướng chính (XZ)
           // Vector3 upward = throwPoint.up;        // Hướng lên (Y)
           // Vector3 throwDirection = (forward * Mathf.Cos(angleInRadians)) + (upward * Mathf.Sin(-angleInRadians));

            // Áp dụng lực cố định theo hướng tính toán
           // rb.AddForce(throwDirection.normalized * throwForce);
           grenade.GetComponent<Rigidbody>().AddForce(throwPoint.forward * throwForce, ForceMode.Impulse);
       // }

       
    }
}
