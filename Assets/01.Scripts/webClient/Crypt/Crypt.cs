using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
public class Crypt
{
	private RijndaelManaged Aes128;
	private RijndaelManaged Aes256;
	public Crypt(string key)
	{
		
        this.Aes256 = new RijndaelManaged();
		this.Aes256.BlockSize = 128;
		this.Aes256.KeySize = 256;
		this.Aes256.Mode = CipherMode.CBC;
		this.Aes256.Padding = PaddingMode.PKCS7;
        this.Aes256.Key = Encoding.UTF8.GetBytes(key);
        this.Aes256.IV = Encoding.UTF8.GetBytes("2efe25734b2955d4");
        
	}
	

    public String AESEncrypt256(String Input)
    {


        var encrypt = Aes256.CreateEncryptor(Aes256.Key, Aes256.IV);
        byte[] xBuff = null;
        using (var ms = new MemoryStream())
        {
            using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
            {
                byte[] xXml = Encoding.UTF8.GetBytes(Input);
                cs.Write(xXml, 0, xXml.Length);
            }

            xBuff = ms.ToArray();
        }

        String Output = Convert.ToBase64String(xBuff);
        return Output;
    }

    public String AESDecrypt256(String Input)
    {
        var decrypt = Aes256.CreateDecryptor();
        byte[] xBuff = null;
        using (var ms = new MemoryStream())
        {
            using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
            {
                byte[] xXml = Convert.FromBase64String(Input);
                cs.Write(xXml, 0, xXml.Length);
            }

            xBuff = ms.ToArray();
        }

        String Output = Encoding.UTF8.GetString(xBuff);
        return Output;
    }
	
	
}
