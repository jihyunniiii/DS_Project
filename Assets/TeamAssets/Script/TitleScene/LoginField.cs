using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using BackEnd;
using UnityEngine.UI;

public class LoginField : MonoBehaviour
{
    private static LoginField instance;
    public GameObject id;
    public GameObject pw;
    public GameObject Certify_number;
    public GameObject Signup;
    public GameObject Signup_text;
    public GameObject login;
    public GameObject nicknametext;
    public GameObject Majorobject;
    public GameObject exitobject;
    public InputField idInput;
    public InputField pwInput;
    public InputField Certify_num;
    public InputField userNicknameInput;
    public InputField Major;

    int ran;
    string email = "dgu.ac.kr";
    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        // 모든 씬에서 유지
        DontDestroyOnLoad(this.gameObject);
    }
    //public InputField userIndateInput;
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

    public void Login() {
        if (idInput.text.Contains(email))
        {
            BackendReturnObject bro = Backend.BMember.CustomLogin(idInput.text, pwInput.text);
            if (bro.IsSuccess())
            {
                Debug.Log("로그인에 성공했습니다");
                
                GameObject.FindWithTag("FadeController").GetComponent<FadeInOut>().FadeToNext();
            }
        }
        else {
            Debug.Log("동국대 메일이 아닙니다.");
        }
    }

    public void Certify() {
        if (idInput.text.Contains(email) && pwInput.text.Length != 0)
        {
            BackendReturnObject bro = Backend.BMember.CustomLogin(idInput.text, pwInput.text);
            if (bro.IsSuccess())
            {
                Debug.Log("이미 가입된 아이디입니다.");
                Backend.BMember.Logout();
            }
            else
            {
                id.SetActive(false);
                pw.SetActive(false);
                Certify_number.SetActive(true);
                Signup.SetActive(true);
                Signup_text.SetActive(false);
                login.SetActive(false);
                nicknametext.SetActive(true);
                Majorobject.SetActive(true);
                exitobject.SetActive(false);
                ran = Random.Range(100000, 999999);

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("qmal789@dgu.ac.kr"); // 보내는사람
                mail.To.Add(idInput.text); // 받는 사람
                mail.Subject = "\"동심\"에서 회원가입 인증 메일을 보냈습니다.";
                mail.Body = "인증 번호 : " + ran + "\n 게임으로 돌아가 화면에 인증번호를 입력하세요.";
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                smtpServer.Port = 587;
                smtpServer.Credentials = new System.Net.NetworkCredential("qmal789@dgu.ac.kr", "a1562021!") as ICredentialsByHost; // 보내는사람 주소 및 비밀번호 확인
                smtpServer.EnableSsl = true;
                ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };
                smtpServer.Send(mail);
                Debug.Log("success");
            }
        }
    }
    public void Signin() {
        if (idInput.text.Contains(email))
        {
            if (ran == int.Parse(Certify_num.text) && Certify_num.text.Length != 0 && Major.text.Length != 0 && userNicknameInput.text.Length !=0)
            {
                
                BackendReturnObject bro = Backend.BMember.CustomSignUp(idInput.text, pwInput.text, Major.text);
                if (bro.IsSuccess())
                {
                    Debug.Log("CustomSignUp : " + bro);
                    Debug.Log("닉네임 업데이트" + Backend.BMember.UpdateNickname(userNicknameInput.text));
                    Backend.BMember.UpdateCustomEmail(idInput.text);
                    Debug.Log("회원가입에 성공했습니다");
                    Backend.BMember.CustomLogin(idInput.text, pwInput.text);
                    Param param = new Param();
                    param.Add("Money", 10);
                    param.Add("Lantern", 0);
                    param.Add("Major", Major.text);
                    param.Add("Nickname", userNicknameInput.text);
                    Backend.GameData.Insert("user", param);
                    Backend.BMember.Logout();
                    id.SetActive(true);
                    pw.SetActive(true);
                    Certify_number.SetActive(false);
                    Signup.SetActive(false);
                    Signup_text.SetActive(true);
                    login.SetActive(true);
                    nicknametext.SetActive(false);
                    Majorobject.SetActive(false);
                    exitobject.SetActive(true);
                }
            }
            else {
                Debug.Log("인증 번호가 다르거나 빈칸이 있습니다.");
            }
        }
        else {
            Debug.Log("동국대 메일이 아닙니다.");
        }
    }
    public void LogOut()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}