﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace AssetLoader.Bundle
{
    public class FileCryptogram
    {
        public class Offset
        {
            /// <summary>
            /// Offset 加密檔案 【檢測OK】
            /// </summary>
            /// <param name="sourceFile"></param>
            /// <returns></returns>
            public static bool OffsetEncryptFile(string sourceFile, int dummySize = 0)
            {
                byte[] dataBytes = File.ReadAllBytes(sourceFile);
                int totalLength = dataBytes.Length + dummySize;
                byte[] offsetDatabytes = new byte[totalLength];
                for (int i = 0; i < totalLength; i++)
                {
                    if (dummySize > 0 && i < dummySize) offsetDatabytes[i] = (byte)(UnityEngine.Random.Range(0, 256));
                    else offsetDatabytes[i] = dataBytes[i - dummySize];
                }
                File.WriteAllBytes(sourceFile, offsetDatabytes);

                return true;
            }

            /// <summary>
            /// Offset 解密檔案 【檢測OK】
            /// </summary>
            /// <param name="encryptBytes"></param>
            /// <returns></returns>
            public static bool OffsetDecryptFile(string encryptFile, int dummySize = 0)
            {
                byte[] dataBytes = File.ReadAllBytes(encryptFile);
                int totalLength = dataBytes.Length - dummySize;
                byte[] offsetDatabytes = new byte[totalLength];
                Buffer.BlockCopy(dataBytes, dummySize, offsetDatabytes, 0, totalLength);
                File.WriteAllBytes(encryptFile, offsetDatabytes);

                return true;
            }

            /// <summary>
            /// Offset 解密檔案 【檢測OK】
            /// </summary>
            /// <param name="encryptBytes"></param>
            /// <returns></returns>
            public static bool OffsetDecryptFile(ref byte[] encryptBytes, int dummySize = 0)
            {
                int totalLength = encryptBytes.Length - dummySize;
                byte[] offsetDatabytes = new byte[totalLength];
                Buffer.BlockCopy(encryptBytes, dummySize, offsetDatabytes, 0, totalLength);
                encryptBytes = offsetDatabytes;

                return true;
            }

            /// <summary>
            /// 返回 Offset 解密 Stream 【檢測OK】
            /// </summary>
            /// <param name="encryptFile"></param>
            /// <returns></returns>
            public static Stream OffsetDecryptStream(string encryptFile, int dummySize = 0)
            {
                var fsDescrypt = new FileStream(encryptFile, FileMode.Open, FileAccess.Read, FileShare.None);
                var dataBytes = new byte[fsDescrypt.Length - dummySize];
                fsDescrypt.Seek(dummySize, SeekOrigin.Begin);
                fsDescrypt.Read(dataBytes, 0, dataBytes.Length);
                fsDescrypt.Dispose();

                var msDescrypt = new MemoryStream();
                for (int i = 0; i < dataBytes.Length; i++)
                {
                    msDescrypt.WriteByte(dataBytes[i]);
                }

                return msDescrypt;
            }
        }

        public class XOR
        {
            /// <summary>
            /// XOR 加密檔案 【檢測OK】
            /// </summary>
            /// <param name="sourceFile"></param>
            /// <returns></returns>
            public static bool XorEncryptFile(string sourceFile, byte key = 0)
            {
                byte[] dataBytes = File.ReadAllBytes(sourceFile);
                for (int i = 0; i < dataBytes.Length; i++)
                {
                    dataBytes[i] ^= key;
                }
                File.WriteAllBytes(sourceFile, dataBytes);

                return true;
            }

            /// <summary>
            /// XOR 解密檔案 【檢測OK】
            /// </summary>
            /// <param name="encryptBytes"></param>
            /// <returns></returns>
            public static bool XorDecryptFile(byte[] encryptBytes, byte key = 0)
            {
                for (int i = 0; i < encryptBytes.Length; i++)
                {
                    encryptBytes[i] ^= key;
                }

                return true;
            }

            /// <summary>
            /// XOR 解密檔案 【檢測OK】
            /// </summary>
            /// <param name="encryptFile"></param>
            /// <returns></returns>
            public static bool XorDecryptFile(string encryptFile, byte key = 0)
            {
                byte[] dataBytes = File.ReadAllBytes(encryptFile);
                for (int i = 0; i < dataBytes.Length; i++)
                {
                    dataBytes[i] ^= key;
                }
                File.WriteAllBytes(encryptFile, dataBytes);

                return true;
            }

            /// <summary>
            /// 返回 XOR 解密 Stream 【檢測OK】
            /// </summary>
            /// <param name="encryptFile"></param>
            /// <returns></returns>
            public static Stream XorDecryptStream(string encryptFile, byte key = 0)
            {
                var fsDescrypt = new FileStream(encryptFile, FileMode.Open, FileAccess.Read, FileShare.None);
                var dataBytes = new byte[fsDescrypt.Length];
                fsDescrypt.Read(dataBytes, 0, dataBytes.Length);
                fsDescrypt.Dispose();

                var msDescrypt = new MemoryStream();
                for (int i = 0; i < dataBytes.Length; i++)
                {
                    dataBytes[i] ^= key;
                    msDescrypt.WriteByte(dataBytes[i]);
                }

                return msDescrypt;
            }
        }

        public class AES
        {
            /// <summary>
            /// AES 加密檔案 【檢測OK】
            /// </summary>
            /// <param name="sourceFile"></param>
            /// <param name="key"></param>
            /// <param name="iv"></param>
            /// <returns></returns>
            public static bool AesEncryptFile(string sourceFile, string key = null, string iv = null)
            {
                if (string.IsNullOrEmpty(sourceFile) || !File.Exists(sourceFile))
                {
                    return false;
                }

                try
                {
                    AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                    MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                    SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                    byte[] keyData = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
                    byte[] ivData = md5.ComputeHash(Encoding.UTF8.GetBytes(iv));
                    aes.Key = keyData;
                    aes.IV = ivData;

                    using (FileStream fsSource = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        byte[] dataBytes = new byte[fsSource.Length];
                        fsSource.Read(dataBytes, 0, dataBytes.Length);
                        fsSource.Dispose();
                        File.Delete(sourceFile);

                        using (FileStream fsEncrypt = new FileStream(sourceFile, FileMode.Create, FileAccess.Write))
                        {
                            //檔案加密
                            using (CryptoStream cs = new CryptoStream(fsEncrypt, aes.CreateEncryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(dataBytes, 0, dataBytes.Length);
                                cs.FlushFinalBlock();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log($"<color=#FF0000>File Encrypt failed.</color> {ex}");
                    return false;
                }

                return true;
            }

            /// <summary>
            /// AES 解密檔案 【檢測OK】
            /// </summary>
            /// <param name="encryptBytes"></param>
            /// <param name="key"></param>
            /// <param name="iv"></param>
            /// <returns></returns>
            public static bool AesDecryptFile(byte[] encryptBytes, string key = null, string iv = null)
            {
                try
                {
                    AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                    MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                    SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                    byte[] keyData = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
                    byte[] ivData = md5.ComputeHash(Encoding.UTF8.GetBytes(iv));
                    aes.Key = keyData;
                    aes.IV = ivData;

                    using (MemoryStream msEncrypt = new MemoryStream(encryptBytes))
                    {
                        // 檔案解密
                        using (CryptoStream cs = new CryptoStream(msEncrypt, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            int idx = 0;
                            int data;
                            while ((data = cs.ReadByte()) != -1)
                            {
                                encryptBytes[idx] = (byte)data;
                                idx++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log($"<color=#FF0000>File Decrypt failed.</color> {ex}");
                    return false;
                }

                return true;
            }

            /// <summary>
            /// AES 解密檔案 【檢測OK】
            /// </summary>
            /// <param name="encryptFile"></param>
            /// <param name="key"></param>
            /// <param name="iv"></param>
            /// <returns></returns>
            public static bool AesDecryptFile(string encryptFile, string key = null, string iv = null)
            {
                try
                {
                    AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                    MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                    SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                    byte[] keyData = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
                    byte[] ivData = md5.ComputeHash(Encoding.UTF8.GetBytes(iv));
                    aes.Key = keyData;
                    aes.IV = ivData;

                    using (FileStream fsSource = new FileStream(encryptFile, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        byte[] dataBytes = new byte[fsSource.Length];
                        fsSource.Read(dataBytes, 0, dataBytes.Length);
                        fsSource.Dispose();
                        File.Delete(encryptFile);

                        using (FileStream fsDecrypt = new FileStream(encryptFile, FileMode.Create, FileAccess.Write))
                        {
                            //檔案解密
                            using (CryptoStream cs = new CryptoStream(fsDecrypt, aes.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(dataBytes, 0, dataBytes.Length);
                                cs.FlushFinalBlock();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log($"<color=#FF0000>File Decrypt failed.</color> {ex}");
                    return false;
                }

                return true;
            }

            /// <summary>
            /// 返回 AES 解密 Stream 【檢測OK】
            /// </summary>
            /// <param name="encryptFile"></param>
            /// <param name="key"></param>
            /// <param name="iv"></param>
            /// <returns></returns>
            public static Stream AesDecryptStream(string encryptFile, string key = null, string iv = null)
            {
                MemoryStream msDecrypt;

                try
                {
                    AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                    MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                    SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                    byte[] keyData = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
                    byte[] ivData = md5.ComputeHash(Encoding.UTF8.GetBytes(iv));
                    aes.Key = keyData;
                    aes.IV = ivData;

                    using (FileStream fsDecrypt = new FileStream(encryptFile, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        // 檔案解密
                        using (CryptoStream cs = new CryptoStream(fsDecrypt, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            msDecrypt = new MemoryStream();
                            int data;
                            while ((data = cs.ReadByte()) != -1) msDecrypt.WriteByte((byte)data);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Debug.Log($"<color=#FF0000>File Decrypt failed.</color> {ex}");
                    return null;
                }

                return msDecrypt;
            }
        }
    }
}