using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Couponevent : MonoBehaviour
{
    public Camera getCamera;
    private RaycastHit hit;
    public GameObject CouponUI;
    public GameObject lobbyscript;
    public int couponcheck = 0;
    private static Couponevent instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null) { 
            Destroy(instance);
        }
        instance = this;
    }
    public static Couponevent GetInstance() { 
        return instance;
    }
    // Update is called once per frame
    public void GetCoupon() {
        if (couponcheck == 0)
        {
            Debug.Log("쿠폰을 획득합니다.");
            couponcheck = 1;
            CouponUI.gameObject.SetActive(true);
        }
        else if (couponcheck == 1) {
            Debug.Log("쿠폰을 이미 획득하셨습니다. 쿠폰은 하루에 한 장만 받을 수 있습니다.");
        }
    }
    public void CheckButtonClick()
    {
        CouponUI.gameObject.SetActive(false);
    }
}
