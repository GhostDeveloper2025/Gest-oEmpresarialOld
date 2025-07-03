using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Services
{
    //public class CredenciaisService
    //{
    //    private const string RegistryPath = @"SOFTWARE\GestaoEmpresarial";

    //    public void SalvarCredenciais(string usuario, string senha)
    //    {
    //        try
    //        {
    //            var key = Registry.CurrentUser.CreateSubKey(RegistryPath);
    //            if (key != null)
    //            {
    //                key.SetValue("Usuario", usuario);
    //                key.SetValue("Senha", Encrypt(senha));
    //                key.Close();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception($"Erro ao salvar credenciais: {ex.Message}");
    //        }
    //    }

    //    public (string usuario, string senha)? CarregarCredenciais()
    //    {
    //        try
    //        {
    //            var key = Registry.CurrentUser.OpenSubKey(RegistryPath);
    //            if (key != null)
    //            {
    //                string usuario = key.GetValue("Usuario") as string ?? "";
    //                string senhaCriptografada = key.GetValue("Senha") as string ?? "";

    //                key.Close();

    //                if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(senhaCriptografada))
    //                {
    //                    return (usuario, Decrypt(senhaCriptografada));
    //                }
    //            }
    //        }
    //        catch
    //        {
    //            return null;
    //        }
    //        return null;
    //    }

    //    public void LimparCredenciais()
    //    {
    //        try
    //        {
    //            var key = Registry.CurrentUser.OpenSubKey(RegistryPath, true);
    //            if (key != null)
    //            {
    //                key.DeleteValue("Usuario", false);
    //                key.DeleteValue("Senha", false);
    //                key.Close();
    //            }
    //        }
    //        catch { }
    //    }

    //    private string Encrypt(string data)
    //    {
    //        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
    //        byte[] encryptedBytes = ProtectedData.Protect(dataBytes, null, DataProtectionScope.CurrentUser);
    //        return Convert.ToBase64String(encryptedBytes);
    //    }

    //    private string Decrypt(string encryptedData)
    //    {
    //        byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
    //        byte[] decryptedBytes = ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);
    //        return Encoding.UTF8.GetString(decryptedBytes);
    //    }
    //}
}
