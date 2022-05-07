using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEngine.UI;

public class LoginField : MonoBehaviour
{
    public InputField idInput;
    public InputField pwInput;
    public InputField myIndateInput;
    public InputField userNicknameInput;

    string email = "dgu.ac.kr";
    //public InputField userIndateInput;

    public Text nickname;
    void Start()
    {
        var bro = Backend.Initialize(true);
        if (bro.IsSuccess())
        {
            Debug.Log("초기화 성공");
        }
        else
        {
            Debug.Log("초기화 실패!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Backend.AsyncPoll();
    }

    public void SignInAndLogin()
    {
        // Debug.Log(Backend.BMember.Logout());
        if (idInput.text.Contains(email))
        {
            var result = Backend.BMember.CustomLogin(idInput.text, pwInput.text);

            if (result.IsSuccess())
            {
                Debug.Log("CustomLogin : " + result);
                GetNickName();
            }
            else
            {
                Debug.Log("로그인에 실패하셨습니다\n회원가입을 시도합니다 : " + result);

                result = Backend.BMember.CustomSignUp(idInput.text, idInput.text);

                Debug.Log("CustomSignUp : " + result);

                Debug.Log("닉네임 업데이트" + Backend.BMember.UpdateNickname(idInput.text));

                nickname.text = idInput.text;
                Backend.BMember.UpdateCustomEmail(idInput.text);

            }
        }
        else {
            Debug.Log("동국대 메일이 아닙니다.");
        }
    }

   public void LogOut()
    {
        Debug.Log(Backend.BMember.Logout());
    }

    public void AutoLogin()
    {
        try
        {
            Debug.Log(Backend.BMember.Logout());
        }
        catch
        {

        }
        Debug.Log(Backend.BMember.LoginWithTheBackendToken());
        GetNickName();
    }

    void GetNickName()
    {
        nickname.text = Backend.UserNickName;
    }
    
}